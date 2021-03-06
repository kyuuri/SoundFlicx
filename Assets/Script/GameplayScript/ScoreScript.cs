﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {

	public Text scoreText;
	//public static ScoreScript Instance { get; private set;}

	public ResultSubmitter resultSubmitter;

	private float score = 0;

	void Awake(){
		//Instance = this;
	}

	void Start () {
		score = 0;
		scoreText.text = score + "";
	}


	public float getScore(){
		return score;
	}

	public void setScore(float sc){
		score = sc;
		scoreText.text = score + "";
		resultSubmitter.SubmitScore (sc);
	}

	public void addScore(float sc){
		score += sc;
		scoreText.text = score + "";

		resultSubmitter.SubmitScore (score);
	}

}
