
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class Rule {
    public Predicate<Hashtable> Predicate;
    public RuleHandler OnSuccess;
    public RuleHandler OnFailure;

    public bool important = false;

    public Rule(Predicate<Hashtable> pre, RuleHandler success, RuleHandler failure) {
        Predicate = pre;
        OnSuccess = success;
        OnFailure = failure;
    }
    public Rule(Predicate<Hashtable> pre, RuleHandler success, RuleHandler failure, bool important) {
        Predicate = pre;
        OnSuccess = success;
        OnFailure = failure;
        this.important = important;
    }

    public bool Validate(Hashtable arg) {
        return Predicate(arg);
    }
}

public class RuleManager {
    public List<Rule> Rules = new List<Rule>();


    public RuleManager() {
        _setBasicRules();
    }

    public List<RuleHandler> ValidateImportant(Hashtable arg, out bool completeSuccess) {
        var importants = Rules.Where(c => c.important).ToList();
        List<RuleHandler> res = new List<RuleHandler>();

        completeSuccess = true;

        for (int i = 0; i < importants.Count; i++) {
            var rule = importants[i];
            // if an important rule failed,
            // we should 
            if (!rule.Validate(arg)) {
                res.Clear();
                res.Add(rule.OnFailure);
                completeSuccess = false;
                break;
            } else {
                res.Add(rule.OnSuccess);
            }
        }

        return res;
    }

    public List<RuleHandler> ValidateUnimportant(Hashtable arg) {
        var importants = Rules.Where(c => !c.important).ToList();
        List<RuleHandler> res = new List<RuleHandler>();

        for (int i = 0; i < importants.Count; i++) {
            var rule = importants[i];

            if (!rule.Validate(arg)) {
                res.Add(rule.OnFailure);
            } else {
                res.Add(rule.OnSuccess);
            }
        }

        return res;
    }

    private void _setBasicRules() {

        // Playing against rules of GC
        Rules.Add(new Rule(delegate (Hashtable hash) {

            var pile = (Deck) hash["Pile"];
            var playedCard = (Card) hash["playedCard"];
            var top = pile.top(); // get pile toppest card (e.g. index 0) without removing it

            return (top.Suit == playedCard.Suit || top.Rank == playedCard.Rank);
            
        }, delegate (Hashtable hash) {
            // Do nothing
        }, delegate (Hashtable hash) {

            // If no other important rule did it already, then return the card to its owner hands
            // Card was never added to pile (that happens in ruleManager if all important rules was successful)

            // Not necessary all failed rules will return the card back to your hand
            // Maybe some good cards played incorrectly would be BURNED in place rather than return to hand
            
            var player = (Player) hash["Player"];
            var card = (Card)hash["Card"]; // Card chosen to be played
            if(!player.Hand.Contains(card)) {
                player.Hand.Add(card);
            }

        }, true));  // This little true here sets the rule as important
                    // which means that this rule is going to be evaluated before any rule which is not important.
                    // in addition, if an important rule fails, you receive a panelty, your card back and end your turn
                    // and no other rule takes effect (like 7 card wont take effect cause card wasnt played successfuly)

        // Playing not in your turn
        Rules.Add(new Rule(delegate (Hashtable hash) {

            var player = (Player)hash["Player"];
            var board = (Board)hash["Board"];

            return board.CurrentPlayer() == player.id;

        }, delegate (Hashtable hash) {
            // Do nothing
        }, delegate (Hashtable hash) {

            // If no other important rule did it already, then return the card to its owner hands
            // Card was never added to pile (that happens in ruleManager if all important rules was successful)

            // Not necessary all failed rules will return the card back to your hand
            // Maybe some good cards played incorrectly would be BURNED in place rather than return to hand

            var player = (Player)hash["Player"];
            var card = (Card)hash["Card"]; // Card chosen to be played
            if (!player.Hand.Contains(card)) {
                player.Hand.Add(card);
            }

        }, true));  // This little true here sets the rule as important
                    // which means that this rule is going to be evaluated before any rule which is not important.
                    // in addition, if an important rule fails, you receive a panelty, your card back and end your turn
                    // and no other rule takes effect (like 7 card wont take effect cause card wasnt played successfuly)


        // PLAYED 1: gain another turn
        Rules.Add(new Rule(delegate (Hashtable hash) {

            var card = (Card)hash["Card"];
            return card.Rank == 1;

        }, delegate (Hashtable hash) {


            EventManager.TriggerEvent("SetNextTurn", new Hashtable() { { "Player", hash["Player"] } });

        }, delegate (Hashtable hash) {
            // Do nothing
        }));
    }

}
