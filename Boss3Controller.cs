using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss3Controller : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float minSpeed;
    // [SerializeField] float fireTime;
    // [SerializeField] float angularSpeed;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    // [SerializeField] GameObject shockwave;
    GameObject comboSounds;
    Vector2 lastVelocity;
    int combo=0;
    Slider healthBar;
    GameManager gameManager;
    int health = 100;
    bool isHit = false;
    public bool isAttack = false;
    // Start is called before the first frame update
    void Start()
    {
        comboSounds = GameObject.Find("ComboSound");
        rb.velocity = transform.up * speed;
        InvokeRepeating("AttackCaller", 0, 6);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb.velocity.magnitude < minSpeed){
            rb.velocity = rb.velocity.normalized * minSpeed;
        }
        
    }


    void AttackCaller(){
        StartCoroutine(Attack());
    }

    IEnumerator Attack(){
        animator.SetBool("appear", true);
        yield return new WaitForSeconds(0.3f);
        isAttack = true;
        yield return new WaitForSeconds(2.2f);
        animator.SetBool("appear", false);

        isAttack = false;
    }
    
    public void Hit(){
        if(isHit){
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
