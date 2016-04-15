using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraRotation : MonoBehaviour {

	public static Vector3 rotateVector;

	public NoteRenderer noteRenderer;

	private bool rotate = false;

	public Camera gameCamera;

	public Transform par;
	public Transform otherPar;

	private Vector3 parInitPos;
	private Vector3 otherParInitPos;

	private List<NoteDescription> allLeftTilt;
	private List<NoteDescription> allRightTilt;

	private Quaternion initRotation;

	private int count = 0;


	// Use this for initialization
	void Start () {
		parInitPos = par.position;
		otherParInitPos = otherPar.position;
		initRotation = transform.rotation;

		//Debug.Log(par.position.x); // 1.5
	}
	
	// Update is called once per frame
	void Update () {
		rotate = false;

		allLeftTilt = noteRenderer.leftTiltNotes[0];

		allRightTilt = noteRenderer.rightTiltNotes[0];

		float angle = 0;

		if (allRightTilt.Count > 0) {
			NoteDescription note = null;
			for (int i = 0; i < allRightTilt.Count; i++) {
				note = allRightTilt [i];
				if (note.Length > 0) {
					if (TimerScript.timePass > note.HitTime + note.Length - 0.01f) {
						if (i + 2 < allRightTilt.Count - 1) {
							note = allRightTilt [i + 2];
						}
					}
					break;
				}
			}

			float cal = CalculateAngle (note);

			if (cal != 0) {
				if (note.TiltAngle < 0) {
					cal += (parInitPos.x - gameCamera.transform.position.x);
				} else if (note.TiltAngle > 0) {
					cal -= (parInitPos.x - gameCamera.transform.position.x);
				} else {
					cal = -cal;
				}
			}
			angle += cal;
		}

		if (allLeftTilt.Count > 0) {
			NoteDescription note = null;
			for (int i = 0; i < allLeftTilt.Count; i++) {
				note = allLeftTilt [i];
				if (note.Length > 0) {
					if (TimerScript.timePass > note.HitTime + note.Length - 0.02f) {
						if (i + 2 < allLeftTilt.Count - 1) {
							note = allLeftTilt [i + 2];
						}
					}
					break;
				}
			}

			float cal = CalculateAngle (note);

			if (cal != 0) {
				if (note.TiltAngle < 0) {
					cal += (parInitPos.x - gameCamera.transform.position.x);
				} else if (note.TiltAngle > 0) {
					cal -= (parInitPos.x - gameCamera.transform.position.x);
				} else {
					cal = -cal;
				}
			}
			angle += cal;

		}

		if (angle == -1.425f || angle == 1.425f || angle == -2.85f || angle == 2.85f) {
			count++;
		} else {
			count = 0;
		}

		if (count < 2) {
			transform.Rotate (Vector3.back * 0.1f, angle / 25.0f);
		}
		else if (count < 20) {
			transform.Rotate (Vector3.back * 0.1f, angle / 25.0f);
		}
		else if (count < 26) {
			transform.Rotate (Vector3.back * 0.1f, angle / 50.0f);
		}

		if (angle == 0) {
			
			if (transform.rotation.z < 0) {
				rotate = true;
				rotateVector = Vector3.forward;
			} else if (transform.rotation.z > 0) {
				rotate = true;
				rotateVector = Vector3.back;
			}

			if (rotate) {
				transform.Rotate (rotateVector, 4.0f * Time.deltaTime);
				if (transform.rotation.z < 0.002f && transform.rotation.z > -0.002f) {
					transform.rotation = initRotation;
				}
			}
		}
	}

	private float CalculateAngle (NoteDescription note){
		float currentTime = TimerScript.timePass;

		if (currentTime > note.HitTime + note.Length || currentTime < note.HitTime)
			return 0.0f;

		float hitTime = note.HitTime;
		float length = note.Length;
		float diffPos = (otherParInitPos.x - parInitPos.x);

		float x;

		if (note.TiltAngle < -0.1f || note.TiltAngle > 0.1f) {

			// pos = (del pos / del time) * (@time-time1 ) + pos1
			x = (diffPos / length) * (currentTime - hitTime) + (parInitPos.x - gameCamera.transform.position.x);

			if (note.TiltAngle > 0) {
				x = -x;
			}
			return x;

		} else if (note.TiltAngle >= -0.1f && note.TiltAngle <= 0.1f){
			return note.NoteObject.transform.position.x - gameCamera.transform.position.x;
		}

		return 0.0f;
	}
}
