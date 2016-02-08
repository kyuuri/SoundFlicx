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
	public float offset;

	// Use this for initialization
	void Start () {
		Application.runInBackground = true;
		controller = new Leap.Controller ();
		fingers = new Finger[4];
	}

	// Update is called once per frame
	void FixedUpdate () {
		Leap.Frame frame = controller.Frame ();
		Leap.Frame previousFrame = controller.Frame (1);

		HandList hands = frame.Hands;
		Hand rightHand = hands.Rightmost;
		Hand leftHand = hands.Leftmost;
	
		float rightHandYaw = rightHand.Direction.Yaw * offset;
		float leftHandYaw = leftHand.Direction.Yaw * offset * -1;

		fingers[2] = rightHand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_INDEX)[0];
		fingers[3] = rightHand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_MIDDLE)[0];
		fingers[1] = leftHand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_INDEX)[0];
		fingers[0] = leftHand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_MIDDLE)[0];

		if (hands.IsEmpty) {
			InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_Q);
			InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
			InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_E);
			InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_R);
		}

//		float speed = 450;
//		if(fingers[0].Hand.IsLeft && GetPressDistance(leftHand, fingers[0]) < speed && GetPressDistance(leftHand, fingers[0]) > 70) {
//			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_Q);
//		}
//		if(fingers[1].Hand.IsLeft && GetPressDistance(leftHand, fingers[1]) < speed && GetPressDistance(leftHand, fingers[1]) > 70) {
//			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_W);
//		}
//		if(fingers[2].Hand.IsRight && GetPressDistance(rightHand, fingers[2]) < speed && GetPressDistance(rightHand, fingers[2]) > 70 ) {
//			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_E);
//		}
//		if(fingers[3].Hand.IsRight && GetPressDistance(rightHand, fingers[3]) < speed && GetPressDistance(rightHand, fingers[3]) > 70) {
//			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_R);
//		}
//		if(fingers[0].Hand.IsLeft && GetPressDistance(leftHand, fingers[0]) > speed) {
//			InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_Q);
//		}
//		if(fingers[1].Hand.IsLeft && GetPressDistance(leftHand, fingers[1]) > speed) {
//			InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_W);
//		}
//		if(fingers[2].Hand.IsRight && GetPressDistance(rightHand, fingers[2]) > speed) {
//			InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_E);
//		}
//		if(fingers[3].Hand.IsRight && GetPressDistance(rightHand, fingers[3]) > speed) {
//			InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_R);
//		}
		if (rightHandYaw > 2 && rightHand.PalmVelocity.x > 150) {
			Debug.Log ("right swipe");
//			Debug.Log(rightHandYaw);
			InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_X);
		}
		if (leftHandYaw > 2  && leftHand.PalmVelocity.x < 150) {
			Debug.Log ("left swipe");
//			Debug.Log(leftHandYaw);
			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_Z);
		}
		Vector3 vecterTemp = new Vector3(200,400,200);
		//if(Mathf.Abs(rightHand.PalmVelocity.y) < 200 && Mathf.Abs(leftHand.PalmVelocity.y) < 200 ){
		if(LessThanEqual(rightHand.PalmVelocity,vecterTemp)){
			if (CheckPress (fingers[0].TipVelocity.y) && fingers[0].Hand.IsLeft) {
				InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_Q);
			}
			if (CheckPress (fingers[1].TipVelocity.y) && fingers[1].Hand.IsLeft) {
				InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_W);
			}

			if (CheckPress (fingers[2].TipVelocity.y) && fingers[2].Hand.IsRight) {
				//Debug.Log (rightIndexFinger.TipVelocity.y);
			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_E);
			}

			if (CheckPress (fingers[3].TipVelocity.y) && fingers[3].Hand.IsRight) {
				InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_R);
			}

			if (CheckRelease (fingers[0].TipVelocity.y) && fingers[0].Hand.IsLeft) {
				InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_Q);
			}

			if (CheckRelease (fingers[1].TipVelocity.y) && fingers[1].Hand.IsLeft) {
				InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
			}

			if (CheckRelease (fingers[2].TipVelocity.y) && fingers[2].Hand.IsRight) {
				InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_E);
			}
			if (CheckRelease (fingers[3].TipVelocity.y) && fingers[3].Hand.IsRight) {
				InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_R);
			}
		}
		//Debug.Log (GetPressDistance(rightHand, fingers[2]));

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

	private float GetPressDistance(Hand hand, Finger finger){
		float distance = Mathf.Sqrt((Mathf.Pow(finger.TipPosition.x-hand.PalmPosition.x,2) 
			+ Mathf.Pow(finger.TipPosition.y-hand.PalmPosition.y,2) + Mathf.Pow(finger.TipPosition.z-hand.PalmPosition.z,2)));
//		float distance = Mathf.Sqrt((Mathf.Pow(finger.TipVelocity.x-hand.PalmVelocity.x,2) 
//			+ Mathf.Pow(finger.TipVelocity.y-hand.PalmVelocity.y,2) + Mathf.Pow(finger.TipVelocity.z-hand.PalmVelocity.z,2)));
		return distance * 5;
	}
}
