using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
/// <summary>
/// 移動可能領域の選択範囲が確定し、移動可能領域の描画がされている間にしかActiveにならないスクリプト
/// </summary>
public class SelectTileController : InGameManager
{
    Vector3Int _beforeTilePos = default;
    Vector3Int _selectedPiecePos = default;
    List<Vector3Int> _encouterTilePositions = new List<Vector3Int>();
    public void SetPiecePositions(Vector3Int selectedPiecePos, List<Vector3Int> encouterTilePos)
    {
        Debug.Log("");
        _selectedPiecePos = selectedPiecePos;
        _encouterTilePositions = encouterTilePos;
    }
    /// <summary>
    /// _canSelectedTileBaseの上にカーソルがあるときのみ_selectedTileBaseが描画される
    /// </summary>
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3Int currentTilePos = _Tilemap.WorldToCell(new Vector3(mousePos.x, mousePos.y, 0));
        TileBase currentTileBase = _Tilemap.GetTile(currentTilePos);

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Debug.Log($"currentTilePosInt {currentTilePos}, currentTilePos3 {_Tilemap.CellToWorld(currentTilePos)}"); //x -0.5 y - 0.05
        }
        //EncounterTileBaseの描画
        if (_encouterTilePositions != null
            &&
            _encouterTilePositions.Any(currentTilePosition => currentTilePosition == currentTilePos)
            ||
            currentTileBase == _EncounterTileBase)
        {
            if (_beforeTilePos == default) InitializePositions(currentTilePos);
            _Tilemap.SetTile(_beforeTilePos, _CanSelectedTileBase);
            _Tilemap.SetTile(currentTilePos, _EncounterTileBase); //overrideの可能性大アリ
        }
        else if (currentTileBase == _CanSelectedTileBase)
        {
            if (_beforeTilePos == default) InitializePositions(currentTilePos);
            _Tilemap.SetTile(_beforeTilePos, _CanSelectedTileBase);
            _Tilemap.SetTile(currentTilePos, _SelectedTileBase);
            _beforeTilePos = currentTilePos;
        }
        else if ((currentTileBase == _SelectedTileBase && Mouse.current.leftButton.wasPressedThisFrame)
                 ||
                 (currentTileBase == _EncounterTileBase && Mouse.current.leftButton.wasPressedThisFrame))
        {
            if (_selectedPiecePos == currentTilePos) return;
            _Tilemap.SetTile(currentTilePos, _SelectedTileBase);
            GetComponent<OpenSelectableArea>().TurnDesideRelay();
            enabled = false;
        }
        else if (_beforeTilePos != currentTilePos)
        {
            if (_beforeTilePos == default) return;
            _Tilemap.SetTile(_beforeTilePos, _CanSelectedTileBase);
        }
        Debug.Log(_beforeTilePos);
    }
    void InitializePositions(Vector3Int currentTilePos)
    {
        _beforeTilePos = currentTilePos;
    }
    void OnDisable()
    {
        _beforeTilePos = default;
    }
}
