using System;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TilemapController : MonoBehaviour
{
    [SerializeField] Tilemap _tilemap;
    [SerializeField] TileBase _movedTileBase;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // InputActionReference
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector3Int tilePos = _tilemap.WorldToCell(new  Vector3(mousePos.x, mousePos.y, 0));
            _tilemap.SetTile(tilePos, _movedTileBase);
        }
    }
}
