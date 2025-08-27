using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
public class ShiftPieces : InGameManager
{
    [SerializeField] private TileBase _001TileBase;
    //マウスがクリックされたところのワールド座標を取得する
    //そのワールド座標をタイル座標に変換する
    //タイル座標に変換してからもう一度ワールド座標に戻す
    //変換したワールド座標に指定のオブジェクトを移動させる
    Vector3Int _beforeTilePos = default;
    Vector3Int _selectedPiecePos = default;
    List<Vector3Int> _encouterTilePositions = new List<Vector3Int>();
    
    [SerializeField] GameObject _shiftTargegtObj;
    void Update()
    {
        //マウスカーソルポジションを取得
        //そこからTilemap のタイルを取得する
    }
}
