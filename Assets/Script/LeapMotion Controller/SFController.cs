using UnityEngine;
using System.Collections;
using Leap;
using WindowsInput;

public class SFController : MonoBehaviour {

	public HandController handController;
	private Leap.Controller controller;
	private HandModel left;
	private HandModel right;
	private Finger[] fingers;
	private Finger[] previousFingers;

	public float offset;

	// Use this for initialization
	void Start () {
		Application.runInBackground = true;
		controller = new Leap.Controller ();
		fingers = new Finger[4];
		previousFingers = new Finger[4];


		offset = 5;
	}

	// Update is called once per frame
	void FixedUpdate () {
		Leap.Frame frame = controller.Frame ();
		Leap.Frame previousFrame = controller.Frame (1);

		HandList hands = frame.Hands;

		HandList previousHands = previousFrame.Hands;



		if (hands.IsEmpty) {
			InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_Q);
			InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
			InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_E);
			InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_R);
		}

		CheckPressing (hands, previousHands);
		CheckFlick (hands);
		//		Debug.Log ((GetPressingDistance(previousRightHand, previousFingers[2]) - GetPressingDistance (rightHand, fingers [2]))*distanceOffset);
		//		if (rightHandYaw > 2 && rightHand.PalmVelocity.x > 150) {
		//			Debug.Log ("right swipe");
		////			Debug.Log(rightHandYaw);
		//			InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_X);
		//		}
		//		if (leftHandYaw > 2  && leftHand.PalmVelocity.x < 150) {
		//			Debug.Log ("left swipe");
		////			Debug.Log(leftHandYaw);
		//			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_Z);
		//		}
		//		Vector3 vecterTemp = new Vector3(200,400,200);
		//		//if(Mathf.Abs(rightHand.PalmVelocity.y) < 200 && Mathf.Abs(leftHand.PalmVelocity.y) < 200 ){
		//		if(LessThanEqual(rightHand.PalmVelocity,vecterTemp)){
		//			if (CheckPress (fingers[0].TipVelocity.y) && fingers[0].Hand.IsLeft) {
		//				InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_Q);
		//			}
		//			if (CheckPress (fingers[1].TipVelocity.y) && fingers[1].Hand.IsLeft) {
		//				InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_W);
		//			}
		//
		//			if (CheckPress (fingers[2].TipVelocity.y) && fingers[2].Hand.IsRight) {
		//				//Debug.Log (rightIndexFinger.TipVelocity.y);
		//			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_E);
		//			}
		//
		//			if (CheckPress (fingers[3].TipVelocity.y) && fingers[3].Hand.IsRight) {
		//				InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_R);
		//			}
		//
		//			if (CheckRelease (fingers[0].TipVelocity.y) && fingers[0].Hand.IsLeft) {
		//				InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_Q);
		//			}
		//
		//			if (CheckRelease (fingers[1].TipVelocity.y) && fingers[1].Hand.IsLeft) {
		//				InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
		//			}
		//
		//			if (CheckRelease (fingers[2].TipVelocity.y) && fingers[2].Hand.IsRight) {
		//				InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_E);
		//			}
		//			if (CheckRelease (fingers[3].TipVelocity.y) && fingers[3].Hand.IsRight) {
		//				InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_R);
		//			}
		//		}

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

		if (CheckPressingDistance (leftHand, previousLeftHand, 0, 0) && leftHand.IsLeft) {
			//			Debug.Log ("Press Q");
			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_Q);
		}
		if (CheckPressingDistance (leftHand, previousLeftHand, 1, 0) && leftHand.IsLeft) {
			//			Debug.Log ("Press W");
			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_W);
		}
		if (CheckPressingDistance (rightHand, previousRightHand, 2, 0) && rightHand.IsRight) {
			//			Debug.Log ("Press E");
			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_E);
		}
		if (CheckPressingDistance (rightHand, previousRightHand, 3, 0) && rightHand.IsRight) {
			//			Debug.Log ("Press R");
			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_R);
		}
		if (CheckPressingDistance (leftHand, previousLeftHand, 0, 1) && leftHand.IsLeft) {
			//			Debug.Log ("Release  Q");
			InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_Q);
		}
		if (CheckPressingDistance (leftHand, previousLeftHand, 1, 1) && leftHand.IsLeft) {
			//			Debug.Log ("Release W");
			InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_W);
		}
		if (CheckPressingDistance (rightHand, previousRightHand, 2, 1) && rightHand.IsRight) {
			//			Debug.Log ("Release E");
			InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_E);
		}
		if (CheckPressingDistance (rightHand, previousRightHand, 3, 1) && rightHand.IsRight) {
			//			Debug.Log ("Release R");
			InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_R);
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
		float distance = 1.2f;
		float speed = 200;

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

		float previousDistance = GetPressingDistance (previousHand.PalmPosition, previousFinger.TipPosition);
		float currentDistance = GetPressingDistance (hand.PalmPosition, finger.TipPosition);
		float deltaDistance = previousDistance - currentDistance;
		if (fingerState == 0) {
			//			return deltaDistance * distanceOffset > distance || finger.TipVelocity.y < -speed;
			return deltaDistance * distanceOffset > distance || finger.TipVelocity.y < -speed;
		} else if (fingerState == 1) {
			//			return deltaDistance * distanceOffset < -(distance/3) || finger.TipVelocity.y > speed/2; 
			return deltaDistance * distanceOffset < -(distance / 4) || finger.TipVelocity.y > speed/2; 
		} else
			return false;

	}

	private void CheckFlick(HandList hands){
		Hand rightHand = hands.Rightmost;
		Hand leftHand = hands.Leftmost;

		float rightHandYaw = rightHand.Direction.Yaw * offset;
		float leftHandYaw = leftHand.Direction.Yaw * offset * -1;

		float speed = 30;
		//		if (rightHandYaw > 1.5 && rightHand.PalmVelocity.x > speed) {
		if (rightHandYaw > 1 && rightHand.PalmVelocity.x > speed) {
			Debug.Log ("right swipe");
			//			Debug.Log(rightHandYaw);
			InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_X);
		}
		//		if (leftHandYaw > 1.5  && leftHand.PalmVelocity.x < -speed) {
		if (leftHandYaw > 1  && leftHand.PalmVelocity.x < -speed) {
			Debug.Log ("left swipe");
			//			Debug.Log(leftHandYaw);
			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_Z);
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
}
