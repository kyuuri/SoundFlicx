using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour {

	public AudioSource mySource;
	 
	public void playAudio(AudioClip x){
		mySource = GetComponent<AudioSource> ();
		mySource.PlayOneShot(x);
	}

	public void pause(){
		mySource.Pause ();
	}

	public void continueMusic(){
		mySource.UnPause ();
	}

//	void Awake() {
//		
//		DontDestroyOnLoad(transform.gameObject);
//	
//	}

}
