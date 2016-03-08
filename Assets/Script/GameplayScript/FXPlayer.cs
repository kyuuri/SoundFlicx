using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class FXPlayer : MonoBehaviour {

//	public AudioMixerSnapshot noFX;
//	public AudioMixerSnapshot FX;
//
//	public static AudioMixerSnapshot Default;
//	public static AudioMixerSnapshot Flange;
//	public static AudioHighPassFilter highPass;
//	public static AudioLowPassFilter lowPass;
//
//	public static bool filterGoingDown = false;
//
//	public AudioSource fxSource;
//
//	public float offset;
//	private bool isPlayed = false;
//
//	public void playAudio(){
//		fxSource.PlayOneShot (fxSource.clip);
//	}
//
//	void Start(){
//		highPass = GetComponent<AudioHighPassFilter> ();
//		lowPass = GetComponent<AudioLowPassFilter> ();
//
//		fxSource = GetComponent<AudioSource> ();
//
//		Default = noFX;
//		Flange = FX;
//
//		offset = GlobalData.selectedTrack.offset;
//		string songName = GlobalData.selectedTrack.songName;
//
//		fxSource .clip = Resources.Load (songName + "Audio.mp3") as AudioClip;
//	}
//
//	void Update () {
//
//		if (!isPlayed && TimerScript.timePass >= (0 + offset)) {
//			playAudio ();
//			isPlayed = true;
//		}
//
//		LowerFilter ();
//	}
//
//	public static void SetHighCutOff(float value){
//		keepHighInRange (ref value);
//		highPass.cutoffFrequency = value;
//	}
//
//	public static void SetLowCutOff(float value){
//		keepHighInRange (ref value);
//		lowPass.cutoffFrequency = value;
//	}
//
//	private static void keepLowInRange(ref float value){
//		if (value > 22000) {
//			value = 22000;
//		}
//	}
//
//	private static void keepHighInRange(ref float value){
//		if (value < 10) {
//			Debug.Log ("fuck: " + value);
//			value = 10;
//		}
//	}
//
//	private static void LowerFilter(){
//		filterGoingDown = LineLTiltChecker.filterLGoingDown && LineRTiltChecker.filterRGoingDown;
//		if (filterGoingDown) {
//
//			/*
//			if (highPass.cutoffFrequency > 200) {
//				float value = highPass.cutoffFrequency - 100;
//				SetHighCutOff (value);
//			}
//			else if (highPass.cutoffFrequency > 10) {
//				float value = highPass.cutoffFrequency - 9;
//				SetHighCutOff (value);
//			}
//			*/
//
//
//
//			if (filterGoingDown) {
//				if (lowPass.cutoffFrequency < 15000) {
//					float value = lowPass.cutoffFrequency + 1000;
//					SetLowCutOff (value);
//				} else if (lowPass.cutoffFrequency < 20000) {
//					float value = lowPass.cutoffFrequency + 500;
//					SetLowCutOff (value);
//				}
//			}
//
//
//		}
//	}

}

