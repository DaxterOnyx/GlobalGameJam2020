using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ObjectiveUI : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler
{
    public TextMeshProUGUI textPro;
    private Structure structure;
    public void Initialize(string txt,Structure str)
    {
        textPro.text = txt;
        structure = str;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        structure.ShowRepair();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        structure.HideRepair();
    }
}
