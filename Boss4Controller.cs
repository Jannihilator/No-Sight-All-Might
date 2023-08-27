using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss4Controller : MonoBehaviour
{
    [SerializeField] float speed;
    // [SerializeField] float fireTime;
    // [SerializeField] float angularSpeed;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float minSpeed;


    // [SerializeField] GameObject shockwave;
    GameObject comboSounds;
    GameObject disco;
    int combo=0;
    Slider healthBar;
    GameManager gameManager;
    int health = 200;
    bool isHit = false;
    public bool isAttack = false;
    float timer;
    float attackTime=5;
    // Start is called before the first frame update
    void Start()
    {
        comboSounds = GameObject.Find("ComboSound");
        rb.velocity = transform.up * speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb.velocity.magnitude < minSpeed){
            rb.velocity = rb.velocity.normalized * minSpeed;
        }
        timer+=Time.deltaTime;
        if(timer>attackTime){
            StartCoroutine(Attack());
            timer=0;
            attackTime = Random.Range(4.5f,6);
        }
    }


    IEnumerator Attack(){
        if(Random.Range(0,2)==0){
            spriteRenderer.color = Color.white;
        }
        else{
            spriteRenderer.color = Color.black;
        }
        animator.SetBool("isAttack", true);
        yield return new WaitForSeconds(0.2f);
        isAttack = true;
        yield return new WaitForSeconds(1.3f);
        animator.SetBool("isAttack", false);

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
            disco.SetActive(false);
            Destroy(gameObject);
            gameManager.GameEnd();
            healthBar.value = 1;
            healthBar.gameObject.SetActive(false);
        }
        isHit = true;
        Invoke("DelayHit", 0.3f);

    }

    void DelayHit(){
        isHit = false;
    }

    public void Init(GameManager _gameManager, Slider _healthBar, GameObject _disco){
        gameManager =  _gameManager;
        healthBar = _healthBar;
        healthBar.gameObject.SetActive(true);
        disco = _disco;
        disco.SetActive(true);
    }
    

}
