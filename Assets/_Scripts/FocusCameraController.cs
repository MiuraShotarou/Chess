using UnityEngine;

public class FocusCameraController : MonoBehaviour
{
    /// <summary>
    /// 各駒のポジションから適切なFocusCameraポジションまでの差
    /// </summary>
    void AdjustmentPiecesPosition()
    {
        //K (0, 0.7, -10)
        //Q (0, 0.5, -10)
        //B (0, 0.5, -10)
        //N (0, 0.35, -10)
        //R (0, 0.2, -10)
        //P (0, 0.4, -10)
        // 非アクティブ時のAlpha値は150
    }
}
