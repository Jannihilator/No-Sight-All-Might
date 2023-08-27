using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] float maxSpeed;
    Vector2 lastVelocity;
    [SerializeField] TrailRenderer tr;

    // Start is called before the first frame update
    void Start()
    {
        rb.rotation += Random.Range(0,180);
        rb.velocity = transform.up * speed;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        lastVelocity = rb.velocity;
    }
    
    // reflect velocity off the surface
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 surfaceNormal = collision.contacts[0].normal;
        rb.velocity = Vector2.Reflect(lastVelocity, surfaceNormal);
    }

    public IEnumerator ShowTrace(){
        tr.emitting=true;
        yield return new WaitForSeconds(1);
        tr.emitting=false;
    }

    

}
