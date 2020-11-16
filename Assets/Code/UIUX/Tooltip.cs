using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    private Text tooltipText;
    private RectTransform backgroudTrans;

    private void Awake()
    {
        backgroudTrans = transform.Find("ttBackground").GetComponent<RectTransform>();
        tooltipText = transform.Find("ttText").GetComponent<Text>();

        ShowTooltip("Random tt text");
        
    }
    private void ShowTooltip(string t)
    {
        gameObject.SetActive(true);
    }
    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
