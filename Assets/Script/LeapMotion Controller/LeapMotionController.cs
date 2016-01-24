using UnityEngine;
using System.Collections;
using Leap;
using WindowsInput;

public class LeapMotionController : MonoBehaviour {
	private Leap.Controller controller;
	private Vector[] previousFingers;
	private Vector[] currentFingers;

	public enum State{
		UP,
		DOWN
	}

	public State state;

	// Use this for initialization
	void Start () {
		Application.runInBackground = true;
		controller = new Leap.Controller ();
		state = State.UP;
		previousFingers = new Vector[4];
		currentFingers = new Vector[4];


		previousFingers [0] = controller.Frame().Hands.Leftmost.Fingers.FingerType(Finger.FingerType.TYPE_MIDDLE)[0].StabilizedTipPosition;
		previousFingers [1] = controller.Frame().Hands.Leftmost.Fingers.FingerType(Finger.FingerType.TYPE_INDEX)[0].StabilizedTipPosition;
		previousFingers [2] = controller.Frame().Hands.Rightmost.Fingers.FingerType(Finger.FingerType.TYPE_INDEX)[0].StabilizedTipPosition;
		previousFingers [3] = controller.Frame().Hands.Rightmost.Fingers.FingerType(Finger.FingerType.TYPE_MIDDLE)[0].StabilizedTipPosition;
	}

	// Update is called once per frame
	void Update (){
		Leap.Frame frame = controller.Frame ();
		HandList hands = frame.Hands;
		Hand rightHand = hands.Rightmost;
		Hand leftHand = hands.Leftmost;

		Finger rightForeFinger = rightHand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_INDEX) [0];
		Finger rightMiddleFinger = rightHand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_MIDDLE) [0];

		Finger leftForeFinger = leftHand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_INDEX) [0];
		Finger leftMiddleFinger = leftHand.Fingers.FingerType (Leap.Finger.FingerType.TYPE_MIDDLE) [0];

		currentFingers [0] = leftMiddleFinger.StabilizedTipPosition;
		currentFingers [1] = leftForeFinger.StabilizedTipPosition;
		currentFingers [2] = rightForeFinger.StabilizedTipPosition;
		currentFingers [3] = rightMiddleFinger.StabilizedTipPosition;

		//CheckFingersPressed (currentFingers);
		float deltaPos = 40;
		if (previousFingers [0].y - currentFingers[0].y > deltaPos) {
			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_Q);
			//Debug.Log ("Q press");
			previousFingers [0] = currentFingers [0];
		} else if (previousFingers [1].y - currentFingers[1].y > deltaPos) {
			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_W);
			//Debug.Log ("W press");
			previousFingers [1] = currentFingers [1];
		} else if (previousFingers [2].y - currentFingers[2].y > deltaPos) {
			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_E);
			//Debug.Log ("E press");
			previousFingers [2] = currentFingers [2];
		} else if (previousFingers [3].y - currentFingers[3].y > deltaPos) {
			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_R);
			//Debug.Log ("R press");
			previousFingers [3] = currentFingers [3];
		}


		else if (previousFingers [0].y - currentFingers[0].y < -deltaPos) {
			//Debug.Log ("Q unpress");
			InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_Q);
			previousFingers [0] = currentFingers [0];
		} else if (previousFingers [1].y - currentFingers[1].y < -deltaPos) {
			//Debug.Log ("W unpress");
			InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
			previousFingers [1] = currentFingers [1];
		} else if (previousFingers [2].y - currentFingers[2].y < -deltaPos) {
			//Debug.Log ("E unpress");
			InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_E);
			previousFingers [2] = currentFingers [2];
		} else if (previousFingers [3].y - currentFingers[3].y < -deltaPos) {
			//Debug.Log ("R unpress");
			InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_R);
			previousFingers [3] = currentFingers [3];
		} 

		//previousFingers [0] = leftMiddleFinger.StabilizedTipPosition;
		//previousFingers [1] = leftForeFinger.StabilizedTipPosition;
		//previousFingers [2] = rightForeFinger.StabilizedTipPosition;
		//previousFingers [3] = rightMiddleFinger.StabilizedTipPosition;

		//CheckFingersPress (rightForeFinger, rightMiddleFinger, leftForeFinger, leftMiddleFinger);
		//		testGestures (frame);
	}

	private void CheckFingersPress(Finger rightForeFinger, Finger rightMiddleFinger, Finger leftForeFinger, Finger leftMiddleFinger){
		float pressSpeed = -150;
		if (leftMiddleFinger.TipVelocity.y < pressSpeed && state == State.UP) {
			//Debug.Log (finger.TipVelocity);
			//state = State.DOWN;
			InputSimulator.SimulateKeyPress (VirtualKeyCode.VK_Q);
			Debug.Log ("Q press");
			state = State.DOWN;

		} else if (leftForeFinger.TipVelocity.y < pressSpeed && state == State.UP) {
			//Debug.Log (finger.TipVelocity);
			state = State.DOWN;
			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_W);
			if (Input.GetKeyDown (KeyCode.W))
				Debug.Log ("W pressed");

		} else if (rightForeFinger.TipVelocity.y < pressSpeed && state == State.UP) {
			//Debug.Log (finger.TipVelocity);
			state = State.DOWN;
			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_E);
			if (Input.GetKeyDown (KeyCode.E))
				Debug.Log ("E pressed");

		} else if (rightMiddleFinger.TipVelocity.y < pressSpeed && state == State.UP) {
			//Debug.Log (finger.TipVelocity);
			state = State.DOWN;
			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_R);
			if (Input.GetKeyDown (KeyCode.R))
				Debug.Log ("R pressed");
		} else 
			state = State.UP;
	}
}
