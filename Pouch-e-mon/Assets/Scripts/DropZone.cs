using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    DropZone positionRef = null;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        Card c = eventData.pointerDrag.GetComponent<Card>();
        if (c != null)
        {
            c.placeholderParent = this.transform;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Card c = eventData.pointerDrag.GetComponent<Card>();
        if (c != null)
        {
            c.parentToReturn = this.transform;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        Card c = eventData.pointerDrag.GetComponent<Card>();
        if (c != null && c.placeholderParent == this.transform)
        {
            c.placeholderParent = c.parentToReturn;
        }
    }
}
