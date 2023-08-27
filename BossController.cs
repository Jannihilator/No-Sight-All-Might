using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float fireTime;
    [SerializeField] float angularSpeed;
    // [SerializeField] GameObject shockwave;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform firePoint1;
    [SerializeField] Transform firePoint2;
    GameObject comboSounds;
    int combo=0;
    Slider healthBar;
    GameManager gameManager;
    float smoothTime = 0.5f;
    float timer = 0;
    Vector3 velocity;
    Vector3 targetPosition;
    int health = 100;
    bool firing = false;
    bool isHit = false;
    // Start is called before the first frame update
    void Start()
    {
        comboSounds = GameObject.Find("ComboSound");
        targetPosition = transform.position;
        InvokeRepeating("RandomRoam", 0, 7);
    }

    // Update is called once per frame
    void Update()
    {
        if(firing){
            transform.Rotate( new Vector3(0, 0, angularSpeed * Time.deltaTime) );
            timer += Time.deltaTime;
            if(timer > fireTime){
                timer = 0;
                Fire();
            }
        }
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime, speed);     
    }

    void RandomRoam(){
        while(Vector3.Distance(targetPosition, transform.position)<7){
            float x=Random.Range(-8.5f,8.5f);
            float y=Random.Range(-4.5f,4.5f);
            targetPosition = new Vector3(x,y,0);
        }
        StartCoroutine(ShockWave());
    }

    IEnumerator ShockWave(){
        yield return new WaitForSeconds(1);
        firing = true;
        yield return new WaitForSeconds(5);
        firing = false;
        // yield return new WaitForSeconds(1);
        // Instantiate(shockwave, transform.position, transform.rotation);
        // yield return new WaitForSeconds(0.25f);
        // Instantiate(shockwave, transform.position, transform.rotation);
    }

    void Fire(){
        Instantiate(projectile, firePoint1.position, firePoint1.rotation);
        Instantiate(projectile, firePoint2.position, firePoint2.rotation);
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
