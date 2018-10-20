using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class Controls : MonoBehaviour {

    public GameObject pausePanel;
    public GameObject onscreenPanel;
    public FirstPersonController FPCscript;
    public GameObject interactionText;

    //[HideInInspector]
    public GameObject interactionObject;
    Interaction interactionScript;

    public AudioSource wind;
    public AudioSource door_close;
    public AudioSource rustle;
    public AudioSource piano_initial;
    public AudioSource piano_note;
    public AudioSource squeak;

    bool isPaused = false;

	// Use this for initialization
	void Start () {
        pausePanel.SetActive(false);
        interactionText.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause(true);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractWithObject();
        }
    }

    public void ToggleInteractionText(bool on, string theText)
    {
        if (on)
        {
            interactionText.SetActive(true);
            interactionText.GetComponent<Text>().text = theText;
            //interactionText = interactionObject.
        }
        else
        {
            interactionText.SetActive(false);
        }
    }

    void InteractWithObject()
    {
        if(!interactionObject)
        {
            Debug.Log("No interaction object.");
            return;
        }
        else
        {
            if(!interactionObject.GetComponent<Interaction>())
            {
                Debug.Log("The object has no interaction script.");
                return;
            }
            interactionScript = interactionObject.GetComponent<Interaction>();
            interactionScript.PerformInteraction();
        }
    }

    public void TogglePause(bool usedKeyboard=false)
    {
        if(isPaused)
        {
            Debug.Log("unpausing");
            if (usedKeyboard) { return; }//unpausing with escape key refuses to hide mouse, so this functionality is disabled for now
            isPaused = false;
            //Time.timeScale = 1;
            pausePanel.SetActive(false);
            onscreenPanel.SetActive(true);
            FPCscript.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;

        }
        else
        {
            Debug.Log("pausing");
            isPaused = true;
            //Time.timeScale = 0;
            pausePanel.SetActive(true);
            onscreenPanel.SetActive(false);
            FPCscript.enabled = false;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }

    public void StopWindAndCloseDoor()
    {
        wind.Stop();
        door_close.Play();
    }
}
