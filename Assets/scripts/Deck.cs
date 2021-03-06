﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {
    private List<Card> _deck = new List<Card>();
    private List<Card> _discardPile = new List<Card>();
    public bool IsPile = false;

    public void InitDeck() {
        _deck.Clear();
        _discardPile.Clear();
        GameObject card = Resources.Load("Card", typeof(GameObject)) as GameObject;
        for (int i = 1; i < 5; i++) {
            for (int j = 1; j < 14; j++) {
                var instance = Instantiate(card, this.transform.position, Quaternion.identity);
                Card c = instance.GetComponent<Card>();
                _deck.Add(c);
                instance.transform.parent = gameObject.transform; // e.g. this deck panel
                c.Init();
                c.Set((SuitEnum)i, j);
            }
        }
        Shuffle();
    }

    public void Clear() {
        _deck.Clear();
    }

    public int Count() {
        return _deck.Count;
    }

    public void Shuffle() {
        UnityEngine.Random rng = new UnityEngine.Random();
        int n = _deck.Count;
        while (n > 1) {
            n--;
            int k = UnityEngine.Random.Range(1, n);
            Card value = _deck[k];
            _deck[k] = _deck[n];
            _deck[n] = value;
        }
    }

    public Card DrawCard(Player p) {
        var card = RemoveCardAt(0); // 0 is the first index hense the top card
        card.MoveTo(p);
        if(p.isHuman) {
            card.SetCardFace(true);
        }
        return card;
    }

    public Card DrawCard(Deck d) {
        var card = RemoveCardAt(0); // 0 is the first index hense the top card
        card.MoveTo(d);
        if (d.IsPile) {
            card.SetCardFace(true);
        }
        return card;
    }

    internal void DrawCard(Player player, int v) {
        for (int i = 0; i < v; i++) {
            DrawCard(player);
        }
    }

    public Card RemoveCardAt(int index) {
        if (_deck.Count == 0 || _deck.Count <= index) {
            return null; // the deck is depleted
        }

        // take the first card off the deck and add it to the discard pile
        Card card = _deck[index];
        _deck.RemoveAt(0);

        // We "remember" the cards we removed from deck
        // Will come in handy in the future
        _discardPile.Add(card);

        return card;
    }

    public Card top() {
        if (_deck.Count > 0) {
            return _deck[0];
        }
        return null;
    }

    public void AddCardAt(Card c, int index) {
        _deck.Insert(index, c);
    }

    public void Remove(Card c) {
        _deck.Remove(c);
    }
}
