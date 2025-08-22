using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 駒を選択したときに駒の移動可能領域を展開する
/// </summary>
public class OpenSelectableArea : InGameManager
{
    Dictionary<string, PieceParameter> _pieceDic = new Dictionary<string, PieceParameter>(); //null
    GameObject _selectedPieceObj = null;
    Vector3 _selectedPiecePos = default(Vector3); //オブジェクトと常に等しい
    PieceParameter _selectedPieceParam = null;
    /// <summary>
    /// 駒の情報をDictionaryに登録する
    /// </summary>
    void Start()
    {
        if (_pieceDic.Count == 0)
        {
            for (int i = 0; i < _PieceParameters.Length; i++)
            {
                _pieceDic.Add(_PieceParameters[i]._PieceName, _PieceParameters[i]);
            }
        }
    }
    /// <summary>
    /// 選択された駒の種類を特定し、以下の動作を行う
    /// ①移動可能領域の展開
    /// ②FocusCameraの更新
    /// ③スキルの表示
    /// </summary>
    /// <param name="selectedPieceObj"></param>
    public void StartOpenArea(GameObject selectedPieceObj)
    {
        //処理負荷を低くする
        if (_selectedPieceObj != selectedPieceObj)
        {
            _selectedPieceObj = selectedPieceObj;
        }
        Animator animatorController = selectedPieceObj.GetComponent<Animator>();
        animatorController.Play("AddOneLine");
        //移動可能領域の展開に必要な要素
        //①移動可能な範囲を検索する → ノーマルタイルでかつ、駒の持つ移動可能範囲内であれば
        //②上下は0.5, 左右は1.0 の幅で次のタイルを取得できる
        //③順番に表示したいので中心座標が必要
        //絶対に必要なのは駒ごとの移動可能座標と、足し算を行う処理、そして方角の判断、Animationを繰り返し再生するint型、移動した先で敵のマスを検知したらそこから先のマスは表示しないようにする処理、そしてそのマスにいる駒はハイライトさせる処理・カーソルを合わせると赤色のタイルが表示される処理、移動した後に深緑のタイルが設定されるようにする
        //方角ごとで関数を呼び出せばよいのか？　とはいえ、それだとばらつきの出る恐れがある
        //AnimationEventが一番優秀かもしれない
        //AnimationEventをもう一度呼び出す必要がある場合は、LooptimeをOnにする
        
    }
    /// <summary>
    /// AnimationEventで駒の移動可能領域を一行拡大する
    /// </summary>
    public void AddOneLineOpenArea()
    {
        
        if (_selectedPiecePos != _selectedPieceObj.transform.position)
        {
            _selectedPiecePos = _selectedPieceObj.transform.position;
        }
        if (_selectedPieceParam._PieceName != _selectedPieceObj.tag)
        {
            _selectedPieceParam =  _pieceDic[_selectedPieceObj.tag];
        }
        List<Vector3Int> renderingOpenAreas = ReturnRenderingOpenArea(); //戻り値 == 選択可能領域 && 敵の駒の下にある || TileBase == _NomalTile
        // 深緑のマスを検知したときのみコライダーを出現させる
        //敵がいたからといって撃破できるわけではない + 敵がいないからといって撃破出来ないわけでもない → 敵を撃破できるか否か、が先である
        //攻撃可能範囲にコライダーを出現させ、敵がいたらそこのPositionと敵のオブジェクトをreturnする → 敵のオブジェクトのOutlineを変更する & 敵がいたタイルの座標をSelectConroller.csに送信しそこのポジションだけ赤タイルを表示するように設定する
        //移動可能範囲で検索をかけ、NomalTileの場合のみにPosiionをreturnする
        
        //行動可能領域がない or 範囲検索が一度で済む駒である場合に処理から抜け出す
        if (renderingOpenAreas.Count == 0
            ||
            _selectedPieceParam._IsMoveLimit)
        {
            return;
        }
        else
        {
            TilesRenderer(renderingOpenAreas);
        }
        //_selectedPieceObjを取得する
        //_selectedPieceObjの中身によって移動領域を確定させる
        //実際に移動できる範囲を検出するための関数を呼び出す
        //移動可能領域がなければreturn = null;で終了する
        //戻り値のコレクションから取り出して該当するVector3IntにTileBase を設定する
        //もう一度,AnimationClipを再生する
    }

    List<Vector3Int> ReturnRenderingOpenArea()
    {
        
        List<Vector3Int> renderingOpenAreas = new List<Vector3Int>();
        return renderingOpenAreas;
    }
    /// <summary>
    /// 引数に渡されたポジションにタイルを描画する
    /// </summary>
    void TilesRenderer(List<Vector3Int> renderingOpenAreas)
    {
        for (int i = 0; i < renderingOpenAreas.Count; i++)
        {
            _Tilemap.SetTile(renderingOpenAreas[i], _CanSelectedTileBase);
        }
    }
}
