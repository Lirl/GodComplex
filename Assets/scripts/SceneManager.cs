using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {

    public float timeBeforeStart;

  
    public void Start() {
    }
    public void QuitRequest() {
        Debug.Log("Quit requested");
        Application.Quit();
    }

    public void LoadNextLevel(string name) {
        Application.LoadLevel(name);
    }
}
