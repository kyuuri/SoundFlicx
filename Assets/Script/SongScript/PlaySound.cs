using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour {

	public AudioSource source;
	public float offset = -0.17f;
	 
	public void playAudio(AudioClip track){
		source.PlayOneShot(track);
	}

	void Start(){
		source = GetComponent<AudioSource> ();
	}

	void Update () {

		if (!source.isPlaying && TimerScript.timePass >= (0 + offset)) {
			playAudio (source.clip);
		}
	}


}
