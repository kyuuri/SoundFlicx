using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour {

	public AudioSource source;
	public float offset = -0.17f;
	private bool isPlayed = false;
	 
	public void playAudio(AudioClip track){
		source.PlayOneShot(track);
	}

	void Start(){
		source = GetComponent<AudioSource> ();

		string songName = GlobalData.selectedTrack.songName;
		if (songName == null) {
			songName = "Test";
		}
		source.clip = Resources.Load ("Tracks/" + songName + "/audio.mp3") as AudioClip;
	}

	void Update () {

		if (!isPlayed && TimerScript.timePass >= (0 + offset)) {
			playAudio (source.clip);
			isPlayed = true;
		}
	}


}
