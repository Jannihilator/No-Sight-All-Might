using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss2Controller : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float minSpeed;
    // [SerializeField] float fireTime;
    // [SerializeField] float angularSpeed;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] BoxCollider2D col;
    // [SerializeField] GameObject shockwave;
    GameObject comboSounds;
    Vector2 lastVelocity;
    int combo=0;
    int cd;
    Slider healthBar;
    GameManager gameManager;
    int health = 100;
    bool isHit = false;
    public bool isAttack = false;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.up * speed;
        comboSounds = GameObject.Find("ComboSound");
        InvokeRepeating("AttackCaller", 2, 8f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb.velocity.magnitude < minSpeed){
            rb.velocity = rb.velocity.normalized * minSpeed;
        }
        // if(rb.angularVelocity > 500){
        //     rb.angularVelocity = 500;
        // }
    }

    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     Vector2 surfaceNormal = collision.contacts[0].normal;
    //     rb.velocity = Vector2.Reflect(lastVelocity, surfaceNormal);
    // }

    void AttackCaller(){
        cd = Random.Range(3, 6);
        StartCoroutine(Attack());
    }

    IEnumerator Attack(){
        Animator[] children = GetComponentsInChildren<Animator>();
        foreach(Animator child in children) {
            child.SetBool("isAttack", true);
        }
        yield return new WaitForSeconds(1.5f);
        isAttack = true;
        col.size = new Vector2(col.size.x, col.size.y*3);
        yield return new WaitForSeconds(cd);
        isAttack = false;
        col.size = new Vector2(col.size.x, col.size.y/3);
        foreach(Animator child in children) {
            child.SetBool("isAttack", false);
        }
    }
    
    public void Hit(){
        if(isHit || isAttack){
            return;
        }
        health -= 20;
        healthBar.value = health/100f;
        // Debug.Log(health);
        var sound = comboSounds.transform.GetChild(combo).GetComponent<AudioSource>();
        sound.time = 0.1f;
        sound.Play();
        combo++;
        if(health == 0){
            Destroy(gameObject);
            gameManager.NextLevel();
            healthBar.value = 1;
            healthBar.gameObject.SetActive(false);
        }
        isHit = true;
        Invoke("DelayHit", 0.3f);

    }

    void DelayHit(){
        isHit = false;
    }

    public void Init(GameManager _gameManager, Slider _healthBar){
        gameManager =  _gameManager;
        healthBar = _healthBar;
        healthBar.gameObject.SetActive(true);
    }
    

}
