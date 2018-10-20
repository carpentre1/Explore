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

    public GameObject affectedObject;



    public enum ActivationType { SingleActivation, CooldownActivation}
    public ActivationType myActivationType = ActivationType.SingleActivation;
    bool activated = false;
    public float cooldown = 0;
    public float interactionDistance = 4;

    public enum Behavior { StareAtPiano }
    public Behavior myBehavior = Behavior.StareAtPiano;

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
        Debug.Log("Activating trigger");
        if(activated && myActivationType == ActivationType.SingleActivation)
        {
            return;
        }
        //all the different behaviors the triggered object can do will be listed here
        if (myBehavior == Behavior.StareAtPiano)
        {
            StareAtPiano();
        }
        activated = true;
        controlsScript.ToggleInteractionText(false, "");
        controlsScript.interactionObject = null;
    }

    void StareAtPiano()
    {
        player.transform.LookAt(gameObject.transform);
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_WalkSpeed -= 4.5f;
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_RunSpeed -= 9.5f;
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().ChangeMouseSensitivity(.1f, .1f);
        controlsScript.piano_initial.Stop();
        controlsScript.piano_note.Play();
        StartCoroutine(StaringAtPianoCoroutine());
    }

    IEnumerator StaringAtPianoCoroutine()
    {
        for (float time = 6f; time >= 0; time -= .05f)
        {
            player.transform.LookAt(gameObject.transform);
            yield return new WaitForSeconds(.05f);
        }
        controlsScript.piano_note.Stop();
        controlsScript.squeak.Play();
        affectedObject.transform.position = new Vector3(affectedObject.transform.position.x, affectedObject.transform.position.y + 1.859f, affectedObject.transform.position.z);
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_WalkSpeed += 4.5f;
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_RunSpeed += 9.5f;
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().ChangeMouseSensitivity(4, 4);

    }

    private void OnMouseOver()
    {
        if(tag == "Interactable")
        {
            if (activated && myActivationType == ActivationType.SingleActivation)
            {
                return;
            }
            if (Vector3.Distance(player.transform.position, gameObject.transform.position) < interactionDistance)
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
