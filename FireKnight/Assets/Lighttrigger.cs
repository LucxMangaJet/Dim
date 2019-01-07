using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighttrigger : MonoBehaviour {



    [SerializeField] GameObject Objecttotigger;

    private void OnTriggerEnter(Collider other)
    {
        Objecttotigger.SetActive(true);
    }






    // Use this for initialization
    void Start ()

    {
		
	}
	
	
}
