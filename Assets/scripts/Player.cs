using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private bool _initialized;
    private List<Card> _hand = new List<Card>();
    public int id;
    public int Health;

    public List<Card> Hand {
        get {
            return _hand;
        }

        set {
            _hand = value;
        }
    }

    public object EventManager { get; private set; }

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
        Hand = new List<Card>();
        id = Infra.GenerateId();
    }

    public void play(Card c, Deck pile) {
        if(pile == null) {
            pile = GameObject.FindGameObjectWithTag("Pile").GetComponent<Deck>();
        }

        c.FaceUp = true;
    }
}
