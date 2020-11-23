using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
    [Multiline()]
    public string content;
    private bool needChange;

    void Start()
    {
        needChange = false;
        TooltipSystem.Hide(needChange);
    }

    //private static LTDescr delay;
    public void hide()
    {
        TooltipSystem.Hide(needChange);
    }

    public void turnOff()
    {
        needChange = !needChange;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystem.Show(content);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Hide(needChange);
        if (needChange)
        {
            needChange = false;
        }
    }
}
