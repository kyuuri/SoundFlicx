﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class JudgeScript : MonoBehaviour {

	public enum Judge {FANTASTIC = 100, GREAT = 50, GOOD = 10, MISS = 0}

	public ResultSubmitter resultSubmitter;
	public ComboScript comboScript;

	public Text judgeText;
	//public static JudgeScript Instance { get; private set;}

	private Vector3 pos;
	private bool isJumping = false;
	private int count = 0;
	private int limit = 4;

	void Awake(){
		//Instance = this;
	}

	void Start () {
		judgeText.text = "";
		isJumping = false;
		pos = judgeText.transform.position;
	}

	void Update(){
		if (isJumping) {
			Jump ();
		}
	}

	public void ApplyJudge(Judge judge){
		judgeText.transform.position = new Vector3(pos.x, pos.y, judgeText.transform.position.z);
		judgeText.text = judge+"";
		if (judge == Judge.FANTASTIC) {
			judgeText.color = Color.Lerp(Color.white, Color.yellow, 0.4f);
		}
		else if(judge == Judge.GREAT){
			judgeText.color = Color.Lerp(Color.white, Color.green, 0.4f);
		}
		else if(judge == Judge.GOOD){
			judgeText.color = Color.Lerp(Color.white, Color.blue, 0.5f);
		}
		else if(judge == Judge.MISS){
			judgeText.color = Color.white;
		}
		if (judge == Judge.MISS) {
			comboScript.MissCombo ();
		}
		count = 0;
		isJumping = true;
	}

	public void StoreJudge (Judge judge){
		resultSubmitter.SubmitJudge (judge);
	}

	private void Jump(){
		if (count < limit) {
			judgeText.transform.position = judgeText.transform.position + Vector3.up;
			++count;
		} else if (count == limit) {
			isJumping = false;
			count = 0;
		}
	}
}
