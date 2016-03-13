using UnityEngine;
using System.Collections.Generic;
using Leap;

public class LineLTiltChecker : MonoBehaviour {

	public enum LaneTiltState {IDLE, L2RTILT, L2LTILT} //0, 1, 2

	public GameObject lTilt;
	private Leap.Controller controller;

	public Transform lineChecker;
	public Transform particleObj;
	public Transform otherParticleObj;
	public ParticleSystem hitParticle;

	private Vector3 parInitPos;
	private Vector3 otherParInitPos;

	//public int laneNumber = 0;
	public string key2R = "b";
	public string key2L = "v";
	private Vector3 firstPosition;

	private JudgeScript.Judge judge;
	private float score;
	private List<NoteDescription> notes;

	public LaneTiltState laneState = LaneTiltState.IDLE;

	public static bool filterLGoingDown = false;

	private bool isHit;
	private bool move;

	void Start () {
		parInitPos = particleObj.transform.position;
		otherParInitPos = otherParticleObj.transform.position;

		controller = new Leap.Controller ();
	}

	// Update is called once per frame
	void Update () {

		//Vector3 pos = lineChecker.transform.position;
		if (Input.GetKey(key2R)) {
			//Vector3 hidePosition = new Vector3 (firstPosition.x, firstPosition.y + 2.7f, 0);
			//lineChecker.transform.localPosition = hidePosition;

			laneState = LaneTiltState.L2RTILT;
		}
		else if(Input.GetKey(key2L)){
			//Vector3 hidePosition = new Vector3 (firstPosition.x, firstPosition.y - 2.7f, 0);
			//lineChecker.transform.localPosition = hidePosition;
			laneState = LaneTiltState.L2LTILT;
		}

		notes = NoteRenderer.leftTiltNotes [0];

		UpdateTiltVisual ();
		CheckTilt ();
		CheckTimeTilt ();

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

			isHit = false;

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

					if (note.TiltAngle < 0) {
						x = -x;
					}
					particleObj.transform.position = new Vector3 (x/1.08f, particleObj.transform.position.y, particleObj.transform.position.z);
					lTilt.transform.position = new Vector3 (x/1.08f, lTilt.transform.position.y, lTilt.transform.position.z);

					isHit = true;
					//PlayEffect (true, 9999);
				}
			}

			if (laneState == LaneTiltState.L2RTILT && note.TiltAngle > 0.1f) {
				isHit = true;
			}
			else if (laneState == LaneTiltState.L2LTILT && note.TiltAngle < -0.1f) {
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
			} else {
				//PlayEffect (false, 0);
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

			if (note.TiltAngle < 0) {
				x = -x;
			}

			bool move = false;
			if (note.TiltAngle > 0.1f && laneState == LaneTiltState.L2RTILT) {
				move = true;
			}
			else if(note.TiltAngle < 0.1f && laneState == LaneTiltState.L2LTILT){
				move = true;
			}

			if (move) {
				particleObj.transform.position = new Vector3 (x, particleObj.transform.position.y, particleObj.transform.position.z);
				lTilt.transform.position = new Vector3 (x, lTilt.transform.position.y, lTilt.transform.position.z);

				//PlayEffect (true, x);
			}

		} else if (note.TiltAngle >= -0.1f && note.TiltAngle <= 0.1f){
			particleObj.transform.position = new Vector3 (note.NoteObject.transform.position.x, particleObj.transform.position.y, particleObj.transform.position.z);
			lTilt.transform.position = new Vector3 (note.NoteObject.transform.position.x, lTilt.transform.position.y, lTilt.transform.position.z);

			//PlayEffect (true, note.NoteObject.transform.position.x);
		}
	}

	private bool IsParticleInRange(float x){
		return x <= parInitPos.x && x >= otherParInitPos.x;
	}

	void UpdateTiltVisual () {
		Frame frame = controller.Frame ();
		HandList hands = frame.Hands;
		Hand lHand = hands.Rightmost;

		if (lHand.IsLeft) {
			Vector3 lTiltPos = new Vector3 (0, lTilt.transform.position.y, lTilt.transform.position.z);

			float lRoll = ToDegrees (lHand.PalmNormal.Roll);

			float lPos = GetLeftXPos (lRoll);

//			if (!isHit) {
//				if (lPos > parInitPos.x && lPos < otherParInitPos.x) {
//					lTilt.transform.position = new Vector3 (lPos, lTiltPos.y, lTiltPos.z);
//				}
//			}
			if (lPos < parInitPos.x)
				lPos = parInitPos.x;
			else if (lPos > otherParInitPos.x)
				lPos = otherParInitPos.x;

			lTilt.transform.position = new Vector3 (lPos, lTiltPos.y, lTiltPos.z);

		}
	}

	float ToDegrees (float Radian) {
		float Degrees;
		Degrees = Radian * 180 / Mathf.PI;
		return Degrees;
	}

	float GetLeftXPos (float degrees) {
		float maxDegrees = 45;
		return  -( ((parInitPos.x - otherParInitPos.x) / maxDegrees) * (-degrees - 45) + parInitPos.x  );
				//-( ((parInitPos.x - otherParInitPos.x) / maxDegrees) * (degrees - 0) - parInitPos.x  );
	}

	void CheckTimeTilt(){
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
			if (note.HitTime - TimerScript.timePass < 1) {
				if (!lTilt.activeSelf) {
					lTilt.SetActive (true);
				}
			} else {
				if(lTilt.activeSelf){
					lTilt.SetActive (false);
				}
			}
		} else {
			if(lTilt.activeSelf){
				lTilt.SetActive (false);
			}
		}
	}

	/*
	private void PlayEffect(bool flange, float value){
		if (flange) {
			SoundPlayer.Flange.TransitionTo (0.1f);
			if (value != 9999) {
				value += 1.475f;
				filterGoingDown = false;
				SoundPlayer.SetLowCutOff (22000 - value * 7000);
			}

		} else {
			SoundPlayer.Default.TransitionTo (0.1f);
			filterGoingDown = true;
		}
	}

	private void LowerFilter(){
		if (filterGoingDown) {
			if (SoundPlayer.lowPass.cutoffFrequency < 22000) {
				float value = SoundPlayer.lowPass.cutoffFrequency + 200;
				SoundPlayer.SetLowCutOff (value);
			}
		}
	}
	*/

//	private void PlayEffect(bool flange, float value){
//		
//		if (flange) {
//			FXPlayer.Flange.TransitionTo (0.1f);
//			if (value != 9999) {
//				value += 1.50f;
//				filterLGoingDown = false;
//
//				//value = value * 450;
//				//FXPlayer.SetHighCutOff (value);
//
//				//value = 22000 - value * 6000;
//				//SoundPlayer.SetLowCutOff (value);
//			}
//
//		} else {
//			//Debug.Log ("false");
//			//FXPlayer.Default.TransitionTo (0.3f);
//			filterLGoingDown = true;
//		
//	}
}
