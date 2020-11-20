using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class Tooltip : MonoBehaviour
{
    [SerializeField]
    private Camera uiCamera;
    public Text tooltipText;
    public RectTransform backgroundRectTransform;
    public RectTransform tooltipTransform;

    private void Awake()
    {
        
        ShowTooltip("Random  kjhgfd\nt text kjhgfdsdfgh");
        
    }
    private void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(tooltipTransform, Input.mousePosition, uiCamera, out localPoint);
        transform.localPosition = localPoint;
    }
    private void ShowTooltip(string t)
    {
        gameObject.SetActive(true);
        tooltipText.text = t;
        float textPaddingSize = 4f;

        Vector2 backgroundSize = new Vector2(tooltipText.preferredWidth + textPaddingSize * 2f, tooltipText.preferredHeight + textPaddingSize * 2f);
        backgroundRectTransform.sizeDelta = backgroundSize;
    }
    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
