using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {
        rb.AddTorque(500);
        rb.AddForce(speed*transform.up);
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
