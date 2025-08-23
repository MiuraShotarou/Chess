using UnityEngine;

public class CollisionEventSystem : OpenSelectableArea
{
    void OnCollisionEnter2D(Collision2D collision2D)
    {
        JudgmentPieceGroup(collision2D.gameObject, transform);
    }
}
