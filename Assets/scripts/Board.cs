using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour {
    public Deck Deck;
    public Deck Pile;
    public List<Player> Players = new List<Player>();

    private bool _initialized = false;

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
        _initDateStructure();
    }

    private void _initDateStructure() {
        Pile = GameObject.FindGameObjectWithTag("Pile").GetComponent<Deck>();
        Players = GameObject.FindGameObjectsWithTag("Player").Select(x => x.GetComponent<Player>()).ToList();
        Deck = GameObject.FindGameObjectWithTag("Deck").GetComponent<Deck>();
    }
}
