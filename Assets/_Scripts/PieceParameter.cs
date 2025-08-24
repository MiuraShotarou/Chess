using System;
using UnityEngine;

/// <summary>
/// 駒ごとの移動可能領域を設定・保存するためのScriptableObject
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObject", fileName = "PieceParameter")]
public class PieceParameter : ScriptableObject
{
    [SerializeField] string _pieceName;
    [SerializeField] int _moveCount;
    [SerializeField, Tooltip("駒を中心にした時の攻撃可能範囲座標の一列目だけを入力してください")]
    Vector3Int[] _attackAreaPositions;
    [SerializeField, Tooltip("駒を中心にした時の移動可能範囲座標の一列目だけを入力してください")]
    Vector3Int[] _moveAreaPositions;
    public Func<string> PieceName;
    public Func<int> MoveCount;
    public Func<Vector3Int[]> AttackAreaPositions;
    public Func<Vector3Int[]> MoveAreaPositions;
    void OnEnable()
    {
        PieceName = () => _pieceName;
        MoveCount = () => _moveCount;
        AttackAreaPositions = () => _attackAreaPositions;
        MoveAreaPositions = () => _moveAreaPositions;
    }
}
