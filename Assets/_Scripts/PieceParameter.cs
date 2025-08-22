using UnityEngine;
/// <summary>
/// 駒ごとの移動可能領域を設定・保存するためのScriptableObject
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObject", fileName = "PieceParameter")]
public class PieceParameter : ScriptableObject
{
    //絶対に必要なのは駒ごとの移動可能座標と、足し算を行う処理、そして方角の判断、Animationを繰り返し再生するint型、移動した先で敵のマスを検知したらそこから先のマスは表示しないようにする処理、そしてそのマスにいる駒はハイライトさせる処理・カーソルを合わせると赤色のタイルが表示される処理、移動した後に深緑のタイルが設定されるようにする
    [SerializeField] string _pieceName;
    [SerializeField] bool _isMoveLimit;
    [SerializeField ,Tooltip("駒を中心にした時の攻撃可能範囲座標の一列目だけを入力してください")]
    Vector3Int[] _atackAreaPositions;
    [SerializeField, Tooltip("駒を中心にした時の移動可能範囲座標の一列目だけを入力してください")] 
    Vector3Int[] _moveAreaPositions;
    public string _PieceName => _pieceName;
    public bool _IsMoveLimit => _isMoveLimit;
    public Vector3Int[] _AtackAreaPositions => _atackAreaPositions;
    public Vector3Int[] _MoveAreaPositions => _moveAreaPositions;
}
