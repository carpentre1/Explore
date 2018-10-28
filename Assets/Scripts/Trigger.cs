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

    public enum Behavior { CloseFrontDoor, DisableMainRoomLight, RugForwards, StartPiano, LeadingLight1, LeadingLight2, BathroomSpook}
    public Behavior myBehavior = Behavior.CloseFrontDoor;

    public bool triggered = false;

    GameObject player;
    GameObject manager;
    Controls controlsScript;

    public GameObject affectedObject;
    public GameObject secondAffectedObject;

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
        if (myBehavior == Behavior.RugForwards)
        {
            RugForwards();
        }
        if (myBehavior == Behavior.StartPiano)
        {
            StartPiano();
        }
        if (myBehavior == Behavior.LeadingLight1)
        {
            LeadingLight1();
        }
        if (myBehavior == Behavior.LeadingLight2)
        {
            LeadingLight2();
        }
        if (myBehavior == Behavior.BathroomSpook)
        {
            BathroomSpook();
        }
        triggered = true;
    }

    void CloseFrontDoor()
    {
        Debug.Log("rot");
        controlsScript.StopWindAndCloseDoor();
        affectedObject.transform.Rotate(Vector3.up * 79);
    }

    void RugForwards()
    {
        controlsScript.rustle.Play();
        StartCoroutine(RugMovementCoroutine(true));
    }

    void StartPiano()
    {
        controlsScript.piano_initial.Play();
    }
    void LeadingLight1()
    {
        StartCoroutine(FlickerLightAndMoveCoroutine(9f, Vector3.left));
    }
    void LeadingLight2()
    {
        StartCoroutine(FlickerLightAndMoveCoroutine(4f, Vector3.forward));
    }
    void BathroomSpook()
    {
        bool pianoWasPlaying = false;
        if(controlsScript.piano_initial.isPlaying)
        {
            controlsScript.piano_initial.Pause();
            pianoWasPlaying = true;
        }
        controlsScript.piano_veryspook.Play();
        secondAffectedObject.SetActive(true);
        StartCoroutine(BathroomLightColorShift(pianoWasPlaying));
    }

    IEnumerator BathroomLightColorShift(bool pianoWasPlaying)
    {
        var startColor = Color.green;
        var endColor = Color.red;
        float curColor = 0;
        float fadeTime = 6f;
        Light theLight = affectedObject.GetComponent<Light>();
        theLight.enabled = true;
        theLight.color = Color.green;

        while(curColor <= 1)
        {
            curColor += Time.deltaTime / fadeTime;
            theLight.color = Color.Lerp(startColor, endColor, curColor);
            yield return new WaitForSeconds(.01f);
        }

        for (float time = 4f; time >= 0; time -= .01f)
        {
            controlsScript.piano_veryspook.volume -= controlsScript.piano_veryspook.volume * .01f;
            yield return new WaitForSeconds(.01f);
        }

        theLight.enabled = false;
        controlsScript.piano_veryspook.Stop();
        secondAffectedObject.SetActive(false);
        if (pianoWasPlaying)
        {
            controlsScript.piano_initial.UnPause();
        }
    }

    IEnumerator FlickerLightAndMoveCoroutine(float distance, Vector3 dir)
    {
        Light theLight = affectedObject.GetComponent<Light>();
        theLight.enabled = true;
        float origIntensity = theLight.intensity;
        theLight.intensity = 0;
        for (float time = 1.5f; time >= 0; time -= .01f)
        {
            theLight.intensity += origIntensity * .01f;
            affectedObject.transform.Translate(dir * Time.deltaTime * distance);
            yield return new WaitForSeconds(.01f);
        }
        for (float time = 1.5f; time >= 0; time -= .01f)
        {
            theLight.intensity -= origIntensity * .01f;
            affectedObject.transform.Translate(dir * Time.deltaTime * distance);
            yield return new WaitForSeconds(.01f);
        }
        Debug.Log("finished");
    }

    IEnumerator RugMovementCoroutine(bool forwards)
    {
        for (float initial_time = .6f; initial_time >= 0; initial_time -= .03f)
        {
            yield return new WaitForSeconds(.03f);
        }
        for (float time = 1f; time >= 0; time -= .03f)
        {
            if(forwards)
            {
                Debug.Log("yee");
                affectedObject.transform.position = new Vector3(affectedObject.transform.position.x + .1f, affectedObject.transform.position.y, affectedObject.transform.position.z);
            }
            else
            {
                affectedObject.transform.position = new Vector3(affectedObject.transform.position.x - .1f, affectedObject.transform.position.y, affectedObject.transform.position.z);
            }
            yield return new WaitForSeconds(.03f);
        }
    }
}
