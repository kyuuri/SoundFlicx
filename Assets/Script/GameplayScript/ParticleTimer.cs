using UnityEngine;
using System.Collections;

public class ParticleTimer : MonoBehaviour {

	public ParticleSystem fastParticle;
	public ParticleSystem farRing;
	public ParticleSystem fastParticle2;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		if (TimerScript.timePass > 13.9) {
			if (!fastParticle.isPlaying) {
				fastParticle.Play ();
			}
		}
		if (TimerScript.timePass > 30.2) {
			if (!farRing.isPlaying) {
				farRing.Play ();
			}
		}
		if (TimerScript.timePass > 45.0) {
			if (!fastParticle2.isPlaying) {
				fastParticle2.Play ();
			}
		}
	}
}
