using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script is attached to an object and tells it what to do when the player interacts with it. it also recognizes when the player mouses over it and displays the interaction text
public class Interaction : MonoBehaviour {

    const string INTERACTION_CONTROL_TEXT = "[E] ";
    string interactionText = "Interact";
    GameObject player;
    GameObject manager;
    Controls controlsScript;



    public enum ActivationType { SingleActivation, CooldownActivation}
    public ActivationType myActivationType = ActivationType.SingleActivation;
    bool activated = false;
    public float cooldown = 0;
    public float interactionDistance = 4;

	// Use this for initialization
	void Start () {
        //establish the player
        player = GameObject.FindGameObjectWithTag("Player");
        manager = GameObject.FindGameObjectWithTag("Manager");
        controlsScript = manager.GetComponent<Controls>();
        Debug.Log(controlsScript);
	}
	

    public void PerformInteraction()
    {
        //do what this object should do
        Debug.Log("interacted!");
    }

    private void OnMouseOver()
    {
        if(tag == "Interactable")
        {
            if(Vector3.Distance(player.transform.position, gameObject.transform.position) < interactionDistance)
            {
                controlsScript.ToggleInteractionText(true, INTERACTION_CONTROL_TEXT + interactionText);
                //assign player's interaction object
                controlsScript.interactionObject = gameObject;
            }
            else//if it's too far, check to see if it was previously interactable and disable that
            {
                if(controlsScript.interactionObject = gameObject)
                {
                    controlsScript.ToggleInteractionText(false, "");
                    controlsScript.interactionObject = null;
                }
            }
        }
    }

    private void OnMouseExit()
    {
        controlsScript.ToggleInteractionText(false, "");
        controlsScript.interactionObject = null;
    }
}
