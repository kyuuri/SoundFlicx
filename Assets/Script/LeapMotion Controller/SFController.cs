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
	}

	// Update is called once per frame
	void Update () {
		//left = handController.leftPhysicsModel;
		//right = handController.rightPhysicsModel;

		//CheckPress ();

		Leap.Frame frame = controller.Frame ();
		HandList hands = frame.Hands;
		Hand rightHand = hands.Rightmost;
		Hand leftHand = hands.Leftmost;

		Finger rightIndexFinger = rightHand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_INDEX)[0];
		Finger rightMiddleFinger = rightHand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_MIDDLE)[0];

		Finger leftIndexFinger = leftHand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_INDEX)[0];
		Finger leftMiddleFinger = leftHand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_MIDDLE)[0];
	
		if (CheckPress (leftIndexFinger.TipVelocity.y) && leftIndexFinger.Hand.IsLeft) {
			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_W);
		}
		if (CheckPress (leftMiddleFinger.TipVelocity.y) && leftMiddleFinger.Hand.IsLeft) {
			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_Q);
		}

		if (CheckPress (rightIndexFinger.TipVelocity.y) && rightIndexFinger.Hand.IsRight) {
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

	private bool CheckPress(float velocity){
		return velocity < -250;
	}

	private bool CheckRelease(float velocity){
		return velocity > 50;
	}


}
