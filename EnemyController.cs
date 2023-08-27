using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] float maxSpeed;
    [SerializeField] TrailRenderer tr;
    int health=1;
    GameManager gameManager;
    PlayerController playerController;
    bool runaway = false;

    Vector2 runDirection;


    // Start is called before the first frame update
    void Start()
    {
        rb.rotation += Random.Range(0,180);
        // rb.velocity = transform.up * speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(runaway){
            rb.velocity = -runDirection*20;
            return;
        }
        rb.AddForce(transform.up * speed);
        // rb.rotation += 1f;

        if (rb.velocity.magnitude > maxSpeed){
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        }
        // lastVelocity = rb.velocity;
    }

    public IEnumerator ShowTrace(float time){
        tr.time = time;
        tr.emitting=true;
        yield return new WaitForSeconds(time);
        tr.emitting=false;
    }

    public void Hit(){
        health--;
        if(health == 0){
            Destroy(gameObject);
            if(gameManager!=null){
                gameManager.enemyLeft--;
            }
        }
        else{
            transform.localScale = new Vector3(transform.localScale.x*0.6f, 0.2f, 1);
            speed++;
            runaway = true;
            runDirection = transform.position - playerController.transform.position;
            Invoke("StopRun", 0.4f);
        }
    }

    void StopRun(){
        runaway=false;
    }

    public void Init(int _health, GameManager _gameManager, PlayerController _playerController){
        health = _health;
        gameManager = _gameManager;
        playerController =  _playerController;
    }



    

}
