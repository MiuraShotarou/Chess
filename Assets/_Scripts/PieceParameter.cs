using System;
using UnityEngine;

/// <summary>
/// 駒ごとの移動可能領域を設定・保存するためのScriptableObject
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObject", fileName = "PieceParameter")]
public class PieceParameter : ScriptableObject
{
    //絶対に必要なのは駒ごとの移動可能座標と、足し算を行う処理、そして方角の判断、Animationを繰り返し再生するint型、移動した先で敵のマスを検知したらそこから先のマスは表示しないようにする処理、そしてそのマスにいる駒はハイライトさせる処理・カーソルを合わせると赤色のタイルが表示される処理、移動した後に深緑のタイルが設定されるようにする
    //Pone の動き（斜め前攻撃、　※最初のみ二回行動、アンパッサン（2歩進んだ直後のポーンの後ろのマスに自身のポーンの攻撃範囲が充てられている場合、そこに移動し相手のポーンを取ってしまうことができる））→ Colliderを残す形でアンパッサンを実装する → 他の特殊ルール的にも「初回の動きであるか否か」のbool型は必要かも
    //キャスリング（条件：キングが一度も動いていない、ルークが一度も動いていない、キングとルークの間に駒がない、チェックされていない、キングの両隣が攻撃範囲に入っていない　効果：キングはルークがいる方向に２マス進み、ルークはキングを飛び越えてひとつ目のマスに移動する）
    //右側にキャスリングすることを「ショートキャスリング」、左側にキャスリングすることを「ロングキャスリング」と呼ぶ →　この２つの動きは固定のようだ
    //プロモーション（条件：ポーンが突き当りのマスに到着した時、キング・ポーン以外の好きな駒に変化することができる）
    //ステイルメイト（効果；チェックがかかっていない状態で何かの駒を動かすと敗北が決まってしまうという時、試合結果はドローになる）
    //ドロー（条件：キング対キングになること、ステイルメイト、3回同一局面、50回ポーンが動かずどの駒も取られていない、合意によるドロー）

    //これら特殊ルール（スキル）をデリゲートで持たせるのはありかも
    [SerializeField] string _pieceName;
    [SerializeField] int _moveCount;

    [SerializeField, Tooltip("駒を中心にした時の攻撃可能範囲座標の一列目だけを入力してください")]
    Vector3Int[] _attackAreaPositions;

    [SerializeField, Tooltip("駒を中心にした時の移動可能範囲座標の一列目だけを入力してください")]
    Vector3Int[] _moveAreaPositions;

    bool _isNotMove = true;
    Func<PieceParameter> _activeSkill;
    public string _PieceName => _pieceName;
    public int _MoveCount => _moveCount;
    public Vector3Int[] _AttackAreaPositions => _attackAreaPositions;
    public Vector3Int[] _MoveAreaPositions => _moveAreaPositions;
    public bool _IsNotMove => _isNotMove;
    public Func<PieceParameter> _ActiveSkill => _activeSkill; //Func関数内で_isNotMoveなどの書き換えを行うべき
}
