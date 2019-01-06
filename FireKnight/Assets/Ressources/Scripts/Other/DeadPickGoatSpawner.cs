using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadPickGoatSpawner : MonoBehaviour {

    [SerializeField] int delay;
    [SerializeField] int intervall;
    [SerializeField] int randomizer;
    [SerializeField] int maxGoats = 5;

    [SerializeField] Transform deadPickGoat_prefab;

    int time;

	// Use this for initialization
	void Start () {
        time = delay;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        time--;

        if (time < 0)
        {
            Instantiate(deadPickGoat_prefab,transform);
            time = intervall + Random.Range(-randomizer,randomizer);

            if (transform.childCount > maxGoats)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
        }
	}
}
