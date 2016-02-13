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
		float acc = ( fantastic * 100 + great * 80 + good * 50 + miss * 0 ) / allnotes;
		return acc;
	}

	private string getRank(){
		float acc = getAccuracy ();
		if (acc < 50.0) {
			return "F";
		} else if (acc < 60.0) {
			return "D";
		} else if (acc < 70.0) {
			return "C";
		} else if (acc < 80.0) {
			return "B";
		} else if (acc < 90.0) {
			return "A";
		} else if (acc < 95.0) {
			return "S";
		} else if (acc < 99.0) {
			return "SS";
		} else {
			return "SSS";
		}
	}


}
