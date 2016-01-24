using UnityEngine;
using System.Collections;

public class ScoreScript : MonoBehaviour {

	private static float score = 0;

	void Start () {
		score = 0;
	}


	public static float getScore(){
		return score;
	}

	public static void setScore(float sc){
		score = sc;
	}

	public static void addScore(float sc){
		score += sc;
	}

}
