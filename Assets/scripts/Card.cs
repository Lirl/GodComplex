using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {
    private SuitEnum _suit;
    private int _rank;
    private int owner = 0; // Id of player owning the card, where 0 is no player

    public SuitEnum Suit { get { return _suit; } }
    public int Rank { get { return _rank; } }

    public bool FaceUp = true;
}