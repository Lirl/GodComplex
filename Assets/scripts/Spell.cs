using System.Collections;
using UnityEngine;

public delegate void RuleHandler(Hashtable arg);
public delegate void SpellExecution(Vector3 pos);

public class Spell {
    public SpellExecution onClick;

    // Any function that adds effect to any element in game
    // to start, let set the mouse corsur with a circle in order for the user to know the AOE range
    // that will take effect after player release mouse at the chosen target
    // IMPORTANT: keep in mind pos is always Vector3(0,0,0) (The player hasn't yet chose the position)
    public SpellExecution onCardPlay;

    public Hashtable Data;
    public bool hasMouseTarget; // Which means we are waiting for a click to trigger onClick handler
}
