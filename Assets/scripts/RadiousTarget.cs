using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiousTarget : MonoBehaviour {

    public Vector3 location;
    public float radius;
    public List<Character> toConvert;
    public Player Player;

    private void Update() {

        // TODO: check if player is human, if so follow mouse

        this.transform.position = Input.mousePosition;        
    }
}
