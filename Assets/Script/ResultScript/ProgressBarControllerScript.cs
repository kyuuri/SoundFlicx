﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Leap;

public class ProgressBarControllerScript : MonoBehaviour {

	private Leap.Controller controller;
	public Transform Done_LoadingBar;
	public Transform Retry_LoadingBar;
	public Transform Done_LoadingBar_Background;
	public Transform Retry_LoadingBar_Background;
	private Status_State state;
	private float timeDelay;
	private bool isFlicking;
	private float flickDelay = 0.4f;
	[SerializeField] private float doneButton_Amount;
	[SerializeField] private float retryButton_Amount;
	[SerializeField] private float progressBarSpeed;

	private enum Status_State
	{
		DONE = 0,
		RETRY = 1
	};

	// Use this for initialization
	void Start () {
		controller = new Leap.Controller ();
		state = Status_State.DONE;
		Done_LoadingBar.gameObject.SetActive (true);
		Done_LoadingBar_Background.gameObject.SetActive (true);
		Retry_LoadingBar.gameObject.SetActive (false);
		Retry_LoadingBar_Background.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		Frame frame = controller.Frame ();
		HandList hands = frame.Hands;
		Hand hand = hands.Rightmost;

		if (!isFlicking) {
			CheckFlick (hands);
		} else if (isFlicking) {
			timeDelay += Time.deltaTime;
		}
		if (timeDelay >= flickDelay) {
			timeDelay = 0;
			isFlicking = false;
		}
		if (state == Status_State.DONE) {
			FillDoneProgressBar (hand);
		} else {
			FillRetryProgressBar (hand);
		}
	}

	private void FillDoneProgressBar(Hand hand){

		if (doneButton_Amount >= 100) {
			UnityEngine.Application.LoadLevel("SongSelection");
		} else if (hand.GrabStrength == 1	) {
			doneButton_Amount += progressBarSpeed * Time.deltaTime;
		} else {
			if(doneButton_Amount > 0)
				doneButton_Amount -= progressBarSpeed * Time.deltaTime*2;
		}
		Done_LoadingBar.GetComponent<UnityEngine.UI.Image> ().fillAmount = doneButton_Amount / 100;
	}

	private void FillRetryProgressBar(Hand hand){

		if (retryButton_Amount >= 100) {
			UnityEngine.Application.LoadLevel("Gameplay");
		} else if (hand.GrabStrength == 1) {
			retryButton_Amount += progressBarSpeed * Time.deltaTime;
		} else {
			if(retryButton_Amount > 0)
				retryButton_Amount -= progressBarSpeed * Time.deltaTime*2;
		}
		Retry_LoadingBar.GetComponent<UnityEngine.UI.Image> ().fillAmount = retryButton_Amount / 100;
	}

	private void CheckFlick(HandList hands){
		Hand rightHand = hands.Rightmost;
		Hand leftHand = hands.Leftmost;
		if (isSwipeRight(rightHand)) {
			Done_LoadingBar.gameObject.SetActive (true);
			Done_LoadingBar_Background.gameObject.SetActive (true);
			Retry_LoadingBar.gameObject.SetActive (false);
			Retry_LoadingBar_Background.gameObject.SetActive (false);
			state = Status_State.DONE;
			isFlicking = true;
		}
		if (isSwipeLeft(leftHand)) {
			Done_LoadingBar.gameObject.SetActive (false);
			Done_LoadingBar_Background.gameObject.SetActive (false);
			Retry_LoadingBar.gameObject.SetActive (true);
			Retry_LoadingBar_Background.gameObject.SetActive (true);
			state = Status_State.RETRY;
			isFlicking = true;
		}

	}

	private bool isSwipeRight(Hand hand){
		float speed = 120;
		float yaw = hand.Direction.Yaw * 5;
		return yaw > 1.2f && hand.PalmVelocity.x > speed;
	}

	private bool isSwipeLeft(Hand hand){
		float speed = 120;
		float yaw = hand.Direction.Yaw * 5 * -1;
		return yaw > 1.2f && hand.PalmVelocity.x < -speed;
	}
}