using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 駒を選択したときに駒の移動可能領域を展開する → 完成してから非表示の機能を付けても良いかもね
/// </summary>
public class OpenSelectableArea : InGameManager
{
    AddPieceFunction _addPieceFunction;
    Dictionary<string, PieceParameter> _pieceDic = new Dictionary<string, PieceParameter>(); //null
    GameObject _selectedPieceObj = null;
    Vector3Int _selectedPiecePos = default(Vector3Int); //オブジェクトと常に等しい
    PieceParameter _selectedPieceParam = null;

    int _selectedPieceMoveCount = 0;

    List<Vector3Int> _renderingOpenAreas = new List<Vector3Int>();
    Vector3Int[] _attackAreaPositions = new Vector3Int[0];
    Vector3Int[] _moveAreaPositions = new Vector3Int[0];

    //Outline変更用
    List<GameObject> _enemyObjs = new List<GameObject>();
    //他のScriptに送信する用
    List<Vector3Int> _encouterTilePositions = new List<Vector3Int>();
    //勝手なルール
    //_attackAreaPositionsにおいて、z == 1 はこれ以上コライダーを出現させる意味がないことを表す
    //
    int _prefabCount = 0;
    int _PrefabCount{ get { return _prefabCount; } set { _prefabCount = value; if (_prefabCount <= 0) { DrowTiles();}}}
    /// <summary>
    /// 駒の情報をDictionaryに登録する
    /// </summary>
    void Start()
    {
        _addPieceFunction = GetComponent<AddPieceFunction>();
        if (_pieceDic.Count == 0)
        {
            for (int i = 0; i < _PieceParameters.Length; i++)
            {
                _pieceDic.Add(_PieceParameters[i].PieceName(), _PieceParameters[i]);
            }
        }
    }
    /// <summary>
    /// 選択された駒の種類を特定し、以下の動作を行う
    /// ①移動可能領域の展開
    /// ②FocusCameraの更新
    /// ③スキルの表示
    /// このメソッドは駒を選択した後に一度だけ実行される
    /// </summary>
    /// <param name="selectedPieceObj"></param>
    public void StartOpenArea(GameObject selectedPieceObj)
    {
        //処理負荷を低くする
        if (_selectedPieceObj != selectedPieceObj)
        {
            _selectedPieceObj = selectedPieceObj;
            _selectedPiecePos = _Tilemap.WorldToCell(selectedPieceObj.transform.position);
            if (!_selectedPieceParam
                ||
                _selectedPieceParam.PieceName() != _selectedPieceObj.tag)
            {
                _selectedPieceParam = _pieceDic[_selectedPieceObj.tag];
            }
            if (_selectedPieceParam.PieceName() == "P")
            {
                if (_selectedPieceObj.GetComponent<SpriteRenderer>().flipX)
                {
                    _selectedPieceParam = _addPieceFunction.UpdatePoneGroup(_selectedPieceParam);
                }
                if (_selectedPieceObj.transform.rotation.x == 0)
                {
                    _selectedPieceParam = _addPieceFunction.AddPoneMoveCount(_selectedPieceParam);
                    _selectedPieceObj.transform.rotation = Quaternion.Euler(360, 0, 0);
                };
            }
            _selectedPieceMoveCount = _selectedPieceParam.MoveCount();
        }
        Initialize();
        ActivePieceOutline(_selectedPieceObj.GetComponent<SpriteRenderer>());
        _AnimatorController.enabled = true;
        _AnimatorController.Play("AddOneLine");
    }
    /// <summary>
    /// AnimationEventで駒の移動可能領域を一行拡大する
    /// </summary>
    public void AddOneLineOpenArea()
    {
        JudgmentRenderingMoveArea();
        JudgmentRenderingAttackArea();
    }
    void JudgmentRenderingMoveArea()
    {
        for (int i = 0; i < _selectedPieceParam.MoveAreaPositions().Length; i++)
        {
            if (_moveAreaPositions[i] == default(Vector3Int))
            {
                _moveAreaPositions[i] = _selectedPiecePos;
            }
            _moveAreaPositions[i] += _selectedPieceParam.MoveAreaPositions()[i];
            TileBase moveAreaTileBase = _Tilemap.GetTile(_moveAreaPositions[i]);
            //移動範囲内のタイルがFieldTileでなかったなら、continueする
            if (moveAreaTileBase != _FieldTileBase)
            {
                continue;
            }
            _renderingOpenAreas.Add(_moveAreaPositions[i]);
        }
    }

    void JudgmentRenderingAttackArea()
    {
        _PrefabCount = _selectedPieceParam.AttackAreaPositions().Length;
        for (int i = 0; i < _selectedPieceParam.AttackAreaPositions().Length; i++)
        {
            if (_attackAreaPositions[i] == default)
            {
                _attackAreaPositions[i] = _selectedPiecePos;
            }
            _attackAreaPositions[i] += _selectedPieceParam.AttackAreaPositions()[i];
            TileBase attackAreaTileBase = _Tilemap.GetTile(_attackAreaPositions[i]);
            //攻撃範囲内のタイルがSelectedTileでなかったなら、continueする
            if (attackAreaTileBase != _SelectedTileBase
                ||
                _attackAreaPositions[i].z == 1)
            {
                _PrefabCount -= 1;
                continue;
            }
            Vector3 attackAreaWorldPosition = _Tilemap.CellToWorld(_attackAreaPositions[i]);
            GameObject boxCollider2DObj = Instantiate(_BoxCollider2DPrefab, attackAreaWorldPosition, Quaternion.identity); //ここで、別のメソッドが呼ばれるので注意
            float destroyTimer = i * 0.1f;
            Destroy(boxCollider2DObj, destroyTimer);
            int notSearchAxis = 1;
            _attackAreaPositions[i].z = notSearchAxis;
        }
    }

    /// <summary>
    /// 引数に渡されたポジションにタイルを描画する
    /// </summary>
    void DrowTiles()
    {
        Debug.Log(_selectedPieceMoveCount);
        if (_selectedPieceMoveCount <= 0
            ||
            _renderingOpenAreas.Count == 0)
        {
            _AnimatorController.enabled = false;
            StartTileSelect();
            return;
        }
        if (_renderingOpenAreas.Count != 0)
        {
            Debug.Log("");
            for (int i = 0; i < _renderingOpenAreas.Count; i++)
            {
                _Tilemap.SetTile(_renderingOpenAreas[i], _CanSelectedTileBase);
            }
        }
        _selectedPieceMoveCount -= 1;
        _renderingOpenAreas.Clear();
        _AnimatorController.Play("AddOneLine", 0, 0);
    }
    /// <summary>
    /// _attackAreaPositions, _moveAreaPositionsを初期化
    /// </summary>
    void Initialize()
    {
        _attackAreaPositions = new Vector3Int[_selectedPieceParam.AttackAreaPositions().Length];
        _moveAreaPositions = new Vector3Int[_selectedPieceParam.MoveAreaPositions().Length];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="spriteRenderer"></param>
    void ActivePieceOutline(SpriteRenderer spriteRenderer)
    {
        spriteRenderer.color = Color.white;
        spriteRenderer.material = Resources.Load<Material>("OutlineMaterial");
    }
    /// <summary>
    /// ReturnRenderingOpenArea() でInstance化したPrefabから衝突情報を受けて呼び出される
    /// 敵の駒があると判明されたマス目だけの処理をする
    /// protectedいらない
    /// </summary>
    /// <param name="collision2D"></param>
    protected void JudgmentPieceGroup(GameObject collisionObj, Transform prefabTrs) //一応、ここで取得された敵のオブジェクトを記憶しておくことも可能
    {
        //flipXから敵の駒であるか否かを判断する
        if (_selectedPieceObj.GetComponent<SpriteRenderer>().flipX != collisionObj.GetComponent<SpriteRenderer>().flipX)
        {
            Vector3Int enemyTilePos = _Tilemap.WorldToCell(prefabTrs.position);
            _renderingOpenAreas.Add(enemyTilePos);
            _encouterTilePositions.Add(enemyTilePos);
            ActivePieceOutline(collisionObj.GetComponent<SpriteRenderer>());
        }
        _PrefabCount -= 1;
    }
    void StartTileSelect()
    {
        GetComponent<SelectTileController>().SetPiecePositions(_selectedPiecePos, _encouterTilePositions);
        GetComponent<SelectTileController>().enabled = true;
        _encouterTilePositions.Clear(); //複雑になってきたらプロパティ化
    }
    public void TurnDesideRelay()
    {
        GetComponent<TurnDesideController>().StartTurnDeside();
    }
}