using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class HarmonyObject : MonoBehaviour
{
	public GameObject click;

    public Baton baton;
    public AudioSource loopSource;
    public AudioClip loopClip;
    public UnityEvent onUnlock;
	public int[] notes;
	public bool unlocked;
	public float earlyStart = .15f;
	public float threshold = .01f;
	public float speed = .0001f;
    public float cheatTime = .1f;  //master loop time - cheat time required to beat level
	public int notesPerOctave = 12;
    
    private float velocityGoal = 0;
	private float velocity = 0;
	private int beatCount = 0;
	private float positionGoal = 0;
	private bool inContact = false;
	private bool moving = false;
	private float beatTime;
    private float completion;

    public bool fantasiaOn = false;

    private float prevTime;
    private float delay;
    private bool prevMoving;

	void Awake()
	{
		loopSource = GetComponent<AudioSource>();
		loopSource.volume = 0;
        loopSource.spatialBlend = 1;
        loopSource.clip = loopClip;
		beatTime = MasterLoop.loopTime / notes.Length;
		positionGoal = ((float)notes[beatCount]) / notesPerOctave;
		transform.localPosition = new Vector3(0, positionGoal, 0);
	}


    // Update is called once per frame
    void Update()
	{
        
        //BATON
        if (inContact)
        {
            completion += Time.deltaTime / (MasterLoop.loopTime - cheatTime);
        } else
        {
            completion = 0;
        }
        AgnosticHand.GetRightBaton().SetCompletion(completion, 0);
        AgnosticHand.GetLeftBaton().SetCompletion(completion, 0);

        //COLOR
        Color color;
        if (unlocked || inContact) {
            color = Color.HSVToRGB(transform.localPosition.y % 1f, 1f, 1f);
            color.a = .75f;

		} else {
            color = Color.HSVToRGB(0, 0, 1);
            color.a = .75f;
		}
		GetComponent<Renderer>().material.color = color;

		if (moving) {
			if (Math.Abs(positionGoal - transform.localPosition.y) < threshold) {
				transform.localPosition = new Vector3(0, positionGoal, 0);
				moving = false;
			} else {
				velocityGoal = (positionGoal - transform.localPosition.y) / 20f;
				if (velocity > velocityGoal) {
					velocity -= speed;
				} else {
					velocity += speed;
				}
				Vector3 delta = new Vector3(0, velocity, 0) ;
				transform.localPosition += delta;
			}
		}

		if (Input.GetKeyDown(KeyCode.N)) {
			Unlock();
		}
	}

	//runs slightly before each beat, so harmony object can get a head start moving
	public void EarlyRunBeat() {
		beatCount++;
		if (beatCount == notes.Length) {
			beatCount = 0;
		} else {
            prevTime = Time.time;
			Invoke("EarlyRunBeat", beatTime);
		}
		if (positionGoal != ((float)notes[beatCount]) / notesPerOctave) {
			positionGoal = ((float)notes[beatCount]) / notesPerOctave;
			moving = true;
		} else {
			moving = false;
		}
	}

	public void NewLoop(){
        if (gameObject.activeInHierarchy)
        {
            if (!fantasiaOn)
            {
                loopSource.Play();
            }
            beatCount = 0;
            prevTime = Time.time;
            Invoke("EarlyRunBeat", beatTime - (beatTime * earlyStart));
        }
	}

	public void OnTriggerEnter()
	{
	  loopSource.volume = 1;
	  inContact = true;
	  click.GetComponent<AudioSource>().Play();

	  Invoke("Unlock", MasterLoop.loopTime - cheatTime);
	}

	public void OnTriggerExit()
	{
	  inContact = false;
	  loopSource.volume = 0;
	  click.GetComponent<AudioSource>().Play();
	  CancelInvoke("Unlock");
	}

	private void Unlock()
	{
		GetComponent<Collider>().enabled = false;
	  	loopSource.volume = 1;
	  	onUnlock.Invoke();
	}

    public void PauseFantasia()
    {
        prevMoving = moving;
        moving = false;
        delay = beatTime - (Time.time - prevTime);
        if (delay < 0) { delay = 0; }
        CancelInvoke();
    }

    public void ResumeFantasia()
    {
        moving = prevMoving;
        Invoke("EarlyRunBeat", delay);
    }
}
