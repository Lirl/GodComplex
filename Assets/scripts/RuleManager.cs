
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RuleManager {
    public List<Rule> Rules = new List<Rule>();

    public List<Rule> ExtraRules = new List<Rule>();

    public List<Rule> CopyExtraRules;

    public RuleManager() {
        _setBasicRules();
        _setExtraRules();
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


        //Converter reciever, on Heart 3, 6, 9 or queen
        Rules.Add(new Rule(delegate (Hashtable hash) {

            var card = (Card)hash["Card"];
            return (card.Rank % 3 == 0);

        }, delegate (Hashtable arg) {
            var player = (Player)arg["Player"];
            var spell = new Spell();

            spell.onCardPlay = delegate (Vector3 pos) {
                Debug.LogError("onCardPlay!");

                // pos here is null (vector(0,0,0))
                // things that should happen on card placement in pile

                // Create Converter object
                var sp = Resources.Load("RadiusCircle", typeof(GameObject)) as GameObject;

                var board = (Board)arg["Board"];

                var insta = board.CreateGameObject(sp, Input.mousePosition);
                Debug.LogError("RadiusCircle created");
                sp.GetComponent<RadiousTarget>().Player = player; // Set player to follow (TODO: check if human)
                arg["prefab"] = insta;

                insta.transform.parent = GameObject.FindGameObjectWithTag("Canvas").transform;
                //EventManager.TriggerEvent("ShowText", new Hashtable() { { "Text", "Convert Peasants To Belivers!" }, { "Duration", 1f } });
            };

            spell.onClick = delegate (Vector3 pos) {

                Debug.LogError("onClick : " + pos);
                // pos is mouse position on click
                // things that should happen on card placement in pile

                var board = (Board)arg["Board"];

                var location = new Vector3(Camera.main.ScreenToWorldPoint(pos).x,
                       Camera.main.ScreenToWorldPoint(pos).y, 0);

                var toConvert = Infra.GetPeasantsInRange(location, (float)1);
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

    private void _setExtraRules() {

        // Any Jack Queen or King
        // ==> summon priests
        ExtraRules.Add(new Rule(delegate (Hashtable hash) {

            var playedCard = (Card)hash["Card"];
            return (playedCard.Rank > 10);

        }, delegate (Hashtable hash) {
            var playedCard = (Card)hash["Card"];
            var board = (Board)hash["Board"];
            var player = (Player)hash["Player"];
            var priest = Resources.Load("Priest", typeof(GameObject)) as GameObject;

            for (int i = 0; i < playedCard.Rank - 10; i++) {

                var instance = board.SpawnRandom(priest);
                var character = instance.GetComponent<Character>();
                character.init();
                character.SetAlliance(player.id);
            }

        }, delegate (Hashtable hash) {

        }, false, "I am here to Serve", "Any Jack, Queen or King : Summon a priest that converts peasants near him to be under your domain", "rul2"));

        ExtraRules.Add(new Rule(delegate (Hashtable hash) {

            var playedCard = (Card)hash["Card"];
            return (playedCard.Rank < 5 && playedCard.Suit == SuitEnum.Spades);

        }, delegate (Hashtable arg) {

            var player = (Player)arg["Player"];
            var spell = new Spell();

            spell.onCardPlay = delegate (Vector3 pos) {
                // Create Converter object
                var board = (Board)arg["Board"];

                Debug.LogError("Attempting Skill circle created");
                var sp = Resources.Load("SkullTarget", typeof(GameObject)) as GameObject;
                
                var insta = board.CreateGameObject(sp, Input.mousePosition);
                Debug.LogError("Skill circle  created");

                sp.GetComponent<RadiousTarget>().Player = player; // Set player to follow (TODO: check if human)
                arg["prefab"] = insta;

                insta.transform.parent = GameObject.FindGameObjectWithTag("Canvas").transform;
                //EventManager.TriggerEvent("ShowText", new Hashtable() { { "Text", "Convert Peasants To Belivers!" }, { "Duration", 1f } });
            };
            spell.onClick = delegate (Vector3 pos) {

                var board = (Board)arg["Board"];

                var location = new Vector3(Camera.main.ScreenToWorldPoint(pos).x,
                       Camera.main.ScreenToWorldPoint(pos).y, 0);

                var toKill = Infra.GetPeasantsInRange(location, (float)1);
                foreach (Character c in toKill) {
                    board.DestroyInstance(c.gameObject);
                }

                // Destroying skull cursor
                board.DestroyInstance((GameObject)arg["prefab"]);
            };

            spell.hasMouseTarget = true;

        }, delegate (Hashtable hash) {

        }, false, "Oppression", "Spades 1-4 : Kills peasants in chosen radius", "rul3"));

        ExtraRules.Add(new Rule(delegate (Hashtable hash) {

            var playedCard = (Card)hash["Card"];
            return (playedCard.Rank == 13 && playedCard.Suit == SuitEnum.Diamonds);

        }, delegate (Hashtable arg) {

            var player = (Player)arg["Player"];
            var spell = new Spell();

            spell.onCardPlay = delegate (Vector3 pos) {
                // Create Converter object
                var sp = Resources.Load("SkullTarget", typeof(GameObject)) as GameObject;

                var board = (Board)arg["Board"];

                var insta = board.CreateGameObject(sp, Input.mousePosition);
                sp.GetComponent<RadiousTarget>().Player = player; // Set player to follow (TODO: check if human)
                arg["prefab"] = insta;

                insta.transform.parent = GameObject.FindGameObjectWithTag("Canvas").transform;
                //EventManager.TriggerEvent("ShowText", new Hashtable() { { "Text", "Convert Peasants To Belivers!" }, { "Duration", 1f } });
            };
            spell.onClick = delegate (Vector3 pos) {

                var board = (Board)arg["Board"];

                var location = new Vector3(Camera.main.ScreenToWorldPoint(pos).x,
                       Camera.main.ScreenToWorldPoint(pos).y, 0);

                var toKill = Infra.GetPeasantsInRange(location, (float)1);
                foreach (Character c in toKill) {
                    board.DestroyInstance(c.gameObject);
                }

                // Destroying skull cursor
                board.DestroyInstance((GameObject)arg["prefab"]);
            };

            spell.hasMouseTarget = true;
            player.TargetSpells.Add(spell);

        }, delegate (Hashtable hash) {

        }, false, "Armagedon", "King of Diamonds : Meteor shower", "rul1"));

        CopyExtraRules = new List<Rule>(ExtraRules);
    }

    public void RevertExtraRules() {
        ExtraRules = CopyExtraRules;
    }

    public Rule ChooseRandomExtraRule() {
        var rule = ExtraRules.Shuffle().PickRandom(1).First();
        ExtraRules.Remove(rule);
        return rule;
    }
}
