using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    //TODO
    // 1 - Implement rules for droping cards on illeagal decks
    // 2 - gives different card types different drop zones
    // 3 - Find out how to create effects on the field and vanish cards (Discard pile?)

    //Use to get event calls about going in canvas zones
    public void OnPointerEnter(PointerEventData data) {

    }

    //Use to get event calls about going out of canvas zones
    public void OnPointerExit(PointerEventData data) {
    }

    //Use to alert card being realesed.
    public void OnDrop(PointerEventData data) {
        Draggable d = data.pointerDrag.GetComponent<Draggable>();

        if (d == null) {
            return;
        }

        d.parentReturn = this.transform;

        if(gameObject.tag == "Pile") {
            Card c = data.pointerDrag.GetComponent<Card>();
            Deck pile = gameObject.GetComponent<Deck>();
            Board board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>();
            Player p = board.GetPlayerById(c.Alliance);

            p.play(c, pile);
        }

    }

}
