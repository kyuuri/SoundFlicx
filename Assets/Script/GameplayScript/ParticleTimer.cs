using UnityEngine;
using System.Collections;
using WindowsInput;

public class ParticleTimer : MonoBehaviour {

	public ParticleSystem fastParticle;
	public ParticleSystem fastParticle2;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		//to be removed
		if (Input.GetKeyDown ("p") && Input.GetKeyDown ("o")) {
			Application.LoadLevel ("SongSelection");
		}

		if (TimerScript.timePass > 13.9) {
			if (!fastParticle.isPlaying) {
				fastParticle.Play ();
			}
		}
		/*
		if (TimerScript.timePass > 30.2) {
			if (!farRing.isPlaying) {
				farRing.Play ();
			}
		}
		*/
		if (TimerScript.timePass > 30.2) {
			if (!fastParticle2.isPlaying) {
				fastParticle2.Play ();
			}
		}
	}
}
