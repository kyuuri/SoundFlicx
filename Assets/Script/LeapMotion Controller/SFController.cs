using UnityEngine;
using System.Collections;
using Leap;
using WindowsInput;

public class SFController : MonoBehaviour {

	public HandController handController;
	private Leap.Controller controller;
	private HandModel left;
	private HandModel right;

	// Use this for initialization
	void Start () {
		Application.runInBackground = true;
		controller = new Leap.Controller ();
		controller.EnableGesture (Gesture.GestureType.TYPEKEYTAP);
	}

	// Update is called once per frame
	void FixedUpdate () {
		//left = handController.leftPhysicsModel;
		//right = handController.rightPhysicsModel;

		//CheckPress ();
		Leap.Frame frame = controller.Frame ();
		HandList hands = frame.Hands;
		Hand rightHand = hands.Rightmost;
		Hand leftHand = hands.Leftmost;


		GestureList gestures = frame.Gestures();

		for (int i = 0; i < gestures.Count; i++) {
			if (gestures [i].Type == Gesture.GestureType.TYPE_KEY_TAP) {
				Debug.Log ("kok");
			}
		}
			
		Finger rightIndexFinger = rightHand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_INDEX)[0];
		Finger rightMiddleFinger = rightHand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_MIDDLE)[0];

		Finger leftIndexFinger = leftHand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_INDEX)[0];
		Finger leftMiddleFinger = leftHand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_MIDDLE)[0];

		//Debug.Log (rightHand.PalmVelocity);
		if (hands.IsEmpty) {
			InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_Q);
			InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
			InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_E);
			InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_R);
		}

		Vector3 vecterTemp = new Vector3(200,400,200);
		//if(Mathf.Abs(rightHand.PalmVelocity.y) < 200 && Mathf.Abs(leftHand.PalmVelocity.y) < 200 ){
		if(LessThanEqual(rightHand.PalmVelocity,vecterTemp)){
		if (CheckPress (leftIndexFinger.TipVelocity.y) && leftIndexFinger.Hand.IsLeft) {
			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_W);
		}
		if (CheckPress (leftMiddleFinger.TipVelocity.y) && leftMiddleFinger.Hand.IsLeft) {
			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_Q);
		}

		if (CheckPress (rightIndexFinger.TipVelocity.y) && rightIndexFinger.Hand.IsRight) {
				//Debug.Log (rightIndexFinger.TipVelocity.y);
			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_E);
		}

		if (CheckPress (rightMiddleFinger.TipVelocity.y) && rightMiddleFinger.Hand.IsRight) {
			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_R);
		}

		if (CheckRelease (leftIndexFinger.TipVelocity.y) && leftIndexFinger.Hand.IsLeft) {
			InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
		}

		if (CheckRelease (leftMiddleFinger.TipVelocity.y) && leftMiddleFinger.Hand.IsLeft) {
			InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_Q);
		}

		if (CheckRelease (rightIndexFinger.TipVelocity.y) && rightIndexFinger.Hand.IsRight) {
			InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_E);
		}
		if (CheckRelease (rightMiddleFinger.TipVelocity.y) && rightMiddleFinger.Hand.IsRight) {
			InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_R);
		}
		}
		Vector tap = rightIndexFinger.TipPosition - rightHand.PalmPosition;
		Debug.Log (tap);
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

}
