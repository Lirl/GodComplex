using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public float speed;
    public int alliance = 0;
    public Vector3 target;
    private bool _initialized;

    public List<Color> colors;
    public SpriteRenderer SR;
    public Rigidbody2D RB;
    public Collider2D COL;

    public Color Color { get; private set; }

    // Use this for initialization
    void Start () {
        init();
	}

    private void Awake() {
        init();
    }

    public void init() {
        if (_initialized) {
            return;
        }
        _initialized = true;
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        COL = GetComponent<Collider2D>();
    }

    public void SetColor() {
        SR.color = colors[alliance];
    }

    public void SetAlliance(int player) {
        alliance = player;
        SetColor();
    }

    public virtual Vector3 RandomVectorInRange() {
        return transform.position;
    }

    public virtual void Movement() {

    }

    public virtual Vector3 GetTargetPosition() {
        return transform.position;
    }

    public void ChangeAlliance(Player p1, Player p2) {
        alliance = p2.id;
        p1.Characters.Remove(this);
        p2.Characters.Add(this);
    }
}
