using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {

    //how many times or how often it should activate its effect
    public enum ActivationType { SingleActivation, CooldownActivation }
    public ActivationType myActivationType = ActivationType.SingleActivation;

    //how the effect is triggered to happen. entry trigger is when the player walks into its area, outside trigger is when something else specifically says "trigger this effect now"
    public enum TriggerType { EntryTrigger, OutsideTrigger }
    public TriggerType myTriggerType = TriggerType.EntryTrigger;

    public enum Behavior { CloseFrontDoor, DisableMainRoomLight}
    public Behavior myBehavior = Behavior.CloseFrontDoor;

    public bool triggered = false;

    GameObject player;
    GameObject manager;
    Controls controlsScript;

    public GameObject affectedObject;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        manager = GameObject.FindGameObjectWithTag("Manager");
        controlsScript = manager.GetComponent<Controls>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if(other.tag == "Player")
        {
            Debug.Log("B");
            if ((myTriggerType == TriggerType.EntryTrigger) && (!triggered))
            {
                Debug.Log("C");
                ActivateTrigger();
            }
            else
            {
                Debug.Log(myTriggerType);
                Debug.Log(triggered);
            }
        }
    }

    void ActivateTrigger()
    {
        Debug.Log("Activating trigger");
        //all the different behaviors the triggered object can do will be listed here
        if(myBehavior == Behavior.CloseFrontDoor)
        {
            CloseFrontDoor();
        }
        triggered = true;
    }

    void CloseFrontDoor()
    {
        Debug.Log("rot");
        //door closing sound
        //shut door
        affectedObject.transform.Rotate(Vector3.up * 79);
    }
}
