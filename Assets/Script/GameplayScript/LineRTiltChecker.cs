﻿using UnityEngine;
using System.Collections.Generic;

public class LineRTiltChecker : MonoBehaviour {

	public enum LaneTiltState {IDLE, R2RTILT, R2LTILT} //0, 1, 2

	public Transform lineChecker;
	public Transform particleObj;
	public Transform otherParticleObj;
	public ParticleSystem hitParticle;

	private Vector3 parInitPos;
	private Vector3 otherParInitPos;

	//public int laneNumber = 0;
	public string key2R = "m";
	public string key2L = "n";
	private Vector3 firstPosition;

	private JudgeScript.Judge judge;
	private float score;
	private List<NoteDescription> notes;

	public LaneTiltState laneState = LaneTiltState.IDLE;

	private Color[] colors;
	private Color bc = new Color(31, 255, 173, 1.0f);

	void Start () {
		colors = new Color[2];
		parInitPos = particleObj.transform.position;
		otherParInitPos = otherParticleObj.transform.position;

		Material[] m = NoteRenderer.rightTiltNotes [0] [0].NoteObject.GetComponent<Renderer> ().materials;
		for(int i = 0 ; i < m.Length ; i++){
			colors [i] = m [i].GetColor ("_TintColor");
		}
	}

	// Update is called once per frame
	void Update () {
		//Vector3 pos = lineChecker.transform.position;
		if (Input.GetKey(key2R)) {
			//Vector3 hidePosition = new Vector3 (firstPosition.x, firstPosition.y + 2.7f, 0);
			//lineChecker.transform.localPosition = hidePosition;

			laneState = LaneTiltState.R2RTILT;
		}
		else if(Input.GetKey(key2L)){
			//Vector3 hidePosition = new Vector3 (firstPosition.x, firstPosition.y - 2.7f, 0);
			//lineChecker.transform.localPosition = hidePosition;
			laneState = LaneTiltState.R2LTILT;
		}

		CheckTilt ();

		if (Input.GetKeyUp(key2L) || Input.GetKeyUp(key2R)) {
			laneState = LaneTiltState.IDLE;
		}
	}

	private float CalculatePercentage (float hitDeltaTime, NoteDescription note){
		//160 max
		judge = JudgeScript.Judge.MISS;
		if (note.NoteState == NoteDescription.NoteHitState.READY) {
			if (hitDeltaTime < 160) { // Fantastic
				judge = JudgeScript.Judge.FANTASTIC;
			}
			JudgeScript.Instance.ApplyJudge (judge);
			note.NoteState = NoteDescription.NoteHitState.HIT;
		}
		return (int)judge;
	}

	public void CheckTilt(){
		notes = NoteRenderer.rightTiltNotes [0];

		if (notes.Count > 0) {
			NoteDescription note = null;
			for (int i = 0; i < notes.Count; i++) {
				note = notes [i];
				if (note.Length > 0) {
					if (TimerScript.timePass > note.HitTime + note.Length - 0.02f) {
						if (i + 2 < notes.Count - 1) {
							note = notes [i + 2];
						}
					}
					break;
				}
			}
			MovePaticle (note);

			float autoTime = TimerScript.timePass;
			float hitDeltaTime = GetHitDeltaTime (autoTime, note.HitTime);

			if (InRange (hitDeltaTime)) {
				if (note.NoteState == NoteDescription.NoteHitState.READY) {
					score = CalculatePercentage (hitDeltaTime, note);
					ApplyHit ();
					JudgeScript.Instance.StoreJudge (judge);
					note.NoteState = NoteDescription.NoteHitState.HIT;
					//free hit

					float currentTime = TimerScript.timePass;
					float hitTime = note.HitTime;
					float length = note.Length;
					float diffPos = otherParInitPos.x - parInitPos.x;

					float x;
					// pos = (del pos / del time) * (@time-time1 ) + pos1
					x = (diffPos / length) * (currentTime - hitTime) + parInitPos.x;

					if (note.TiltAngle >= 0) {
						x = -x;
					}
					particleObj.transform.position = new Vector3 (x/1.08f, particleObj.transform.position.y, particleObj.transform.position.z);
				}
			}
			bool isHit = false;

			if (laneState == LaneTiltState.R2RTILT && note.TiltAngle > 0.1f) {
				isHit = true;
			}
			else if (laneState == LaneTiltState.R2LTILT && note.TiltAngle < -0.1f) {
				isHit = true;
			}
			else if (laneState == LaneTiltState.IDLE && note.TiltAngle >= -0.1f && note.TiltAngle <= 0.1f) {
				isHit = true;
			}

			if (isHit) {
				note.NoteState = NoteDescription.NoteHitState.HIT;
				judge = JudgeScript.Judge.FANTASTIC;
				score = (int)judge;
				if (TiltInRange (note)) {			
					for (int j = 0; j < note.EachComboTime.Length; j++) {
						if (!note.EachComboAdded [j]) {
							if (TimerScript.timePass >= note.EachComboTime [j]) {
								ApplyHit ();
								note.EachComboAdded [j] = true;
							}
						}
					}
				}
			}

		}
	}

	private bool InRange(float hitDeltaTime){
		return hitDeltaTime <= 90;
	}

	private bool TiltInRange(NoteDescription note){
		return note.NoteState == NoteDescription.NoteHitState.HIT && note.HitTime < TimerScript.timePass && TimerScript.timePass < note.HitTime + note.Length;
	}

	private float GetHitDeltaTime(float hitTime, float noteTime){
		float hitDeltaTime = noteTime - hitTime;

		if (hitDeltaTime < 0) { // make sure it is positive
			hitDeltaTime = -hitDeltaTime;
		}
		hitDeltaTime *= 1000;
		return hitDeltaTime;
	}

	private void DestroyNote(NoteDescription note){
		notes.Remove (note);
		note.DestroySelf ();
		note = null;
	}

	private void ApplyScore (float score){
		if (score != 0) {
			ScoreScript.Instance.addScore(score);
		}
	}

	private void ApplyJudge (JudgeScript.Judge judge){
		JudgeScript.Instance.ApplyJudge (judge);
	}

	private void ApplyCombo (){
		ComboScript.Instance.ApplyCombo (1);
	}

	private void ApplyHit(){
		hitParticle.Play ();
		ApplyScore (score);
		ApplyJudge (judge);
		ApplyCombo ();
	}

	private void MovePaticle (NoteDescription note){
		float currentTime = TimerScript.timePass;

		if (currentTime > note.HitTime + note.Length || currentTime < note.HitTime)
			return;
		
		float hitTime = note.HitTime;
		float length = note.Length;
		float diffPos = otherParInitPos.x - parInitPos.x;

		float x;

		if (note.TiltAngle < -0.1f || note.TiltAngle > 0.1f) {

			// pos = (del pos / del time) * (@time-time1 ) + pos1
			x = (diffPos / length) * (currentTime - hitTime) + parInitPos.x;

			if (note.TiltAngle > 0) {
				x = -x;
			}

			bool move = false;
			if (note.TiltAngle > 0.1f && laneState == LaneTiltState.R2RTILT) {
				move = true;
			} else if (note.TiltAngle < 0.1f && laneState == LaneTiltState.R2LTILT) {
				move = true;
			}
				
			if (move) {
				particleObj.transform.position = new Vector3 (x, particleObj.transform.position.y, particleObj.transform.position.z);
			}

		} else if (note.TiltAngle >= -0.1f && note.TiltAngle <= 0.1f){
			particleObj.transform.position = new Vector3 (note.NoteObject.transform.position.x, particleObj.transform.position.y, particleObj.transform.position.z);
		}
	}

	private bool IsParticleInRange(float x){
		return x <= parInitPos.x && x >= otherParInitPos.x;
	}
}
