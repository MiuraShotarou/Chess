using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class SelectTileController : InGameManager
{
    Vector3Int _beforeTilePos;
    List<Vector3Int> EncouterTilePositions = new List<Vector3Int>();
    /// <summary>
    /// _beforeTilePosにカーソルのポジションを代入する
    /// 
    /// ※現状、_canSelectedTileBase 上でEnabledしないと不具合が発生する
    /// </summary>
    void OnEnable()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        _beforeTilePos = _Tilemap.WorldToCell(new Vector3(mousePos.x, mousePos.y, 0));
    }
    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3Int currentTilePos = _Tilemap.WorldToCell(new Vector3(mousePos.x, mousePos.y, 0));
        TileBase currentTileBase = _Tilemap.GetTile(currentTilePos);
        if (EncouterTilePositions != null
            &&
            EncouterTilePositions.Any(currentTilePosition => currentTilePosition == currentTilePos)
            ||
            currentTileBase == _EncounterTileBase)
        {
            _Tilemap.SetTile(_beforeTilePos, _CanSelectedTileBase);
            _Tilemap.SetTile(currentTilePos, _EncounterTileBase); //overrideの可能性大アリ
        }
        else if (currentTileBase == _CanSelectedTileBase)
        {
            _Tilemap.SetTile(_beforeTilePos, _CanSelectedTileBase);
            _Tilemap.SetTile(currentTilePos, _SelectedTileBase);
            _beforeTilePos = currentTilePos;
        }
        else if (currentTileBase  == _SelectedTileBase
                 &&
                 Mouse.current.leftButton.wasPressedThisFrame)
        {
            _Tilemap.SetTile(currentTilePos, _SelectedTileBase);
            //キャラ下に配置されているタイルも設定する必要あり
            //キャラの移動処理
            enabled = false;
        }
        else if (_beforeTilePos != currentTilePos)
        {
            _Tilemap.SetTile(_beforeTilePos, _CanSelectedTileBase);
        }
    }
}
