using UnityEngine;
using System.Collections.Generic;

public class LineFlickChecker : MonoBehaviour {

	public enum LaneFlickState {NONE, Flick} //0, 1

	public Transform flickChecker;
	public ParticleSystem hitParticle;

	public int laneNumber = 4;
	public string key = "z";
	private Vector3 firstPosition;
	private JudgeScript.Judge judge;
	private float score;
	private List<NoteDescription> laneFlickNotes;

	public LaneFlickState laneState = LaneFlickState.NONE;

	void Start () {
		Vector3 pos = flickChecker.transform.localPosition;
		firstPosition = new Vector3 (pos.x, pos.y, pos.z);
	}
		
	void Update () {
		if (key == "z") {
			Debug.Log (Input.GetKeyDown(key));
		}

		Vector3 pos = flickChecker.transform.position;
		if (Input.GetKeyDown(key)) {
			//Debug.Log (key);
			Vector3 hidePosition = new Vector3 (firstPosition.x, firstPosition.y + 3.2f, 0);
			flickChecker.transform.localPosition = hidePosition;

			laneState = LaneFlickState.Flick;
		}
		else if(!Input.GetKeyDown(key) || Input.GetKeyUp (key)){
			Vector3 hidePosition = new Vector3 (firstPosition.x, firstPosition.y - 3.2f, 0);
			flickChecker.transform.localPosition = hidePosition;
			laneState = LaneFlickState.NONE;
		}
		CheckFlick ();
	}

	private float CalculateFlickPercentage (float hitDeltaTime, NoteDescription note){
		//250 max
		judge = JudgeScript.Judge.MISS;

		if (note.NoteState == NoteDescription.NoteHitState.READY) {
			if (hitDeltaTime < 170) { // Fantastic
				judge = JudgeScript.Judge.FANTASTIC;
			} else if (hitDeltaTime < 210) { // Good
				judge = JudgeScript.Judge.GREAT;
			} else { // Bad
				judge = JudgeScript.Judge.GOOD;
			}
			JudgeScript.Instance.ApplyJudge (judge);
			note.NoteState = NoteDescription.NoteHitState.HIT;
		}
		return (int)judge;
	}

	public void CheckFlick(){
		if (laneState == LaneFlickState.Flick) {
			float hitTime = TimerScript.timePass;
			laneFlickNotes = NoteRenderer.allnotes [laneNumber];

			for (int i = 0; i < laneFlickNotes.Count; i++) {
				NoteDescription note = laneFlickNotes [i];
				float hitDeltaTime = GetHitDeltaTime (hitTime, note.HitTime);

				if (InRange (hitDeltaTime)) {
					if (note.NoteState == NoteDescription.NoteHitState.READY) {
						score = CalculateFlickPercentage (hitDeltaTime, note);
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
		}
	}

	private bool InRange(float hitDeltaTime){
		return hitDeltaTime <= 250;
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
		laneFlickNotes.Remove (note);
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

