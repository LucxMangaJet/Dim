using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleClass : MonoBehaviour
{
    public Animator anim;

    void Start()
    {

        anim = GetComponent<Animator>();

    }

    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            anim.Play("MainDoor");

        }
    }

}