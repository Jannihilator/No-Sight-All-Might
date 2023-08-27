using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed;
    // [SerializeField] float rotateSpeed;
    public float dashPower;
    [SerializeField] float dashTime;
    [SerializeField] float dashDelay;
    [SerializeField] TrailRenderer tr;
    [SerializeField] GameObject killLight;
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject comboSounds;
    [SerializeField] AudioSource dashSound;
    [SerializeField] Perk perk1;
    [SerializeField] Perk perk2;
    [SerializeField] GameObject perkMenu;
    [SerializeField] Text comboText;
    [SerializeField] Text perkText;
    [SerializeField] Animator comboAnimator;

    // [SerializeField] Animator animator;
    [SerializeField] int health = 3;
    public int combo=0;
    bool isDashing = false;
    bool canDash = true;
    public bool isBoss = false;
    bool hit = false;
    public bool isDark = false;
    public int dashLeft;
    bool invincible = true;
    Vector2 move;
    Vector2 mousePosition;
    public float killLightSize = 7;
    public GameObject torch;
    public int superBounce=0;
    bool isSuperBounce = false;
    bool gainPerk = false;
    int perkThreshold = 3;
    public bool isChoosing = false;
    public bool isDialogue = false;

    void Start(){
        StartCoroutine(TurnInvicibleFor(1.0f));
    }

    public IEnumerator TurnInvicibleFor(float time){
        invincible = true;
        yield return new WaitForSeconds(time);
        invincible = false;
    }


    void Update()
    {
        move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        // mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(Input.GetMouseButtonDown(0) && canDash && isDark && dashLeft>0){
            StartCoroutine(Dash());
        }
        
        if(Input.GetMouseButtonDown(1) && superBounce>0 && isDark){
            StartCoroutine(SuperBounce());
            superBounce--;
        }
        
    }

    // public void Blink(bool shouldBlink){
    //     animator.SetBool("Blink", shouldBlink);
    // }

    

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isDashing || isSuperBounce){
            return;
        }
        
        rb.velocity = move * speed;

        // if(move != Vector2.zero){
        //     Quaternion targetRotation =  Quaternion.LookRotation(transform.forward, move);
        //     transform.rotation = targetRotation;
        // }
        
    }

    IEnumerator SuperBounce(){
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var jumpVector = mousePosition - new Vector2(transform.position.x, transform.position.y);
        jumpVector.Normalize();
        isSuperBounce = true;
        rb.velocity = jumpVector * 50;
        GetComponent<SpriteRenderer>().color = Color.white;
        tr.emitting = true;

        yield return new WaitForSeconds(6);
        tr.emitting = false;

        isSuperBounce = false;
        if(!isBoss){
            GetComponent<SpriteRenderer>().color = Color.black;
        }

    }

    IEnumerator Dash(){
        dashSound.PlayOneShot(dashSound.clip);
        hit = false;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var jumpVector = mousePosition - new Vector2(transform.position.x, transform.position.y);
        jumpVector.Normalize();
        canDash = false;
        isDashing = true;
        rb.velocity = jumpVector * dashPower;
        tr.emitting = true;
        StartCoroutine(TurnInvicibleFor(0.5f));
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        tr.emitting = false;
        if(!hit){
            dashLeft--;
            combo = 0;
            if(gainPerk){
                perkThreshold++;
                gainPerk=false;
            }
            comboAnimator.SetBool("noCombo", true);

        }
        gameManager.UpdateDashMenu(dashLeft);
        yield return new WaitForSeconds(dashDelay);
        canDash = true;
        
    }

    void SetComboBool(){
        comboAnimator.SetBool("newCombo", false);
    }

    void ShowPerk(){
        isChoosing=true;
        int range;
        perkMenu.SetActive(true);
        perkText.text = perkThreshold.ToString()+"X bonus: choose one\nnext perk: "+(perkThreshold+1).ToString()+"X";
        Time.timeScale = 0;
        if(gameManager.isTorch){
            range = 1;
        }
        else{
            range = 0;
        }
        int random1 = Random.Range(range, 6);
        int random2 = Random.Range(range, 6);
        while(random1 == random2){
            random2 = Random.Range(range, 6);
        }
        perk1.SetPerk(random1);
        perk2.SetPerk(random2);
        
    }

    public void HidePerk(){
        perkMenu.SetActive(false);
        Time.timeScale = 1;
        isChoosing=false;
        if(isDialogue){
            gameManager.banner.gameObject.SetActive(true);
            gameManager.banner.StartDialogue(gameManager.level);
            isDialogue=false;
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        if(isDashing || isSuperBounce){
            if(col.gameObject.CompareTag("Enemy")){
                var sound = comboSounds.transform.GetChild(combo).GetComponent<AudioSource>();
                sound.time = 0.1f;
                sound.Play();
                if(combo<9){
                    combo++;
                    comboText.text = combo.ToString()+"X";
                    comboAnimator.SetBool("noCombo", false);
                    comboAnimator.SetBool("newCombo", true);
                    Invoke("SetComboBool", 0.05f);
                }
                col.GetComponent<EnemyController>().Hit();
                if(combo==perkThreshold){
                    ShowPerk();
                    gainPerk = true;
                }
                    
                hit = true;
                if(gameManager.enemyLeft == 1){
                    gameManager.enterBoss = true;
                    isBoss = true;
                    combo = 0;
                    comboAnimator.SetBool("noCombo", true);
                    return;
                }
                Instantiate(killLight, transform.position, Quaternion.identity).transform.localScale = Vector3.one * killLightSize;
            }
            else if(col.gameObject.CompareTag("Light")){
                Destroy(col.gameObject);
                StartCoroutine(gameManager.SceneFlash(10));
                hit = true;
            }
            else if(col.gameObject.CompareTag("Projectile")){
                if(!isSuperBounce){
                    Hit();
                }
            }
            else if(col.gameObject.CompareTag("Boss")){
                switch(gameManager.level){
                    case 1:
                        col.GetComponent<BossController>().Hit();
                        hit = true;
                        break;
                    case 2:
                        var boss2 = col.GetComponent<Boss2Controller>();
                        if(boss2.isAttack  && !isSuperBounce){
                            Hit();
                        }
                        else{
                            boss2.Hit();
                            hit = true;

                        }
                        break;
                    case 3:
                        var boss3 = col.GetComponent<Boss3Controller>();
                        if(boss3.isAttack && !isSuperBounce){
                            Hit();
                        }
                        else{
                            boss3.Hit();
                            hit = true;
                        }
                        break;
                    case 4:
                        var boss4 = col.GetComponent<Boss4Controller>();
                        if(boss4!=null){
                            if(boss4.isAttack && !isSuperBounce){
                                Hit();
                            }
                            else{
                                boss4.Hit();
                                hit = true;
                            }
                        }
                        break;


                }
            }
        }
        else{
            if(!invincible && (col.gameObject.CompareTag("Boss") || col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("Projectile"))){
                // Destroy(gameObject);
                Hit();
            }
            //player lose health
        }
    }


    public void Heal(){
        if(health<5){
            health++;
        }
        gameManager.UpdateLifePanel(health, true);

    }

    void Hit(){
        health--;
        if(gameManager.level==4 && isBoss){
            StartCoroutine(TurnInvicibleFor(1));
        }
        else{
            StartCoroutine(TurnInvicibleFor(0.5f));
        }
        gameManager.UpdateLifePanel(health,false);
        if(health > 0){
            StartCoroutine(gameManager.SceneFlash(5));
        }
        if(health == 0){
            gameManager.PlayerDied();
        }
    }
}
