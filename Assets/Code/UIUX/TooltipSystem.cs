using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem current;
    private static bool onoff;
    public Tooltip tooltip;
    void Start()
    {
        onoff = true;
    }
    public void Awake()
    {
        current = this;
    }
    public static void Show(string content)
    {
        
        if (onoff)
        {
            current.tooltip.SetText(content);
            current.tooltip.gameObject.SetActive(true);
        }
        
    }
    public static void Hide(bool nc)
    {
        if (nc)
        {
            onoff = !onoff;
        }
        current.tooltip.gameObject.SetActive(false);
    }
}
