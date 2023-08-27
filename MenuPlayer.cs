using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPlayer : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed;
    // [SerializeField] float rotateSpeed;
    public float dashPower;
    [SerializeField] float dashTime;
    [SerializeField] float dashDelay;
    [SerializeField] TrailRenderer tr;
    [SerializeField] AudioSource dashSound;
    [SerializeField] GameObject killLight;
    [SerializeField] GameObject startLight;
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject comboSounds;
    [SerializeField] SpriteRenderer bg;
    [SerializeField] SpriteRenderer playbtn;
    [SerializeField] Text playText;
    [SerializeField] Text titleText;
    [SerializeField] Text descriptionText;
    int combo=0;
    bool isDark = true;

    bool isDashing = false;
    bool canDash = true;
    Vector2 move;
    Vector2 mousePosition;
    // Start is called before the first frame update

    void Start () {
        Time.timeScale = 1;
        for(int i=0;i<10;i++){
            float x=Random.Range(-8.5f,8.5f);
            float y=Random.Range(-4.5f,4.5f);
            Instantiate(enemy, new Vector3(x,y,0), Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360))));
        }
        InvokeRepeating("ShowEnemyTrace", 3, 3);

    }
    void Update()
    {
        move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        // mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(Input.GetMouseButtonDown(0) && canDash && isDark){
            StartCoroutine(Dash());
        }
        if(Input.GetKeyDown("space")){
            ToggleLight();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isDashing){
            return;
        }
        
        rb.velocity = move * speed;

        // if(move != Vector2.zero){
        //     Quaternion targetRotation =  Quaternion.LookRotation(transform.forward, move);
        //     transform.rotation = targetRotation;
        // }
        
    }


    void ShowEnemyTrace(){
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(var enemy in enemies){
            StartCoroutine(enemy.GetComponent<EnemyController>().ShowTrace(1));
        }
    }
    void ToggleLight(){
        if(isDark){
            bg.color = Color.white;
            isDark=false;
            titleText.color = Color.black;
            playbtn.color = Color.black;
            playText.color = Color.white;
            descriptionText.color = new Color32(35, 35, 35, 255);
            GetComponent<SpriteRenderer>().color = Color.black;
        }
        else{
            bg.color = Color.black;
            isDark = true;
            titleText.color = Color.white;
            playbtn.color = Color.white;
            playText.color = Color.black;
            descriptionText.color = new Color32(221, 221, 221,255);
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    IEnumerator Dash(){
        dashSound.PlayOneShot(dashSound.clip);
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var jumpVector = mousePosition - new Vector2(transform.position.x, transform.position.y);
        jumpVector.Normalize();
        canDash = false;
        isDashing = true;
        rb.velocity = jumpVector * dashPower;
        tr.emitting = true;
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        tr.emitting = false;
        
        yield return new WaitForSeconds(dashDelay);
        canDash = true;
        
    }

    void PlayGame(){
        SceneManager.LoadScene("SampleScene");
    }
     void OnTriggerEnter2D(Collider2D col){
        if(isDashing){
            if(col.gameObject.CompareTag("Enemy")){
                var sound = comboSounds.transform.GetChild(combo).GetComponent<AudioSource>();
                sound.time = 0.1f;
                sound.Play();
                if(combo<9){
                    combo++;
                }
                Instantiate(killLight, transform.position, Quaternion.identity);
                col.GetComponent<EnemyController>().Hit();
            }
            else{
                Invoke("PlayGame", 1);
                Instantiate(startLight, new Vector3(0, -0.5f, 0), Quaternion.identity);
            }

        }
     }
}
