using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peasant : Character {

    private float CurrTime = 0;

    public Color Color { get; private set; }

    // Use this for initialization
    void Start() {
        RB = GetComponent<Rigidbody2D>();
        UpdateTarget();
    }

    private void UpdateTarget() {
        target = transform.position + Infra.RandomVectorInRange(-5f, 5f, -4.6f, 4.6f);

        Invoke("UpdateTarget", 0.8f);
    }

    // Update is called once per frame
    void Update() {
        Movement();
        SetColor();
    }

    private void SetColor() {
        SR.color = colors[alliance];
    }

    public Vector3 RandomVectorInRange(float xMin, float xMax, float yMin, float yMax) {
        return new Vector3(UnityEngine.Random.Range(xMin, xMax), UnityEngine.Random.Range(yMin, yMax));
    }

    public override void Movement() {
        RB.velocity -= RB.velocity;
        RB.AddForce((GetTargetPosition() - transform.position).normalized * speed * Time.smoothDeltaTime);  

        // Handles 
        RB.position = new Vector3(Mathf.Clamp(transform.position.x, -6.0F, 6.0F), Mathf.Clamp(transform.position.y, -4.6F, 4.6F));

        //Debug.LogError("Enemy:" + transform.position);
    }

    public override Vector3 GetTargetPosition() {
        return target;
    }
}
