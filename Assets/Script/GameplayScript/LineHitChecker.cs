using UnityEngine;
using System.Collections.Generic;

public class LineHitChecker : MonoBehaviour {

	public enum LaneHitState {NONE, HIT, HOLD} //0, 1, 2

	public Transform lineChecker;
	public ParticleSystem hitParticle;

	public int laneNumber = 0;
	public string key = "q";
	private Vector3 firstPosition;
	private JudgeScript.Judge judge;
	private float score;
	private List<NoteDescription> laneNotes;

	public LaneHitState laneState = LaneHitState.NONE;

	void Start () {
		Vector3 pos = lineChecker.transform.localPosition;
		firstPosition = new Vector3 (pos.x, pos.y, pos.z);
		Vector3 scale = lineChecker.transform.localScale;
	}

	// Update is called once per frame
	void Update () {

		Vector3 pos = lineChecker.transform.position;
		if (Input.GetKeyDown(key)) {
			//Debug.Log (key);
			Vector3 hidePosition = new Vector3 (firstPosition.x, firstPosition.y + 2.7f, 0);
			lineChecker.transform.localPosition = hidePosition;

			laneState = LaneHitState.HIT;
		}
		else if(Input.GetKeyUp (key)){
			Vector3 hidePosition = new Vector3 (firstPosition.x, firstPosition.y - 2.7f, 0);
			lineChecker.transform.localPosition = hidePosition;
			laneState = LaneHitState.NONE;
		}
			
		CheckPress ();

		if (Input.GetKey(key)) {
			laneState = LaneHitState.HOLD;
		}
	}

	private float CalculatePercentage (float hitDeltaTime, NoteDescription note){
		//160 max
		judge = JudgeScript.Judge.MISS;
		if (note.NoteState == NoteDescription.NoteHitState.READY) {
			if (hitDeltaTime < 80) { // Fantastic
				judge = JudgeScript.Judge.FANTASTIC;
			} else if (hitDeltaTime < 130) { // Good
				judge = JudgeScript.Judge.GREAT;
			} else { // Bad
				judge = JudgeScript.Judge.GOOD;
			}
			JudgeScript.Instance.ApplyJudge (judge);
			note.NoteState = NoteDescription.NoteHitState.HIT;
		}
		return (int)judge;
	}

	public void CheckPress(){
		if (laneState == LaneHitState.HIT) {
			float hitTime = TimerScript.timePass;
			laneNotes = NoteRenderer.allnotes [laneNumber];

			for (int i = 0; i < laneNotes.Count; i++) {
				NoteDescription note = laneNotes [i];
				float hitDeltaTime = GetHitDeltaTime (hitTime, note.HitTime);
			
				if (InRange (hitDeltaTime)) {
					if (note.NoteState == NoteDescription.NoteHitState.READY) {
						score = CalculatePercentage (hitDeltaTime, note);
						if (note.Length == 0) {
							DestroyNote (note);
						}
						ApplyHit ();
						JudgeScript.Instance.StoreJudge (judge);
						break;
					}
				} else {
					break;
				}
				note.NoteState = NoteDescription.NoteHitState.HIT;
			}
		} else if (laneState == LaneHitState.HOLD) {
			
			laneNotes = NoteRenderer.allnotes [laneNumber];

			for (int i = 0; i < laneNotes.Count; i++) {
				NoteDescription note = laneNotes [i];

				if (note.NoteState != NoteDescription.NoteHitState.MISSED) {
					if (HoldInRange (note)) {

						for (int j = 0 ; j < note.EachComboTime.Length; j++) {
							if (!note.EachComboAdded [j]){
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
	}

	public void Release(){
		laneState = LaneHitState.NONE;
		score = 0;
	}

	private bool InRange(float hitDeltaTime){
		return hitDeltaTime <= 160;
	}

	private bool HoldInRange(NoteDescription note){
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
		laneNotes.Remove (note);
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
}
