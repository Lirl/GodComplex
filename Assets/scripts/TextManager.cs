using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour {
    public bool isListen = false;
	// Use this for initialization
	void Start () {
        Init();
    }
    private void Awake() {
        Init();
    }

    public void Init() {
        EventManager.StartListening("ShowText", _showTextHandler);
    }
    // Update is called once per frame
    void Update () {

	}

    public void _showTextHandler(Hashtable arg) {
        GetComponent<Text>().text = arg["Text"].ToString();
        GetComponent<Text>().canvasRenderer.SetAlpha(0);
        GetComponent<Text>().CrossFadeAlpha(255, 1000f, false);
        Invoke("FadeOut", 3);
    }

    public void FadeOut() {
        GetComponent<Text>().CrossFadeAlpha(0, 5f, false);
    }
}
