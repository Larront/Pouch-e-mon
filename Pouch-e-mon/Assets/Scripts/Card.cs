using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public Transform parentToReturn = null;
    public Transform placeholderParent = null;

    Vector3 targetScale = Vector3.one;
    Vector3 velocity = Vector3.zero;
    float smoothTime = 0.3f;

    GameObject placeholder = null;

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Enlarge on Pickup
        this.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);

        // Create Placeholder object to indicate spacing
        placeholder = new GameObject();
        LayoutElement le = placeholder.AddComponent<LayoutElement>();

        // Adjust placeholder size
        le.preferredWidth = 0;
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        le.flexibleHeight = 0;
        le.flexibleWidth = 0;
        var rectTransform = placeholder.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.sizeDelta = new Vector2(175, 250);
        }
        placeholder.transform.localScale = Vector3.zero;

        placeholder.transform.SetParent(this.transform.parent);
        placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        // Set old parent
        this.parentToReturn = this.transform.parent;

        placeholderParent = parentToReturn;

        // Remove from current parent
        this.transform.SetParent(this.transform.root);

        // Remove Raycast Blocking on pickup
        GetComponent<CanvasGroup>().blocksRaycasts = false;

    }

    public void OnDrag(PointerEventData eventData)
    {
        // Follow mouse on drag
        this.transform.position = eventData.position;

        // Make sure placeholder Parent is set to the corrent place
        if (placeholder.transform.parent != placeholderParent)
            placeholder.transform.SetParent(placeholderParent);

        int newSiblingIndex = placeholderParent.childCount;

        for (int i = 0; i < placeholderParent.childCount; i++)
        {
            if (this.transform.position.x < placeholderParent.GetChild(i).position.x && placeholder.transform.GetSiblingIndex() != i)
            {
                newSiblingIndex = i;

                if (placeholder.transform.GetSiblingIndex() < newSiblingIndex)
                    newSiblingIndex--;
                break;
            }

        }
        placeholder.transform.SetSiblingIndex(newSiblingIndex);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Shrink on Drop
        this.transform.localScale = new Vector3(1, 1, 1);

        // Go to designated parent
        this.transform.SetParent(this.parentToReturn);
        this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());

        // Add back Raycast blocking on drop
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        // Destroy Placeholder
        Destroy(placeholder);

    }

    void Update()
    {
        if (placeholder != null)
        {
            if (placeholder.transform.localScale != targetScale)
            {
                placeholder.transform.localScale = Vector3.SmoothDamp(placeholder.transform.localScale, targetScale, ref velocity, smoothTime);
            }
        }

    }

    void SetNewTargetPosition(Vector3 pos)
    {
        targetScale = pos;
        velocity = Vector3.zero;
    }
}
