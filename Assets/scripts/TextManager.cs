using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// USAGE:
// EventManager.TriggerEvent("ShowText", new Hashtable() { { "Text", "Hello" }, { "Duration", 1f } });

public class TextManager : MonoBehaviour {
    public bool isListen = false;
    private bool _initialized = false;

    private List<Hashtable> texts = new List<Hashtable>();
    private bool _alreadyShowing = false;

    public Text TextCompoenet { get; private set; }
    public List<GameObject> PeasantPortionUI { get; private set; }

    // Use this for initialization
    void Start() {
        Init();
    }
    private void Awake() {
        Init();
    }

    public void Init() {
        if (_initialized) {
            return;
        }

        _initialized = true;
        
        TextCompoenet = GetComponent<Text>();
        _subscribeEvents();
    }

    private void _subscribeEvents() {
        EventManager.StartListening("ShowText", _showTextHandler);


        PeasantPortionUI = new List<GameObject>();
        PeasantPortionUI.Add(GameObject.Find("Portion2"));
        PeasantPortionUI.Add(GameObject.Find("Portion1"));
        EventManager.StartListening("PeasantPortion", _showPeasantPortion);

    }

    private void _showPeasantPortion(Hashtable arg) {
        Player player = (Player)arg["Player"];
        string portion = arg["Portion"].ToString();
        PeasantPortionUI[player.id - 1].GetComponent<Text>().text = portion + '%';
    }

    public void _showTextHandler(Hashtable arg) {

        if (_alreadyShowing) {
            texts.Add(arg);
        } else {
            showText(arg);
        }
    }

    private void showText(Hashtable arg) {
        _alreadyShowing = true;

        TextCompoenet.text = arg["Text"].ToString();
        TextCompoenet.canvasRenderer.SetAlpha(0);
        TextCompoenet.CrossFadeAlpha(255, 1f, false);
        Debug.Log("Trying to inoke fadeout");
        Invoke("FadeOut", (float)arg["Duration"]);
    }

    public void FadeOut() {
        GetComponent<Text>().CrossFadeAlpha(0, 1f, false);
        Invoke("TriggerNext", 1f);
    }

    public void TriggerNext() {
        _alreadyShowing = false;
        if (texts.Count > 0) {
            var arg = texts[0];
            texts.RemoveAt(0);

            showText(arg);
        }
    }
}