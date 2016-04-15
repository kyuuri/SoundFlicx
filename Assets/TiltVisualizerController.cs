using UnityEngine;
using System.Collections;
using Leap;
using System.Collections.Generic;

public class TiltVisualizerController : MonoBehaviour {

	public NoteRenderer noteRenderer;
	
	public GameObject rTilt;
	public GameObject lTilt;
	private Leap.Controller controller;
	private float rightMost;
	private float leftMost;

	// Use this for initialization
	void Start () {
		controller = new Leap.Controller ();
		rightMost = rTilt.transform.position.x;
		leftMost = lTilt.transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		Frame frame = controller.Frame ();
		HandList hands = frame.Hands;
		Hand rHand = hands.Rightmost;
		Hand lHand = hands.Leftmost;

		List<NoteDescription> rightTiltNote = null;
		List<NoteDescription> leftTiltNote = null;
		if (noteRenderer.rightTiltNotes [0] != null) {
			rightTiltNote = noteRenderer.rightTiltNotes [0];
		}
		if (noteRenderer.leftTiltNotes [0] != null) {
			leftTiltNote = noteRenderer.leftTiltNotes [0];
		}
		Vector3 rTiltPos = new Vector3(0,rTilt.transform.position.y,rTilt.transform.position.z);
		Vector3 lTiltPos = new Vector3(0,lTilt.transform.position.y,lTilt.transform.position.z);

		float rRoll = ToDegrees(rHand.PalmNormal.Roll);
		float lRoll = ToDegrees(lHand.PalmNormal.Roll);

		float rPos = GetRightXPos (rRoll);
		float lPos = GetLeftXPos (lRoll);
		if(rightTiltNote != null || leftTiltNote.Count != null){
			if (CheckTimeTilt (rightTiltNote) || CheckTimeTilt (leftTiltNote)) {
//			state = TILT_STATE.TILTING;

				if (rPos < rightMost && rPos > leftMost) {
					rTilt.transform.position = new Vector3 (rPos, rTiltPos.y, rTiltPos.z);
				}
		
				if (lPos < rightMost && lPos > leftMost) {
					lTilt.transform.position = new Vector3 (lPos, lTiltPos.y, lTiltPos.z);
				}
			}
		}
	}
		
	bool CheckTimeTilt(List<NoteDescription> tiltNotes){
		if (tiltNotes.Count > 0) {
			NoteDescription note = null;
			for (int i = 0; i < tiltNotes.Count; i++) {
				note = tiltNotes [i];
				if (note.Length > 0) {
					if (TimerScript.timePass > note.HitTime + note.Length - 0.02f) {
						if (i + 2 < tiltNotes.Count - 1) {
							note = tiltNotes [i + 2];
						}
					}
					break;
				}
			}
			if (note.HitTime != null) {
				if (note.HitTime - TimerScript.timePass < 1)
					return true;
			}
			return false;
		}
		return false;
	}

	float ToDegrees (float Radian) {
		float Degrees;
		Degrees = Radian * 180 / Mathf.PI;
		return Degrees;
	}

	float GetRightXPos (float degrees) {
		float maxDegrees = 60;
		return (2 * leftMost / maxDegrees) * (degrees) + rightMost;
	} 

	float GetLeftXPos (float degrees) {
		float maxDegrees = 60;
		return (2 * leftMost / maxDegrees) * (degrees) + leftMost;
	}

}
