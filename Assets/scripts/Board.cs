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

    private bool _initialized = false;

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
        GameObject peasant = Resources.Load("Peasant", typeof(GameObject)) as GameObject;
        characters.Add(peasant);
        GameObject priest = Resources.Load("Priest", typeof(GameObject)) as GameObject;
        characters.Add(priest);
    }

    private void _initDateStructure() {
        //Pile = GameObject.FindGameObjectWithTag("Pile").GetComponent<Deck>();
        //Players = GameObject.FindGameObjectsWithTag("Player").Select(x => x.GetComponent<Player>()).ToList();
        //Deck = GameObject.FindGameObjectWithTag("Deck").GetComponent<Deck>();
    }

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
