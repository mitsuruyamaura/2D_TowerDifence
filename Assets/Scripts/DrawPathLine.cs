using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPathLine : MonoBehaviour
{
    /// <summary>
    /// 経路用のライン生成
    /// </summary>
    /// <param name="drawPaths"></param>
    public void CreatePathLine(Vector3[] drawPaths) {
        TryGetComponent(out LineRenderer lineRenderer);

        // ラインの太さを調整
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;

        // 生成するラインの頂点数を設定(今回は始点と終点を１つずつ)
        lineRenderer.positionCount = drawPaths.Length;

        lineRenderer.SetPositions(drawPaths);
    }
}
