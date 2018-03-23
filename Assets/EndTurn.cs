using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurn : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(EndTurnHandler);
    }

    public void EndTurnHandler() {
        EventManager.TriggerEvent("EndTurn");
    }
}
