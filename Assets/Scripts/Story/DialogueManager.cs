using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour {
    // Dialogue Info
    public Text DialogueText;
    private string p,o;
    private string[] sentences;
    List<string> jobs = new List<string>() {"Select Job", "Astronaut", "Office Worker", "Wildlife Biologist" };

    // Player Info
    public InputField input_name;
    public GameObject field; 

    // Job Info
    public Dropdown input_occupation; 
    public GameObject dropdown;

    public GameObject next; // Next button

    private int num;  

    // ------------------------------------------------------------------------
    public void Start()
    {
        sentences = new string[7];
        input_occupation.AddOptions(jobs);
        StartDialogue();
    }
    // ------------------------------------------------------------------------
    public void StartDialogue()
    {
        num = 0;
        sentences[0] = "Once upon a time, there was a story about ... I'm sorry, what's your name again?";     
        sentences[4] = "Ehem. Back to the story! Hm... Oh! One day, they found themselves in a predicament.";
        sentences[5] = "Dun dun dun! ... A little too dramatic? Okay, well they found that they couldn't do their job until they found some missing pieces!";
        sentences[6] = "Mwhahahahaha! ... I'm getting carried away now. Let's get into it, shall we?";
        DisplayNextSentence();
    }
    // ------------------------------------------------------------------------
    public void GetInputName(string input)
    {
        p = input;
        sentences[1] = "Ah, that's right. Your name is " + p + "? Interesting...";
        sentences[2] = "Anywho! " + p + " was a different individual who always wanted to be something when they grew up.";
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
        else if (o == "Office Worker")
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
    // ------------------------------------------------------------------------
    public void DisplayNextSentence()
    {    
        DialogueText.text = sentences[num];
        num += 1;

        if (DialogueText.text == sentences[1])
        {
            field.SetActive(false);
      
            next.SetActive(true);

        } else if (DialogueText.text == sentences[2])
        {
            dropdown.SetActive(true);
            next.SetActive(false);
        } else
        {
            dropdown.SetActive(false);
        }

        if(num == sentences.Length)
        {
            EndDialogue();
            return;
        }
    }
    // ------------------------------------------------------------------------
    void EndDialogue()
    {
        if (o == "Astronaut")
        {
            SceneManager.LoadScene("Space");
        }
        else if (o == "Office Worker")
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
