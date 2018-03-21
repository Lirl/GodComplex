using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour {
    public Deck Deck;
    public Deck Pile;
    public List<Player> Players = new List<Player>();
    public List<GameObject> characters;

    public int TurnCount { get; private set; }
    public int PlayerCount { get; private set; }
    public RuleManager RuleManager { get; private set; }

    private int nextPlayer = 0;
    private int turnDirection = 1;
    private int currentPlayerIndex = 0;
    private Player currentPlayer;

    private bool _initialized = false;
    private double _winPersantage = 0.85; // Win condition

    void Start() {
        init();
    }

    void Awake() {
        init();
        CreatePeasants();
        Invoke("CreatePriest", 3f);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void init() {
        if (_initialized) {
            return;
        }

        _initialized = true;
        _initDateStructure();
        _subscribeEvents();
    }

    private void _initDateStructure() {

        TurnCount = 0;
        PlayerCount = Players.Count;

        RuleManager = new RuleManager();

        Pile = GameObject.FindGameObjectWithTag("Pile").GetComponent<Deck>();
        Players = GameObject.FindGameObjectsWithTag("Player").Select(x => x.GetComponent<Player>()).ToList();
        Deck = GameObject.FindGameObjectWithTag("Deck").GetComponent<Deck>();

        GameObject peasant = Resources.Load("Peasant", typeof(GameObject)) as GameObject;
        characters.Add(peasant);
        GameObject priest = Resources.Load("Priest", typeof(GameObject)) as GameObject;
        characters.Add(priest);
    }

    private void _subscribeEvents() {

        EventManager.StartListening("Play", PlayHandler);
        EventManager.StartListening("EndTurn", _endTurnHandler);
        EventManager.StartListening("StartTurn", _startTurnHandler);
        EventManager.StartListening("RoundOver", _roundOverHandler);
        EventManager.StartListening("GameOver", _gameOverHandler);

        // Enable changing which player would be next
        EventManager.StartListening("SetNextTurn", SetNextTurn);
    }


    private void _gameOverHandler(Hashtable arg) {
        var player = arg["winner"];
    }

    public int CurrentPlayer() {
        return currentPlayerIndex;
    }

    // A player played a card
    private void PlayHandler(Hashtable arg) {
        Card card = (Card)arg["Card"];// <== Card that has been played
        Deck pile = (Deck)arg["Pile"];// <== Pile it was placed on
        Player player = (Player)arg["Player"];

        bool completeSuccess;
        var handlers = RuleManager.ValidateImportant(arg, out completeSuccess);

        // Execute all handlers for imprtant rules
        for (int i = 0; i < handlers.Count; i++) {
            handlers[i](arg);
        }

        if (!completeSuccess) {
            // If an important rule has failed, we should stop here
            EventManager.TriggerEvent("EndTurn", arg);
            return;
        }

        handlers = RuleManager.ValidateUnimportant(arg);
        // Execute all handlers for unimprtant rules (whether they succeed or failed)
        for (int i = 0; i < handlers.Count; i++) {
            handlers[i](arg);
        }

        if (player.OnMouseRelease != null && player.OnMouseRelease.Count > 0) {
            // We are not done yet, mouse should stay put as the top card of the pile
            // while the mouse can still continue the drag to choose a point in screen to which
            // card effect will take place


            bool prepareReleaseFlag = false;

            // Apply channeling animation
            var spells = player.OnMouseRelease;
            for (int i = 0; i < spells.Count; i++) {
                var spell = spells[i];


                // Let Board get ready
                if (spell.hasMouseTarget) {
                    if (!prepareReleaseFlag) {
                        prepareReleaseFlag = true;
                        EventManager.TriggerEvent("PrepareForRelease", arg);
                    }
                }

                // What should be happening immidiately uppon card placement on pile
                // (and no important rule failed)
                if (spells[i].onCardPlay != null) {
                    spells[i].onCardPlay(new Vector3(0, 0, 0), arg);
                }
            }

        } else {
            EventManager.TriggerEvent("EndTurn", arg);
        }
    }

    public void _startTurnHandler(Hashtable arg) {
        // arg is null

        // Handle draw card from deck to currentPlayer
        Deck.DrawCard(currentPlayer);

        // Increase turn count
        // (it will determine if a round was over)
        TurnCount++;

        // To trigger effects that occour at turn start?
        // (currently doesnt have a handler)
        // Like temple creating believers or something
        EventManager.TriggerEvent("OnTurnStart");
    }


    private void _endTurnHandler(Hashtable arg) {
        // Change next player whoe's going to play
        // Some spell effect changed the next player
        if (nextPlayer != 0) {
            currentPlayerIndex = nextPlayer;
        } else {
            // Set next player as normal based onb turnDirection
            currentPlayerIndex = (currentPlayerIndex + turnDirection) % Players.Count;
        }

        currentPlayer = Players[currentPlayerIndex];


        // A round is over
        if (TurnCount % PlayerCount == 0) {
            // Check win condition
            EventManager.TriggerEvent("RoundOver");
        } else {
            // Handle start turn
            EventManager.TriggerEvent("StartTurn");
        }
    }

    private void _roundOverHandler(Hashtable arg) {
        var peasants = GameObject.FindGameObjectsWithTag("Peasant");
        var total = peasants.Count();
        bool winnerFound = false;

        // WIN CONDITION!
        // If a player has more than winPersantage% ownership on all peasant he/she win the game

        Player player;
        for (int i = 0; i < Players.Count; i++) {
            player = Players[i];
            var playerId = player.id;
            double portion = peasants.Select(p => p.GetComponent<Character>()).Where(c => c.alliance == playerId).Count() / total;
            if (portion > _winPersantage) {
                EventManager.TriggerEvent("GameOver", new Hashtable() { { "winner", player } });
                winnerFound = true;
                break;
            }
        }

        // No need to continue to next turn if we found game winner
        if (!winnerFound) {
            EventManager.TriggerEvent("StartTurn");
        }


    }

    private void SetNextTurn(Hashtable arg) {
        nextPlayer = ((Player)arg["Player"]).id;
    }

    // CREATING PEASANTS AND OTHER CREATURES

    private void CreatePeasants() {
        Debug.Log("Creating Peasant");
        for (int i = 0; i < 100; i++) {
            SpawnRandom(characters[0]);
        }

    }
    public void SpawnRandom(GameObject gameObject) {
        //Set random location for spawn
        Vector3 screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(UnityEngine.Random.Range(0, Screen.width), UnityEngine.Random.Range(0, Screen.height), 10));
        Instantiate(gameObject, screenPosition, Quaternion.identity);
        
        //Debug.Log("Creating character");
    }

    public void CreatePriest() {
        Debug.Log("Creating Priest");
        SpawnRandom(characters[1]);
    }
}
