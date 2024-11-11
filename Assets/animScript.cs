using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animScript : MonoBehaviour
{
    Animator anim;

    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckKey();
    }

    void CheckKey()
    {
        // Check if any movement keys are pressed
        bool isRunning = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        if (isRunning)
        {
            anim.SetBool("run", true);
            anim.SetBool("idle", false);
        }
        else
        {
            anim.SetBool("run", false);
            anim.SetBool("idle", true);
        }

        // Check for the shooting action
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("shoot", true);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            anim.SetBool("shoot", false);
        }
    }
}
