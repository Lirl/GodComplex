using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    //Remember from which deck you came from so if you are 
    //not in a DropZone the card returns to the same deck
    public Transform parentReturn = null;

    public void OnBeginDrag(PointerEventData data) {
        //Raycasts are going out from your mouse, we dont want to block them 
        //while we drag...
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        parentReturn = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent);
    }

    public void OnDrag(PointerEventData data) {
        this.transform.position = data.position;
    }

    public void OnEndDrag(PointerEventData data) {
        //in case we didnt end in a DropZone
        this.transform.SetParent(parentReturn);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}