using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour {

	public AudioSource source;
	public float offset;
	private bool isPlayed = false;
	 
	public void playAudio(AudioClip track){
		source.PlayOneShot(track);
	}

	void Start(){
		source = GetComponent<AudioSource> ();
		offset = GlobalData.selectedTrack.offset;

		string songName = GlobalData.selectedTrack.songName;
		if (songName == null) {
			//songName = "Test";
			songName = "The_Clear_Blue_Sky";
		}
		source.clip = Resources.Load (songName + "Audio.mp3") as AudioClip;
	}

	void Update () {

		if (!isPlayed && TimerScript.timePass >= (0 + offset)) {
			playAudio (source.clip);
			isPlayed = true;
		}

		//if(TimerScript.timePass > 10f){
		if (!source.isPlaying && isPlayed) { // song ends
			Application.LoadLevel("ResultScene");}
	}


}
