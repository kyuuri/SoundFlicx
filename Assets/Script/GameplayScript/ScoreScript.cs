using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {

	public Text scoreText;
	public static ScoreScript Instance { get; private set;}

	private float score = 0;

	void Awake(){
		Instance = this;
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
		if (score > GlobalData.result.score) {
			GlobalData.result.score = score;
		}
	}

	public void addScore(float sc){
		score += sc;
		scoreText.text = score + "";

		if (score > GlobalData.result.score) {
			GlobalData.result.score = score;
		}
	}

}
