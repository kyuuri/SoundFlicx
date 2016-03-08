using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour {

	public AudioSource baseSource;

	public float offset;
	private bool isPlayed = false;
	 
	public void playAudio(){
		baseSource.PlayOneShot (baseSource.clip);
	}

	void Start(){
		baseSource = GetComponent<AudioSource> ();

		offset = GlobalData.selectedTrack.offset;
		string songName = GlobalData.selectedTrack.songName;

		baseSource.clip = Resources.Load (songName + "Audio.mp3") as AudioClip;
	}

	void Update () {

		if (!isPlayed && TimerScript.timePass >= (0 + offset)) {
			playAudio ();
			isPlayed = true;
		}

		if (!baseSource.isPlaying && isPlayed) { // song ends
			Application.LoadLevel("ResultScene");
		}
	}
}
