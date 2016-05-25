﻿using UnityEngine;
using System.Collections;
using Leap;
using WindowsInput;
using System.Collections.Generic;
using UnityEngine.Networking;

public class SFControllerNetwork : NetworkBehaviour {

	public NoteRenderer noteRenderer;
	public ItemController itemController;

	public LineHitChecker b1;
	public LineHitChecker b2;
	public LineHitChecker b3;
	public LineHitChecker b4;

	public LineFlickChecker f1;
	public LineFlickChecker f2;

	public LineRTiltChecker rt;
	public LineLTiltChecker lt;

	public HandController handController;
	private Leap.Controller controller;
	private HandModel left;
	private HandModel right;
	private Finger[] fingers;
	private Finger[] previousFingers;

	private bool isRflicking = false;
	private bool isLflicking = false;
	private bool isSkilling = false;

	private float flickDelay = 0.18f;

	private float RDelay = 0;
	private float LDelay = 0;
	public float maxAngle;
	public float offsetDegrees;
	//	private float rightTiltDegrees;
	//	private float leftTiltDegrees;

	public float offset;


	private bool[] fingerStage = new bool[4];

	// Use this for initialization
	void Start () {
		//		Application.runInBackground = true;
		controller = new Leap.Controller ();
		fingers = new Finger[4];
		previousFingers = new Finger[4];

		offset = 5;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (!isLocalPlayer) {
//			Debug.Log ("IsLocalPlayer");
			return;
		} else {
			Debug.Log ("Local Player");
		}

		Leap.Frame frame = controller.Frame ();
		Leap.Frame previousFrame = controller.Frame (1);

		HandList hands = frame.Hands;

		HandList previousHands = previousFrame.Hands;

		if (hands.IsEmpty) {
			//			b1.KeyUp ();
			//			b2.KeyUp ();
			//			b3.KeyUp ();
			//			b4.KeyUp ();
			//			f1.KeyNone ();
			//			f2.KeyNone ();

			InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_Q);
			InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
			InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_E);
			InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_R);
			InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_Z);
			InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_X);
		}

		if (hands.Count == 1) {
			if (hands.Leftmost.IsLeft) {
				//				b3.KeyUp ();
				//				b4.KeyUp ();
				//				f2.KeyNone ();

				InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_E);
				InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_R);
				InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_X);
			} else {
				//				b1.KeyUp ();
				//				b2.KeyUp ();
				//				f1.KeyNone ();

				InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_Q);
				InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
				InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_Z);
			}
		}

		if (TimerScript.timePass > -1) {
			CheckFlick (hands);

			if (isLflicking) {
				LDelay += Time.deltaTime;
				//				b1.KeyUp ();
				//				b2.KeyUp ();
				InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_Q);
				InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
			}
			if (isRflicking) {
				RDelay += Time.deltaTime;
				//				b3.KeyUp ();
				//				b4.KeyUp ();
				InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_E);
				InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_R);
			}

			if(LDelay >= flickDelay){
				LDelay = 0;
				isLflicking = false;
			}
			if(RDelay >= flickDelay){
				RDelay = 0;
				isRflicking = false;
			}

			CheckPressing (hands, previousHands);
		}
		CheckTilt (hands, previousHands);

		//versus
		if (GlobalData.isVersus && itemController != null) {
			if (CheckSkillMotion (hands)) {
				isSkilling = true;
				itemController.UseItem ();
			}
		}
	}


	private void CheckPressing(HandList hands, HandList previousHands){
		Hand rightHand = hands.Rightmost;
		Hand leftHand = hands.Leftmost;

		Hand previousRightHand = previousHands.Rightmost;
		Hand previousLeftHand = previousHands.Leftmost;

		//		fingers[0] = leftHand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_MIDDLE)[0];
		//		fingers[1] = leftHand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_INDEX)[0];
		//		fingers[2] = rightHand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_INDEX)[0];
		//		fingers[3] = rightHand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_MIDDLE)[0];

		//		previousFingers[0] = previousLeftHand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_MIDDLE)[0];
		//		previousFingers[1] = previousLeftHand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_INDEX)[0];
		//		previousFingers[2] = previousRightHand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_INDEX)[0];
		//		previousFingers[3] = previousRightHand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_MIDDLE)[0];

		if (!isLflicking) {
			if (CheckPressingDistance (leftHand, previousLeftHand, 0, 0) && leftHand.IsLeft) {
				if (!fingerStage [0]) {
					//					b1.KeyUp ();
					//					b1.KeyDown ();
					//					b1.KeyHold ();
					InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_Q);
					InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_Q);
				}
				fingerStage [0] = true;
			}
			if (CheckPressingDistance (leftHand, previousLeftHand, 1, 0) && leftHand.IsLeft) {
				if (!fingerStage [1]) {
					//					b2.KeyUp ();
					//					b2.KeyDown ();
					//					b2.KeyHold ();
					InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_W);
					InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_W);
				}
				fingerStage [1] = true;
			}
		}
		if (!isRflicking) {
			if (CheckPressingDistance (rightHand, previousRightHand, 2, 0) && rightHand.IsRight) {
				if (!fingerStage [2]) {
					//					b3.KeyUp ();
					//					b3.KeyDown ();
					//					b3.KeyHold ();
					InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_E);
					InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_E);
					fingerStage [2] = true;
				}
			}
			if (CheckPressingDistance (rightHand, previousRightHand, 3, 0) && rightHand.IsRight) {
				if (!fingerStage [3]) {
					//					b4.KeyUp ();
					//					b4.KeyDown ();
					//					b4.KeyHold ();
					InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_R);
					InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_R);
					fingerStage [3] = true;
				}
			}
		}

		if (CheckPressingDistance (leftHand, previousLeftHand, 0, 1) && leftHand.IsLeft) {
			if (fingerStage [0]) {
				//			Debug.Log ("Release  Q");
				//				b1.KeyUp ();
				InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_Q);
				fingerStage [0] = false;
			}
		}
		if (CheckPressingDistance (leftHand, previousLeftHand, 1, 1) && leftHand.IsLeft) {
			if (fingerStage [1]) {
				//			Debug.Log ("Release W");
				//				b2.KeyUp ();
				InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_W);
				fingerStage [1] = false;
			}
		}
		if (CheckPressingDistance (rightHand, previousRightHand, 2, 1) && rightHand.IsRight) {
			if (fingerStage [2]) {
				//			Debug.Log ("Release E");
				//				b3.KeyUp ();
				InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_E);
				fingerStage [2] = false;
			}
		}
		if (CheckPressingDistance (rightHand, previousRightHand, 3, 1) && rightHand.IsRight) {
			if (fingerStage [3]) {
				//				b4.KeyUp ();
				//			Debug.Log ("Release R");
				InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_R);
				fingerStage [3] = false;
			}
		}
	}
	/**
	 * check pressing finger by compute distance between tip and palm
	 * hand is hand at current frame.
	 * previousHand is hand in previous frame.
	 * index is description of finger
	 * 0 = left middle finger
	 * 1 = left index finger
	 * 2 = right index finger
	 * 3 = right middle finger
	 * fingerstate is state of pressing 0 = press finger, 1 = release finger
	 * 
	 */
	private bool CheckPressingDistance(Hand hand, Hand previousHand, float index, float fingerState){
		Finger finger;
		Finger previousFinger;
		float distanceOffset = 1;
		float distance = 1.1f;
		float speed = 124;

		if (index == 0 && hand.IsLeft) {
			finger = hand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_MIDDLE) [0];
			previousFinger = previousHand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_MIDDLE) [0];
		} else if (index == 1 && hand.IsLeft) {
			finger = hand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_INDEX) [0];
			previousFinger = previousHand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_INDEX) [0];
		} else if (index == 2 && hand.IsRight) {
			finger = hand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_INDEX) [0];
			previousFinger = previousHand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_INDEX) [0];
		} else if (index == 3 && hand.IsRight) {
			finger = hand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_MIDDLE) [0];
			previousFinger = previousHand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_MIDDLE) [0];
		} else {
			return false;
		}

		//if(index == 2){
		//			Debug.Log (calculateAngle(finger));
		//Debug.Log(finger.Bone(Bone.BoneType.TYPE_PROXIMAL).Basis.zBasis.y);
		//			Debug.Log(finger.Bone(Bone.BoneType.TYPE_PROXIMAL).Basis.zBasis.y > 0.60f);
		//Debug.Log(CaculateTipDistance(finger));
		//}
		//		if (fingerState == 0)
		//			return calculateAngle (finger) <= 130;
		//		else
		//			return calculateAngle (finger) > 140;

		float previousDistance = GetPressingDistance (previousHand.PalmPosition, previousFinger.TipPosition);
		float currentDistance = GetPressingDistance (hand.PalmPosition, finger.TipPosition);
		float deltaDistance = previousDistance - currentDistance;

		float tipDistance = CaculateTipDistance(finger);

		if (fingerState == 0) {
			//			return deltaDistance * distanceOffset > distance || finger.TipVelocity.y < -speed;
			return (deltaDistance * distanceOffset > distance/3.0f && finger.TipVelocity.y < -speed);
		} else if (fingerState == 1) {
			//			return deltaDistance * distanceOffset < -(distance/3) || finger.TipVelocity.y > speed/2; 
			return (deltaDistance * distanceOffset < -(distance/2.2) || finger.TipVelocity.y > speed/2) && tipDistance > 100f; 
		} else
			return false;

	}


	private void CheckFlick(HandList hands){
		Hand rightHand = hands.Rightmost;
		Hand leftHand = hands.Leftmost;

		float rightHandYaw = rightHand.Direction.Yaw * offset;
		float leftHandYaw = leftHand.Direction.Yaw * offset * -1;

		float speed = 128;

		f1.KeyNone ();
		f2.KeyNone ();
		//		InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_X);
		//		InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_Z);

		if (!isRflicking) {
			//		if (rightHandYaw > 1.5 && rightHand.PalmVelocity.x > speed) {
			//			if (rightHandYaw > 1.21f && rightHand.PalmVelocity.x > speed && rightHand.IsRight) {
			if (rightHand.PalmVelocity.x > speed && rightHand.IsRight) {
				//				Debug.Log ("right swipe");
				//			Debug.Log(rightHandYaw);
				f2.KeyFlick();
				//				InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_X);
				isRflicking = true;
				LDelay = 0;
			}
		}
		if (!isLflicking) {
			//		if (leftHandYaw > 1.5  && leftHand.PalmVelocity.x < -speed) {
			//			if (leftHandYaw > 1.21f && leftHand.PalmVelocity.x < -speed && leftHand.IsLeft) {
			if (leftHand.PalmVelocity.x < -speed && leftHand.IsLeft) {
				//Debug.Log ("left swipe");
				//			Debug.Log(leftHandYaw);
				f1.KeyFlick();
				//				InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_Z);
				isLflicking = true;
				RDelay = 0;
			}
		}
	}

	private void CheckTilt (HandList hands, HandList previousHands){
		Hand rHand = hands.Rightmost;
		Hand lHand = hands.Leftmost;

		Hand previousRHand = previousHands.Rightmost;
		Hand previousLHand = previousHands.Leftmost;
		//		float offset = 10;

		//		float rRoll = rHand.PalmNormal.Roll * offset;
		//		float lRoll = lHand.PalmNormal.Roll * offset;
		//		float previousRRoll = previousRHand.PalmNormal.Roll * offset;
		//		float previousLRoll = previousLHand.PalmNormal.Roll * offset;
		float rRoll = rHand.PalmNormal.Roll;
		float lRoll = lHand.PalmNormal.Roll;
		float previousRRoll = previousRHand.PalmNormal.Roll;
		float previousLRoll = previousLHand.PalmNormal.Roll;

		float rRollDegrees = ToDegrees (rRoll);
		float lRollDegrees = ToDegrees (lRoll);

		float rightTilt = (rRoll - previousRRoll) * 100;
		float leftTilt = (lRoll - previousLRoll) * 100;
		float deltaTilt = 0.05f;
		float holdTilt = 0.8f;

		List<NoteDescription> rightTiltNote = noteRenderer.rightTiltNotes[0];
		List<NoteDescription> leftTiltNote = noteRenderer.leftTiltNotes[0];


		if (hands.Count == 0) {
			rt.KeyRIDLE ();
			lt.KeyLIDLE ();
			//			InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_M);
			//			InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_N);
			//			InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_V);
			//			InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_B);
			//		} else if(false){
			//
			//			NoteDescription rNote = GetNote(rightTiltNote);
			//			NoteDescription lNote = GetNote(leftTiltNote);
			//
			//
			//			if (rNote.TiltAngle != 0) {
			//				if (rHand.IsValid && rHand.IsRight) {
			//					if (rightTilt > deltaTilt) {
			////				Debug.Log ("RIGHT left");
			//						InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_N);
			//					} else if (rightTilt < -deltaTilt) {
			////				Debug.Log ("RIGHT right");
			//						InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_M);
			//					} else {
			//						Debug.Log(rightTilt + " ..... " + deltaTilt);
			//						InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_N);
			//						InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_M);
			//					}
			//				}
			//			} else if(rNote.TiltAngle == 0){
			//				if (rHand.IsValid && rHand.IsRight) {
			//					if (rightTilt > holdTilt) {
			////						Debug.Log ("RIGHT left");
			//						InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_N);
			//					} else if (rightTilt < -holdTilt) {
			////						Debug.Log ("RIGHT right");
			//						InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_M);
			//					} else {
			//						InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_N);
			//						InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_M);
			//					}
			//				}
			//			}
			//
			//			//left hand tilt
			//			if (lNote.TiltAngle != 0) {
			//				if (lHand.IsValid && lHand.IsLeft) {
			////				Debug.Log ("LEFT left");
			//					if (leftTilt > deltaTilt) {
			//						InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_V);
			//					} else if (leftTilt < -deltaTilt) {
			////				Debug.Log ("LEFT right");
			//						InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_B);
			//					} else {
			//						Debug.Log(leftTilt + " ..... " + deltaTilt);
			//						InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_V);
			//						InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_B);
			//					}
			//				}
			//			} else if (lNote.TiltAngle == 0) {
			//				if (lHand.IsValid && lHand.IsLeft) {
			//					//				Debug.Log ("LEFT left");
			//					if (leftTilt > holdTilt) {
			//						InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_V);
			//					} else if (leftTilt < -holdTilt) {
			//						//				Debug.Log ("LEFT right");
			//						InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_B);
			//					} else {
			//						InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_V);
			//						InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_B);
			//					}
			//				}
		} else {
			CheckRollAngle (lHand, leftTiltNote);
			CheckRollAngle (rHand, rightTiltNote);
		}
	}


	private NoteDescription GetNote( List<NoteDescription> tiltNotes ){
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
		return note;
	}

	private void CheckRollAngle(Hand hand, List<NoteDescription> tiltNotes){
		float rollDegrees = ToDegrees (hand.PalmNormal.Roll);
		float rightTiltDegrees = 0;
		float leftTiltDegrees = 0;

		NoteDescription note = null;

		if (tiltNotes.Count > 0) {
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
		} else {
			return;
		}

		if (hand.IsRight && note.IsRTilt) {
			if (note.TiltAngle == 0) {
				if (rollDegrees > 35 || rollDegrees < 0) {
					rt.KeyRIDLE ();
					//					InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_M);
					//					InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_N);
				} else {
					rt.KeyRToR ();
					//					InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_M);
				}
			} else if (note.TiltAngle < 0.1f) {
				rightTiltDegrees = FindDegrees (note);
				//				Debug.Log (rollDegrees +  "       " + rightTiltDegrees);
				if (rollDegrees > rightTiltDegrees - offsetDegrees && rollDegrees < rightTiltDegrees + offsetDegrees) {
					//				if (Mathf.Abs(rollDegrees-maxAngle)/2 > rightTiltDegrees - offsetDegrees && rollDegrees < rightTiltDegrees + offsetDegrees) {
					rt.KeyRToL();
					//					InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_N);
					//					InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_M);
					//					Debug.Log (rollDegrees + " > " + (rightTiltDegrees - offsetDegrees) + "   ||    " + rollDegrees + " < " + (rightTiltDegrees + offsetDegrees));
				} else {
					//					Debug.Log (rollDegrees + " < " + (rightTiltDegrees - offsetDegrees) + "   ||    " + rollDegrees + " > " + (rightTiltDegrees + offsetDegrees));
					rt.KeyRIDLE();
					//					InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_N);
				}
			} else if (note.TiltAngle > 0.1f) {
				rightTiltDegrees = FindDegrees (note) * -1;
				//					Debug.Log (rollDegrees - maxAngle);
				//				if (2*rollDegrees - maxAngle > rightTiltDegrees - offsetDegrees && 2*rollDegrees - maxAngle < rightTiltDegrees + offsetDegrees) {
				if (rollDegrees - maxAngle > rightTiltDegrees - offsetDegrees && rollDegrees - maxAngle < rightTiltDegrees + offsetDegrees) {
					rt.KeyRToR ();
					//					InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_M);
					//					InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_N);
				} else {
					rt.KeyRIDLE ();
					//					InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_M);
				}
			}
		}

		if (hand.IsLeft && note.IsLTilt) {
			rollDegrees *= -1;
			if(note.TiltAngle == 0){
				if (rollDegrees > 35 || rollDegrees < 0) {
					//					Debug.Log (rollDegrees);
					lt.KeyLIDLE();
					//					InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_V);
					//					InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_B);
				} else {
					lt.KeyLToL ();
					//					InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_V);
				}
			} else if (note.TiltAngle > 0.1f) {
				//				 if (note.TiltAngle > 0.1f) {
				leftTiltDegrees = FindDegrees (note);
				if (rollDegrees > leftTiltDegrees - offsetDegrees && rollDegrees < leftTiltDegrees + offsetDegrees) {
					lt.KeyLToR ();
					//					InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_B);
					//					InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_V);
				} else {
					lt.KeyLIDLE();
					//					InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_B);
				}
			} else if (note.TiltAngle < 0.1f) {
				leftTiltDegrees = FindDegrees (note)*-1;
				//				Debug.Log (rollDegrees - maxAngle +  "       " + leftTiltDegrees);
				//				if (2*rollDegrees - maxAngle > leftTiltDegrees - offsetDegrees && 2*rollDegrees - maxAngle < leftTiltDegrees + offsetDegrees) {
				if (rollDegrees - maxAngle > leftTiltDegrees - offsetDegrees && rollDegrees - maxAngle < leftTiltDegrees + offsetDegrees) {
					lt.KeyLToL ();
					//					InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_V);
					//					InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_B);
				} else {
					lt.KeyLIDLE();
					//					InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_V);				
				}
			}
		}
	}

	private bool CheckSkillMotion(HandList hands){
		Hand leftHand = hands.Leftmost;
		Hand rightHand = hands.Rightmost;
		float rHandSpeed = rightHand.PalmVelocity.y;
		float lHandSpeed = leftHand.PalmVelocity.y;

		if (!isSkilling) {
			float speed = 500;

			return leftHand.PalmVelocity.y > speed || rightHand.PalmVelocity.y > speed;
		} else {
			if (rHandSpeed < 0 && lHandSpeed < 0) {
				isSkilling = false;
			}
			return false;	
		}
	}

	private bool CheckPress(float velocity){
		return velocity < -200;
	}

	private bool CheckRelease(float velocity){
		return velocity > 100;
	}

	bool LessThanEqual (Leap.Vector a, Vector3 b){
		return (Mathf.Abs (a.x) <= b.x) && (Mathf.Abs (a.y) <= b.y) && (Mathf.Abs (a.z) <= b.z);
	}

	private float GetPressingDistance(Vector palmPosition, Vector tipPosition){
		float distance = Mathf.Sqrt((Mathf.Pow(tipPosition.x - palmPosition.x,2) 
			+ Mathf.Pow(tipPosition.y - palmPosition.y,2) + Mathf.Pow(tipPosition.z - palmPosition.z,2)));
		return distance ;
	}

	private float CalculateAngle(Finger finger){
		Vector v1 = finger.Bone (Bone.BoneType.TYPE_METACARPAL).NextJoint; // vertex
		Vector v2 = finger.Bone (Bone.BoneType.TYPE_METACARPAL).PrevJoint;
		Vector v3 = finger.Bone (Bone.BoneType.TYPE_PROXIMAL).NextJoint;

		Vector3 vec1 = new Vector3 (v1.x, v1.y, v1.z);
		Vector3 vec2 = new Vector3 (v2.x, v2.y, v2.z);
		Vector3 vec3 = new Vector3 (v3.x, v3.y, v3.z);

		float d12 = Vector3.Distance(vec1, vec2);
		float d13 = Vector3.Distance(vec1, vec3);
		float d23 = Vector3.Distance(vec2, vec3);
		float angle = Mathf.Acos ((d12*d12 + d13*d13 - d23*d23)/(2*d12*d13));

		return angle * 180 / Mathf.PI;
	}

	private float CaculateTipDistance(Finger finger){
		Vector v1 = finger.Bone (Bone.BoneType.TYPE_METACARPAL).PrevJoint;
		Vector v2 = finger.Bone (Bone.BoneType.TYPE_DISTAL).NextJoint;

		Vector3 vec1 = new Vector3 (v1.x, v1.y, v1.z);
		Vector3 vec2 = new Vector3 (v2.x, v2.y, v2.z);

		return Vector3.Distance(vec1, vec2);
	}

	float ToDegrees (float Radian)
	{
		float Degrees;
		Degrees = Radian * 180 / Mathf.PI;
		return Degrees;
	}

	float FindDegrees(NoteDescription note){
		float len = note.Length;
		float hitTime = note.HitTime;
		float currentTime = TimerScript.timePass;
		float initTime = 0;

		return ((45 / len) * (currentTime - hitTime) + initTime);
	}
}


//45 >>  0
//35 >> 5
//25 >> 10
//5  >> 20
//-5 >> 25
//-25>> 35
//-45 >> 45