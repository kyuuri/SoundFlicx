using UnityEngine;
using System.Collections;

public class ResultSubmitter : MonoBehaviour {

	public int playerNumber = 1;

	public void SubmitJudge(JudgeScript.Judge judge){
		if (playerNumber == 1) {
			if (judge == JudgeScript.Judge.FANTASTIC) {
				GlobalData.result.fantastic += 1;
			} else if (judge == JudgeScript.Judge.GREAT) {
				GlobalData.result.great += 1;
			} else if (judge == JudgeScript.Judge.GOOD) {
				GlobalData.result.good += 1;
			} else if (judge == JudgeScript.Judge.MISS) {
				GlobalData.result.miss += 1;
			}
		} else {
			if (judge == JudgeScript.Judge.FANTASTIC) {
				GlobalData.result2.fantastic += 1;
			} else if (judge == JudgeScript.Judge.GREAT) {
				GlobalData.result2.great += 1;
			} else if (judge == JudgeScript.Judge.GOOD) {
				GlobalData.result2.good += 1;
			} else if (judge == JudgeScript.Judge.MISS) {
				GlobalData.result2.miss += 1;
			}
		}
	}

	public void SubmitScore(float score){
		if (playerNumber == 1) {
			if (score > GlobalData.result.score) {
				GlobalData.result.score = score;
			}
		} else {
			if (score > GlobalData.result2.score) {
				GlobalData.result2.score = score;
			}
		}
	}

	public void SubmitCombo(int combo){
		if (playerNumber == 1) {
			if (combo > GlobalData.result.maxCombo) {
				GlobalData.result.maxCombo = combo;
			}
		} else {
			if (combo > GlobalData.result2.maxCombo) {
				GlobalData.result2.maxCombo = combo;
			}
		}
	}
}
