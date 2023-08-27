using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkFlip : MonoBehaviour
{
    [SerializeField] Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("isRight", true);
        animator.SetFloat("offset", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        animator.SetBool("isRight", true);
        animator.SetFloat("offset", 0.5f);
    }
}
