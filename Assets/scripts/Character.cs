using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public float speed;
    public int alliance = 0;
    public Vector3 target;

    public List<Color> colors;
    public SpriteRenderer SR;
    public Rigidbody2D RB;
    public Collider2D COL;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual Vector3 RandomVectorInRange() {
        return transform.position;
    }

    public virtual void Movement() {

    }

    public virtual Vector3 GetTargetPosition() {
        return transform.position;
    }
}
