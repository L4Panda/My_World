using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour {

    // Dialogue Info
    public Text DialogueText;
    private string p, o, t, s;
    private string[] sentences;
    List<string> jobs = new List<string>() {"Select Job", "Astronaut", "Businessman", "Wildlife Biologist" };
    List<string> seasons = new List<string>() { "Select Season", "Spring", "Summer", "Fall", "Winter" };

    // Player Info
    public GameObject field; 

    // Job Info
    public Dropdown input_occupation; 
    public GameObject dropdown;

    // Time Info
    public GameObject morning;
    public GameObject night;

    // Season Info
    public Dropdown input_season;
    public GameObject dropdown2;
    
    public GameObject next; // Next button
    public GameObject view; // View button

    private int num;

    // ------------------------------------------------------------------------
    public void Start()
    {
        sentences = new string[9];
        input_occupation.AddOptions(jobs);
        input_season.AddOptions(seasons);
        StartDialogue();
    }
    // ------------------------------------------------------------------------
    public void StartDialogue()
    {
        num = 0;
        sentences[0] = "Once upon a time, there was a story about ... I'm sorry, what's your name again?";
        
        
        DisplayNextSentence();
    }
    // ------------------------------------------------------------------------
    public void GetInputName(string input)
    {
        p = input;
        sentences[1] = "Ah, that's right. Your name is " + p + "? Interesting...";
        sentences[2] = "Anywho! " + p + " was a different individual who always wanted to be something when they grew up.";
        sentences[4] = "Ehem. Back to the story! Hm ... Oh! " + p + " was more of a: ";
        sentences[6] = p + " had a favorite season which was... Um...";
        sentences[8] = "Are you ready to explore " + p + "'s world?";
    }
    // ------------------------------------------------------------------------
    public void GetInputJob(int index)
    {
        o = jobs[index];
        if (o == "Astronaut")
        {
            next.SetActive(true);
            sentences[3] = "Ooo! Spacey. I love those people. They are so great at doing the things and the stuffs.";
        }
        else if (o == "Businessman")
        {
            next.SetActive(true);
            sentences[3] = "Oohhh... That doesn't sound too appealing. To each their own!";
        }
        else if (o == "Wildlife Biologist")
        {
            next.SetActive(true);
            sentences[3] = "Aww!  They must really love animals. I had a pet fish once... ";
        }
        else
        {
            next.SetActive(false);
        }
    }

    public void GetInputSeason(int index)
    {
        s = seasons[index];
        if (s == "Spring")
        {
            next.SetActive(true);
            sentences[7] = "Ah, the rain and the flowers and the allergies. How nice.";
        }
        else if (s == "Summer")
        {
            next.SetActive(true);
            sentences[7] = "Hot! Hot! Hot! Much too hot for me! The beach is nice though.";
        }
        else if (s == "Fall")
        {
            next.SetActive(true);
            sentences[7] = "I love when the trees change colors and who could forget thanksgiving? Yummy.";
        }
        else if (s == "Winter")
        {
            next.SetActive(true);
            sentences[7] = "The snow is so beautiful, isn't it? Do you want to build a snowman?";
        }
        else
        {
            next.SetActive(false);
        }
    }


    public void Morning()
    {
        t = "Morning";
        sentences[5] = "Of course. You like the birds singing and the sun rising.";
        DisplayNextSentence();
    }

    public void Night()
    {
        t = "Night";
        sentences[5] = "Of course. You like the night sky and the illuminating moon.";
        DisplayNextSentence();
    }
    // ------------------------------------------------------------------------
    public void DisplayNextSentence()
    {    
        DialogueText.text = sentences[num];
        num += 1;

        if (DialogueText.text == sentences[1])
        {
            field.SetActive(false);    
            next.SetActive(true);

        }
        else if (DialogueText.text == sentences[2])
        {
            dropdown.SetActive(true);
            next.SetActive(false);
        }
        else if (DialogueText.text == sentences[3])
        {
            dropdown.SetActive(false);
        }
        else if (DialogueText.text == sentences[4])
        {
            morning.SetActive(true);
            night.SetActive(true);
            next.SetActive(false);
        }
        else if (DialogueText.text == sentences[5])
        {
            morning.SetActive(false);
            night.SetActive(false);
            next.SetActive(true);
        }
        else if (DialogueText.text == sentences[6])
        {
            dropdown2.SetActive(true);
            next.SetActive(false);
        }
        else if (DialogueText.text == sentences[8])
        {
            dropdown2.SetActive(false);
            view.SetActive(true);
            next.SetActive(false);
            
        }
    }
    // ------------------------------------------------------------------------
    public void EndDialogue()
    {
        if (o == "Astronaut")
        {
            
            SceneManager.LoadScene("Space");
        }
        else if (o == "Businessman")
        {
            
            SceneManager.LoadScene("City");
        }
        else if (o == "Wildlife Biologist")
        {
          
            SceneManager.LoadScene("Island");
        }
    }
    // ------------------------------------------------------------------------
}
