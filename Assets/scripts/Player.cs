using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private bool _initialized;
    private List<Card> _hand = new List<Card>();
    public int id;

    public GameObject UIHandContainer;

    public int Health;
    public List<Character> Characters;
    public int belivers;
    public List<Spell> TargetSpells = new List<Spell>();
    public Board board;

    public List<Card> Hand;
    public bool isHuman = true;

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
        board = FindObjectOfType<Board>();
        Health = 0;
        belivers = 0;
        Hand = new List<Card>();
        //id = Infra.GenerateId(); id was already defined

        UIHandContainer = GameObject.FindGameObjectWithTag("HandContainer" + id);

        // Adding this player to board
        board.Players.Add(this);
    }

    public void play(Card c, Deck pile) {
        Debug.Log("Playing");
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
                                                            { "Player", this },
                                                            { "Board", board }
        });
    }

    public override string ToString() {
        return "Player " + id;
    }
}
