using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimMAINMENU : MonoBehaviour
{
    public Animator anim;
    bool Isopening = false;
    public AudioSource DoorSound;

    void Start()
    {

        anim = GetComponent<Animator>();
        DoorSound = GetComponent<AudioSource>();

    }

void Update() ///when Menu finished, DELETE this Updatefunction
    {
        if (Input.GetKeyDown("1"))
        {
            DoorOpening();

        }
        if (Isopening == true)
        {
            DoorOpening();
        }
    }

    public void DoorOpening()
    {
        anim.Play("MainDoor");
        DoorSound.Play();

    }



}