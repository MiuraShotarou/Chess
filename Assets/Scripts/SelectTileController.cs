using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class SelectTileController : MonoBehaviour
{
    [SerializeField] Tilemap _tilemap;
    [SerializeField] TileBase _selectedTileBase;
    [SerializeField]  TileBase _canSelectedTileBase;
    Vector3Int _beforeTilePos;

    /// <summary>
    /// 現状、_canSelectedTileBase 上でEnabledしないと不具合が発生する
    /// </summary>
    void OnEnable()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        _beforeTilePos = _tilemap.WorldToCell(new Vector3(mousePos.x, mousePos.y, 0));
    }
    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3Int currentTilePos = _tilemap.WorldToCell(new Vector3(mousePos.x, mousePos.y, 0));
        TileBase currentTileBase = _tilemap.GetTile(currentTilePos);
        if (currentTileBase == _canSelectedTileBase)
        {
            _tilemap.SetTile(_beforeTilePos, _canSelectedTileBase);
            _tilemap.SetTile(currentTilePos, _selectedTileBase);
            _beforeTilePos = currentTilePos;
        }
        else if (currentTileBase  == _selectedTileBase
                 &&
                 Mouse.current.leftButton.wasPressedThisFrame)
        {
            _tilemap.SetTile(currentTilePos, _selectedTileBase);
            //キャラ下に配置されているタイルも設定する必要あり
            //キャラの移動処理
            enabled = false;
        }
        else if (_beforeTilePos != currentTilePos)
        {
            _tilemap.SetTile(_beforeTilePos, _canSelectedTileBase);
        }
    }
}
