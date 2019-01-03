using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEnding : MonoBehaviour {

    public AudioSource Bird;

	// Use this for initialization
	void Start ()
    {
        Bird = GetComponent<AudioSource>();
        Invoke("PlayAudio", 10.0f);
            
            }
	
	// Update is called once per frame
	void Update ()

    {
		
	}

    void PlayAudio()
    {
        Bird.Play();

    }
}
