﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Leap;

public class StartScript : MonoBehaviour {

	public AudioSource source;

	public Text startText;
	private static float delay = 0.5f;
	private float timer = 0;
	private bool blinkUp = false;
	private float opac = 1.0f;
	private float opacRate = 0.06f;

	private bool goingNext = false;
	private float sceneTimer = 0;

	public UnityEngine.UI.Image loadingPicture;
	public Transform particalFolder;
	public Transform audioSpectrum;
	public Transform uiCamera;

	Leap.Controller controller;
	// Use this for initialization
	void Start () {
		loadingPicture.rectTransform.localPosition = new Vector3(5000,5000);
		Application.runInBackground = true;
		controller = new Leap.Controller ();
		controller.EnableGesture (Gesture.GestureType.TYPESWIPE);
	}

	// Update is called once per frame
	void Update () {
		Blink ();

		Leap.Frame frame = controller.Frame ();
		GestureList gestures = frame.Gestures();

		for (int i = 0; i < gestures.Count; i++) {
			if (gestures [i].Type == Gesture.GestureType.TYPE_SWIPE) {
				if(!source.isPlaying && ! goingNext){
				goingNext = true;
					source.Play ();
				}
				delay = 0.1f;
				opacRate = 0.65f;
				startText.color = new Color (startText.color.r, startText.color.g, startText.color.b, opac);
			}
		}

		if(goingNext){
			sceneTimer += Time.deltaTime;
		}

		if (sceneTimer > 1.7f) {
			StartCoroutine(ChangeScene());
		}

		if (Input.GetKeyDown ("space")) {
			if(!source.isPlaying && ! goingNext){
				goingNext = true;
				source.Play ();
			}
			delay = 0.1f;
			opacRate = 0.65f;
			startText.color = new Color (startText.color.r, startText.color.g, startText.color.b, opac);

		}
	}

	void Blink(){
		//startText.enabled = blinkUp;
		if (blinkUp) {
			opac += opacRate;
		} else {
			opac -= opacRate;
		}
		startText.color = new Color (startText.color.r, startText.color.g, startText.color.b, opac);
		timer += Time.deltaTime;

		if (timer >= delay) {
			timer = 0;
			blinkUp = !blinkUp;
		}
	}

	IEnumerator ChangeScene() {
		particalFolder.gameObject.SetActive (false);
		audioSpectrum.gameObject.SetActive (false);
		uiCamera.gameObject.SetActive (false);
		loadingPicture.rectTransform.localPosition = new Vector3(0,0);
		source.Play ();
		//yield return new WaitForSeconds(2);


		// Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
		AsyncOperation async = Application.LoadLevelAsync("SongSelection");

		// While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
		while (!async.isDone) {
			yield return null;
		}
	}

//	public void ChangeScene(){
//		SceneManager.LoadSceneAsync("SongSelection");
//	}

	float PercentWidth(float width){
		return (width / 100) * UnityEngine.Screen.width;
	}

	float PercentHeight(float height){
		return (height / 100) * UnityEngine.Screen.height;
	}


}