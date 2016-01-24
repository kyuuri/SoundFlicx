using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoteDestroyer : MonoBehaviour {

	public List<NoteDescription>[] allnotes;
	private List<NoteDescription> laneNotes;

	void Update () {
		allnotes = NoteRenderer.allnotes;

		for (int i = 0; i < allnotes.Length; i++) {
			laneNotes = NoteRenderer.allnotes[i];
			if (laneNotes.Count > 0) {
				NoteDescription note = laneNotes [0];
				float deltaTime = GetDeltaTime (note.HitTime);

				if (OutRange (deltaTime)) {
					Debug.Log (0);
					note.DestroySelf ();
					laneNotes.Remove (note);
				}
			}
		}
	}

	private float GetDeltaTime(float noteTime){
		float hitDeltaTime = noteTime - TimerScript.timePass;
		hitDeltaTime *= 1000;
		return hitDeltaTime;
	}

	private bool OutRange(float deltaTime){
		return deltaTime < -120;
	}






}
