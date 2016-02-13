using UnityEngine;
using System.Collections;

public class ResultScore{

	public float score;

	public int fantastic;
	public int great;
	public int good;
	public int miss;

	public int maxCombo;

	public float getAccuracy(){
		int allnotes = fantastic + great + good + miss;
		float acc = ( fantastic * 100 + great * 80 + good * 50 + miss * 0 ) * 1.0f / allnotes;
		return acc;
	}

	public string getRank(){
		float acc = getAccuracy ();
		if (acc < 50.0f) {
			return "F";
		} else if (acc < 60.0f) {
			return "D";
		} else if (acc < 70.0f) {
			return "C";
		} else if (acc < 80.0f) {
			return "B";
		} else if (acc < 90.0f) {
			return "A";
		} else if (acc < 95.0f) {
			return "S";
		} else if (acc < 99.0f) {
			return "SS";
		} else {
			return "SSS";
		}
	}


}
