using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

    public GameObject view;
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
       
        if (Input.GetKeyDown(KeyCode.I))
        {
            
            view.SetActive(!view.activeSelf);
        }
    }

   
}
