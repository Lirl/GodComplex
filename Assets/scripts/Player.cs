﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private bool _initialized;
    private List<Card> _hand = new List<Card>();
    public int id;
    public int Health;
    public List<Character> Characters;
    public int belivers;
    public List<Spell> OnMouseRelease = new List<Spell>();

    public List<Card> Hand;

    // Use this for initialization
    void Start() {  
        init();
    }

    void Awake() {
        init();
    }

    public void init() {
        if (_initialized) {
            return;
        }

        _initialized = true;

        Health = 0;
        belivers = 0;
        Hand = new List<Card>();
        id = Infra.GenerateId();
    }

    public void play(Card c, Deck pile) {
        if(pile == null) {
            pile = GameObject.FindGameObjectWithTag("Pile").GetComponent<Deck>();
        }

        c.FaceUp = true;

        // Remove played card from hand
        // Card has no container at this point, and will return to player hand or stay at the top of pile
        // if no important rule has failed
        Hand.Remove(c);

        EventManager.TriggerEvent("Play", new Hashtable() { { "Card", c },
                                                            { "Pile", pile },
                                                            { "Player", this }
        });
    }

}
