using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPathLine : MonoBehaviour
{
    private LineRenderer lineRenderer;

    /// <summary>
    /// �o�H�p�̃��C������
    /// </summary>
    /// <param name="drawPaths"></param>
    public void CreatePathLine(Vector3[] drawPaths) {
        TryGetComponent(out lineRenderer);

        // ���C���̑����𒲐�
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;

        // �������郉�C���̒��_����ݒ�(����͎n�_�ƏI�_���P����)
        lineRenderer.positionCount = drawPaths.Length;

        lineRenderer.SetPositions(drawPaths);
    }
}
