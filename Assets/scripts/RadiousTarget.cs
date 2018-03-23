using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiousTarget : MonoBehaviour {

    public Vector3 location;
    public float radius;
    public List<Character> toConvert;
    public Player Player;

    private void Update() {
        this.transform.position = Input.mousePosition;        
    }
}
