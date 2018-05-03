using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class PlayerGravity : MonoBehaviour {

    public Planet planet;
    private Transform playerTransform;

	// Use this for initialization
	void Start () {
        planet = GameObject.FindGameObjectWithTag("Planet").GetComponent<Planet>();
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

        playerTransform = transform;
    }
	
	// Update is called once per frame
	void Update () {
        planet.Attract(playerTransform);
        
	}
}
