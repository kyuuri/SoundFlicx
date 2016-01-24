using UnityEngine;
using System.Collections.Generic;

public class LineHitChecker : MonoBehaviour {

	enum LaneState {none, hit, hold} //0, 1, 2

	public int laneNumber = 0;
	public string key = "q";
	public Transform lineChecker;
	private Vector3 firstPosition;
	private float score;
	private List<NoteDescription> laneNotes;

	private LaneState laneState = LaneState.none;

	void Start () {
		Vector3 pos = lineChecker.transform.localPosition;
		firstPosition = new Vector3 (pos.x, pos.y, pos.z);
		Vector3 scale = lineChecker.transform.localScale;
	}

	// Update is called once per frame
	void Update () {

		Vector3 pos = lineChecker.transform.position;

		if (Input.GetKeyDown (key)) {
			//Debug.Log (key);
			Vector3 hidePosition = new Vector3 (firstPosition.x, firstPosition.y + 2.7f, 0);
			lineChecker.transform.localPosition = hidePosition;
			Press ();
		}
		else if(Input.GetKeyUp (key)){
			Vector3 hidePosition = new Vector3 (firstPosition.x, firstPosition.y - 2.7f, 0);
			lineChecker.transform.localPosition = hidePosition;
		}
	}

	private float CalculatePercentage (float hitDeltaTime, NoteDescription note){
		//120 max
		float score = 0;
		if (note.NoteState == 0) {
			if (hitDeltaTime < 60) { // Fantastic
				score = 100;
			} else if (hitDeltaTime < 100) { // Good
				score = 50;
			} else { // Bad
				score = 10;
			}
			note.NoteState = 1;
		}
		Debug.Log (score);
		return score;
	}

	public void Press(){
		if (laneState == LaneState.none) {
			float hitTime = TimerScript.timePass;
			laneNotes = NoteRenderer.allnotes[laneNumber];

			for (int i = 0; i < laneNotes.Count ; i++) {
				NoteDescription note = laneNotes [i];
				float hitDeltaTime = GetHitDeltaTime (hitTime, note.HitTime);
				if (InRange (hitDeltaTime)) {
					if (note.NoteState == 0) {
						CalculatePercentage (hitDeltaTime, note);
						DestroyNote (note);
						break;
					}
				} else {
					break;
				}
			}
		}
	}

	public bool InRange(float hitDeltaTime){
		return hitDeltaTime <= 120;
	}

	public float GetHitDeltaTime(float hitTime, float noteTime){
		float hitDeltaTime = noteTime - hitTime;

		if (hitDeltaTime < 0) { // make sure it is positive
			hitDeltaTime = -hitDeltaTime;
		}
		hitDeltaTime *= 1000;
		return hitDeltaTime;
	}

	public void DestroyNote(NoteDescription note){
		laneNotes.Remove (note);
		note.DestroySelf ();
		note = null;
	}
}
