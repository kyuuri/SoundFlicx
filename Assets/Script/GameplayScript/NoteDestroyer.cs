﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoteDestroyer : MonoBehaviour {

	public PlayInformation playInformation;

	public LineHitChecker[] lineCheckers = new LineHitChecker[4];
	public LineRTiltChecker rTiltChecker;
	public LineLTiltChecker lTiltChecker;
	public List<NoteDescription>[] allnotes;
	private List<NoteDescription> laneNotes;
	private List<NoteDescription>[] rightTiltNotes;
	private List<NoteDescription>[] leftTiltNotes;


	void Update () {
		allnotes = playInformation.noteRenderer.allnotes;
		rightTiltNotes = playInformation.noteRenderer.rightTiltNotes;
		leftTiltNotes = playInformation.noteRenderer.leftTiltNotes;

		for (int i = 0; i < allnotes.Length; i++) {
			laneNotes = playInformation.noteRenderer.allnotes[i];
			if (laneNotes.Count > 0) {
				NoteDescription note = laneNotes [0];
				if (!note.IsLTilt && !note.IsRTilt) {
					float deltaTime = GetDeltaTime (note.HitTime);

					if (OutRange (deltaTime)) {
						if (note.NoteState == NoteDescription.NoteHitState.READY) {
							note.NoteState = NoteDescription.NoteHitState.MISSED;
							playInformation.judgeScript.ApplyJudge (JudgeScript.Judge.MISS);
							playInformation.judgeScript.StoreJudge (JudgeScript.Judge.MISS);
						}
						if (note.Length > 0 && note.NoteState != NoteDescription.NoteHitState.MISSED) {
							if (CheckReleaseLongNoteEndPoint (note)) {
								//nothing
							} else if (lineCheckers [i].laneState != LineHitChecker.LaneHitState.HOLD) {
								note.NoteState = NoteDescription.NoteHitState.MISSED;
								playInformation.judgeScript.ApplyJudge (JudgeScript.Judge.MISS);
								playInformation.judgeScript.StoreJudge (JudgeScript.Judge.MISS);
							}
						}
						if (DestroyNote (note)) {
							laneNotes.Remove (note);
						}
					}
				}
			}
		}

		CheckDestroyTiltNote (rightTiltNotes[0], "R");
		CheckDestroyTiltNote (leftTiltNotes[0], "L");

		CheckDestroyBar ();
	}


	private void CheckDestroyTiltNote(List<NoteDescription> notes, string side){
		if (notes.Count > 0) {
			NoteDescription note = notes [0];
			float deltaTime = GetDeltaTime (note.HitTime);

			if (note.IsRTilt || note.IsLTilt) {

				if (OutRange (deltaTime)) {
					if (note.Length == 0) {
						//ignore, just delete.
					} else {
						if (note.NoteState == NoteDescription.NoteHitState.READY) {
							note.NoteState = NoteDescription.NoteHitState.MISSED;
							playInformation.judgeScript.ApplyJudge (JudgeScript.Judge.MISS);
							playInformation.judgeScript.StoreJudge (JudgeScript.Judge.MISS);
						}
						if (note.Length > 0 && note.NoteState != NoteDescription.NoteHitState.MISSED) {
							if (side == "R") {
								bool miss = false;
								if (CheckReleaseLongNoteEndPoint (note)) {
									//nothing
								} else if (rTiltChecker.laneState != LineRTiltChecker.LaneTiltState.R2LTILT && note.TiltAngle < -0.1f) {
									miss = true;
								} else if (rTiltChecker.laneState != LineRTiltChecker.LaneTiltState.R2RTILT && note.TiltAngle > 0.1f) {
									miss = true;
								} else if (rTiltChecker.laneState != LineRTiltChecker.LaneTiltState.IDLE && note.TiltAngle >= -0.1f && note.TiltAngle <= 0.1f) {
									miss = true;
								}

								if (miss) {
									note.NoteState = NoteDescription.NoteHitState.MISSED;
									playInformation.judgeScript.ApplyJudge (JudgeScript.Judge.MISS);
									playInformation.judgeScript.StoreJudge (JudgeScript.Judge.MISS);
								}
							} else {
								bool miss = false;
								if (CheckReleaseLongNoteEndPoint (note)) {
									//nothing
								} else if (lTiltChecker.laneState != LineLTiltChecker.LaneTiltState.L2LTILT && note.TiltAngle < -0.1f) {
									miss = true;
								} else if (lTiltChecker.laneState != LineLTiltChecker.LaneTiltState.L2RTILT && note.TiltAngle > 0.1f) {
									miss = true;
								} else if (lTiltChecker.laneState != LineLTiltChecker.LaneTiltState.IDLE && note.TiltAngle >= -0.1f && note.TiltAngle <= 0.1f) {
									miss = true;
								}

								if (miss) {
									note.NoteState = NoteDescription.NoteHitState.MISSED;
									playInformation.judgeScript.ApplyJudge (JudgeScript.Judge.MISS);
									playInformation.judgeScript.StoreJudge (JudgeScript.Judge.MISS);
								}
							}

						}
					}
					if (DestroyNote (note)) {
						if (side == "R") {
							rightTiltNotes [0].Remove (note);
						} else {
							leftTiltNotes [0].Remove (note);
						}
					}
				}
			}
		}

		/*
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
		*/
	}

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

	private bool DestroyNote(NoteDescription note){
		float deltaTime = GetDeltaTime (note.HitTime + note.Length);

		if (OutRange (deltaTime)) {
			note.DestroySelf ();
			return true;
		}
		return false;
	}

	private void CheckDestroyBar(){
		List<GameObject> bars = playInformation.noteRenderer.bars;
		if (bars.Count > 0) {
			GameObject bar = bars [0];
			if (bar.transform.position.z - transform.position.z < -0.5f) {
				bars.RemoveAt (0);
				Destroy (bar, 0.01f);
			}
		}
	}






}
