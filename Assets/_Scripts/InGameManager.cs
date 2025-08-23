using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InGameManager : MonoBehaviour
{
    [SerializeField] PieceParameter[] _pieceParameters; 
    [SerializeField] Tilemap _tilemap;
    [SerializeField] TileBase _encounterTileBase;
    [SerializeField] TileBase _selectedTileBase;
    [SerializeField] TileBase _canSelectedTileBase;
    [SerializeField] TileBase _fieldTileBase;
    [SerializeField]  GameObject _boxCollider2DPrefab;
    Animator _animatorController;
    protected PieceParameter[] _PieceParameters => _pieceParameters;
    protected Tilemap _Tilemap => _tilemap;
    protected TileBase _EncounterTileBase => _encounterTileBase;
    protected TileBase _SelectedTileBase => _selectedTileBase;
    protected TileBase _CanSelectedTileBase => _canSelectedTileBase;
    protected TileBase _FieldTileBase => _fieldTileBase;
    protected GameObject _BoxCollider2DPrefab => _boxCollider2DPrefab;
    protected Animator _AnimatorController => _animatorController;

    void Awake()
    {
        _animatorController = GetComponent<Animator>();
    }
}
