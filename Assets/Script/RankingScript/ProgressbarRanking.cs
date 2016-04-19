using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Leap;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

	public class ProgressbarRanking : MonoBehaviour {

	public AudioSource confirm;
	public AudioSource selectB;

	public UnityEngine.UI.Image loadingPicture;
//	public GameObject particle;
	public GameObject leapCamera;

	private bool next = false;
	private float delay = 0;
	private string str = "SongSelection";

	private Leap.Controller controller;
	public Transform Done_LoadingBar;
	public Transform Done_LoadingBar_Background;
	private float timeDelay;

	public float doneButton_Amount;

	[SerializeField] private float progressBarSpeed;

	// Use this for initialization
	void Start () {
		controller = new Leap.Controller ();
//		particle.active = true;
		leapCamera.active = true;
		Done_LoadingBar.gameObject.SetActive (true);
		Done_LoadingBar_Background.gameObject.SetActive (true);


		next = false;
		delay = 0;

	}

	// Update is called once per frame
	void Update () {
		Frame frame = controller.Frame ();
		HandList hands = frame.Hands;
		Hand hand = hands.Rightmost;
		FillDoneProgressBar (hand);
		if (next) {
			delay += Time.deltaTime;
		}

		if (delay >= 0.4f) {
//			particle.active = false;
			leapCamera.active = false;
			loadingPicture.gameObject.active = true;
			UnityEngine.Application.LoadLevel (str);
		}
	}

	private void FillDoneProgressBar(Hand hand){

		if (doneButton_Amount >= 100) {
			if (!confirm.isPlaying) {
				confirm.Play ();
			}
			next = true;

		} else if (hand.GrabStrength == 1 || Input.GetKey(KeyCode.Space)) {
			doneButton_Amount += progressBarSpeed * Time.deltaTime;
		} else {
			if(doneButton_Amount > 0)
				doneButton_Amount -= progressBarSpeed * Time.deltaTime*2;
		}
		Done_LoadingBar.GetComponent<UnityEngine.UI.Image> ().fillAmount = doneButton_Amount / 100;
	}
		
}
