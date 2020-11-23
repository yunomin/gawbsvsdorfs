using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
    [Multiline()]
    public string content;
    private bool enable;

    void Start()
    {
        TooltipSystem.Hide();
        enable = true;
    }

    //private static LTDescr delay;
    public void hide()
    {
        TooltipSystem.Hide();
    }
    public void turnOff()
    {
        enable = !enable;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //delay = LeanTween.delayedCall(0.5f, () =>
        //{
        //    TooltipSystem.Show(content);
        //});
        if (enable)
        {
            TooltipSystem.Show(content);
        }
        
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //LeanTween.cancel(delay.uniqueId);
        TooltipSystem.Hide();
    }
}
