
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

            if (!rule.Validate(arg)) {
                Debug.Log("Rule " + i + " Falied");
                res.Clear();
                res.Add(rule.OnFailure);
                completeSuccess = false;
                break;
            } else {
                Debug.Log("Rule " + i + " Succeeded");
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
            var playedCard = (Card) hash["Card"];
            var top = pile.top(); // get pile toppest card (e.g. index 0) without removing it

            Debug.Log("top : " + top + " card: " + playedCard);
            
            return (top.Suit == playedCard.Suit || top.Rank == playedCard.Rank);
            
        }, delegate (Hashtable hash) {
            // Do nothing
        }, delegate (Hashtable hash) {

            // If no other important rule did it already, then return the card to its owner hands
            // Card was never added to pile (that happens in ruleManager if all important rules was successful)

            // Not necessary all failed rules will return the card back to your hand
            // Maybe some good cards played incorrectly would be BURNED in place rather than return to hand
            Debug.Log("RUNNING ON FAILURE PLAYING AGAINST THE RULES");

            var player = (Player) hash["Player"];
            var card = (Card)hash["Card"]; // Card chosen to be played

            card.MoveBack();

        }, true));  // This little true here sets the rule as important
                    // which means that this rule is going to be evaluated before any rule which is not important.
                    // in addition, if an important rule fails, you receive a panelty, your card back and end your turn
                    // and no other rule takes effect (like 7 card wont take effect cause card wasnt played successfuly)

        // Playing not in your turn
        Rules.Add(new Rule(delegate (Hashtable hash) {

            var card = (Card)hash["Card"];
            var board = (Board)hash["Board"];

            return board.CurrentPlayer().id == card.Alliance;

        }, delegate (Hashtable hash) {
            // Do nothing
        }, delegate (Hashtable hash) {

            // If no other important rule did it already, then return the card to its owner hands
            // Card was never added to pile (that happens in ruleManager if all important rules was successful)

            // Not necessary all failed rules will return the card back to your hand
            // Maybe some good cards played incorrectly would be BURNED in place rather than return to hand
            var player = (Player)hash["Player"];
            var card = (Card)hash["Card"]; // Card chosen to be played

            Debug.Log("RUNNING ON FAILURE NOT YOUR TURN " + card + " p: " + player);

            // Move card back to player's hand
            card.MoveBack();

        }, true));  // This little true here sets the rule as important
                    // which means that this rule is going to be evaluated before any rule which is not important.
                    // in addition, if an important rule fails, you receive a panelty, your card back and end your turn
                    // and no other rule takes effect (like 7 card wont take effect cause card wasnt played successfuly)


        // PLAYED 1: gain another turn
        /*Rules.Add(new Rule(delegate (Hashtable hash) {
            var card = (Card)hash["Card"];
            return card.Rank == 1;
        }, delegate (Hashtable hash) {
            EventManager.TriggerEvent("SetNextTurn", new Hashtable() { { "Player", hash["Player"] } });
        }, delegate (Hashtable hash) {
            // Do nothing
        }));*/


        //Converter reciever, on Diamond 3, 6, 9 or queen
        Rules.Add(new Rule(delegate (Hashtable hash) {

            var card = (Card)hash["Card"];
            return true || (card.Rank % 3 == 0);

        }, delegate (Hashtable arg) {
            var player = (Player)arg["Player"];
            var spell = new Spell();
            
            spell.onCardPlay = delegate (Vector3 pos) {
                Debug.LogError("onCardPlay!");

                // pos here is null (vector(0,0,0))
                // things that should happen on card placement in pile

                // Create Converter object
                var sp = Resources.Load("RadiousTarget", typeof(GameObject)) as GameObject;
                var board = (Board)arg["Board"];

                var insta = board.CreateGameObject(sp, Input.mousePosition);
                insta.GetComponent<RadiousTarget>().Player = player; // Set player to follow (TODO: check if human)
                arg["prefab"] = insta;

                insta.transform.parent = Camera.main.transform;
            };

            spell.onClick = delegate (Vector3 pos) {

                Debug.LogError("onClick : " + pos);
                // pos is mouse position on click
                // things that should happen on card placement in pile

                var board = (Board)arg["Board"];

                var location = new Vector3(Camera.main.ScreenToWorldPoint(pos).x,
                       Camera.main.ScreenToWorldPoint(pos).y, 0);

                var toConvert = Infra.GetPeasantsInRange(location, (float) 0.5);
                foreach (Character c in toConvert) {
                    c.SetAlliance(player.id);
                }

                board.DestroyInstance((GameObject)arg["prefab"]);
            };

            spell.hasMouseTarget = true;

            Debug.LogError("Adding a new spell successfuly ==> " + player.id);
            player.TargetSpells.Add(spell);

        }, delegate (Hashtable hash) {
            // Do nothing
        }));

    }




}
