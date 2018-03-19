using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {
    public int Owner = 0; // Id of player owning the card, where 0 is no player

    public SuitEnum Suit;
    public int Rank;
    public GameObject card;
    

    public bool FaceUp = true;

    private void Awake() {
        var r = name.Split(new char[] { '_' });
        Rank = int.Parse(r[0]);
        Suit = (SuitEnum) int.Parse(r[1]);
    }
}