using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    [SerializeField]
    private Text txtTotalClearPoint;

    // Start is called before the first frame update
    void Start()
    {
        UpdateDisplayTotalClearPoint();
    }

    private void UpdateDisplayTotalClearPoint() {
        txtTotalClearPoint.text = GameData.instance.totalClearPoint.ToString();
    }
}
