using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoteDestroyer : MonoBehaviour {

	public LineHitChecker[] lineCheckers = new LineHitChecker[4];
	public List<NoteDescription>[] allnotes;
	private List<NoteDescription> laneNotes;
	private List<NoteDescription>[] rightTiltNotes;
	private List<NoteDescription>[] leftTiltNotes;


	void Update () {
		allnotes = NoteRenderer.allnotes;
		rightTiltNotes = NoteRenderer.rightTiltNotes;
		leftTiltNotes = NoteRenderer.leftTiltNotes;

		for (int i = 0; i < allnotes.Length; i++) {
			laneNotes = NoteRenderer.allnotes[i];
			if (laneNotes.Count > 0) {
				NoteDescription note = laneNotes [0];
				float deltaTime = GetDeltaTime (note.HitTime);

				if (OutRange (deltaTime)) {
					if (note.NoteState == NoteDescription.NoteHitState.READY) {
						note.NoteState = NoteDescription.NoteHitState.MISSED;
						JudgeScript.Instance.ApplyJudge (JudgeScript.Judge.MISS);
						JudgeScript.Instance.StoreJudge (JudgeScript.Judge.MISS);
					}
					if (note.Length > 0 && note.NoteState != NoteDescription.NoteHitState.MISSED) {
						if(CheckReleaseLongNoteEndPoint(note)){
							//nothing
						}
						else if(lineCheckers[i].laneState != LineHitChecker.LaneHitState.HOLD ){
							note.NoteState = NoteDescription.NoteHitState.MISSED;
							JudgeScript.Instance.ApplyJudge (JudgeScript.Judge.MISS);
							JudgeScript.Instance.StoreJudge (JudgeScript.Judge.MISS);
						}
					}
					DestroyNote (note);
				}
			}
		}

		//CheckDestroyTiltNote (rightTiltNotes[0]);
		//CheckDestroyTiltNote (leftTiltNotes[0]);

	}

	/*
	private void CheckDestroyTiltNote(List<NoteDescription> notes){

		if (notes.Count > 0) {
			NoteDescription note = notes [0];
			float deltaTime = GetDeltaTime (note.HitTime);

			if (OutRange (deltaTime)) {
				Debug.Log ("destroy : " + note.ToString ());
				if (note.NoteState == NoteDescription.NoteHitState.READY) {
					note.NoteState = NoteDescription.NoteHitState.MISSED;
					JudgeScript.Instance.ApplyJudge (JudgeScript.Judge.MISS);
					JudgeScript.Instance.StoreJudge (JudgeScript.Judge.MISS);
				}
				DestroyNote (note);
			}
		}
	}*/

	private bool CheckReleaseLongNoteEndPoint(NoteDescription note){
		return (note.HitTime + note.Length - TimerScript.timePass) <= 0.175f;
	}

	private float GetDeltaTime(float noteTime){
		float deltaTime = noteTime - TimerScript.timePass;
		deltaTime *= 1000;
		return deltaTime;
	}

	private bool OutRange(float deltaTime){
		return deltaTime < -120;
	}

	private void DestroyNote(NoteDescription note){
		float deltaTime = GetDeltaTime (note.HitTime + note.Length);

		if (OutRange (deltaTime)) {
			note.DestroySelf ();
			laneNotes.Remove (note);
			rightTiltNotes [0].Remove (note);
			leftTiltNotes [0].Remove (note);
		}

	}






}
