using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public float speed;
    public float ConvertRadius;
    public int Alliance = 0;
    public Vector3 target;
    private bool _initialized;

    public List<Color> colors;
    public SpriteRenderer SR;
    public Rigidbody2D RB;
    public Collider2D COL;

    public Color Color { get; private set; }
    public bool Paused = false;

    // Use this for initialization
    void Start () {
        init();
        UpdateTarget();
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
    
    private void UpdateTarget() {
        target = transform.position + Infra.RandomVectorInRange(-5f, 5f, -4.6f, 4.6f);

        Invoke("UpdateTarget", UnityEngine.Random.Range(0.5f, 1.2f));
    }

    // Update is called once per frame

    public Vector3 RandomVectorInRange(float xMin, float xMax, float yMin, float yMax) {
        return new Vector3(UnityEngine.Random.Range(xMin, xMax), UnityEngine.Random.Range(yMin, yMax));
    }

    public virtual void Movement() {
        if(Paused) {
            return;
        }

        RB.velocity -= RB.velocity;
        RB.AddForce((GetTargetPosition() - transform.position).normalized * speed * Time.smoothDeltaTime);

        // Handles 
        RB.position = new Vector3(Mathf.Clamp(transform.position.x, -6.0F, 6.0F), Mathf.Clamp(transform.position.y, -4.6F, 4.6F));
    }

    public virtual Vector3 GetTargetPosition() {
        return target;
    }
    public void SetColor() {
        SR.color = colors[Alliance];
    }

    public void SetAlliance(int player) {
        Alliance = player;
        SetColor();
    }

    public virtual Vector3 RandomVectorInRange() {
        return transform.position;
    }

    public void ChangeAlliance(Player p1, Player p2) {
        Alliance = p2.id;
        p1.Characters.Remove(this);
        p2.Characters.Add(this);
    }

    public virtual void Convert() {
        List<Character> toConvert = Infra.GetPeasantsInRange(this.transform.position, ConvertRadius);
        for (int i = 0; i < toConvert.Count; i++) {
            toConvert[i].Alliance = this.Alliance;
        }
    }

    public void DestroyCharacter() {
        Destroy(gameObject);
    }

}
