using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public GameObject gameOver;
    public GameObject win;
    public GameObject timer;
    public Text clock;
    private float timeLeft;
    private int found;
    private List<GameObject> pickups;


	// Use this for initialization
	void Start () {

        pickups = Resources.FindObjectsOfTypeAll<GameObject>().Cast<GameObject>().Where(g => g.tag == "Pick Up").ToList();
        timeLeft = 60;
        clock.text = timeLeft.ToString();
	}
	
	// Update is called once per frame
	void Update () {
        timeLeft -= Time.deltaTime;
        clock.text = timeLeft.ToString();
        if (timeLeft < 0 && found != pickups.Count)
        {
            Time.timeScale = 0f;
            gameOver.SetActive(true);
        }
        else if (timeLeft < 0 && found == pickups.Count) 
        {
            Time.timeScale = 0f;
            win.SetActive(true);
        }
    }

    public void After()
    {
        timeLeft = 100;
        Time.timeScale = 1f;
        gameOver.SetActive(false);
        timer.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);
        found += 1;
    }
}
