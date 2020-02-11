using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CamMover : MonoBehaviour ,IPointerEnterHandler, IPointerExitHandler
{
    public bool touched { get; private set; }

    public void OnPointerEnter(PointerEventData eventData)
    {
        touched = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        touched = false;
    }

}
