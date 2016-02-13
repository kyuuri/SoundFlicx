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

	private bool isRflicking = false;
	private bool isLflicking = false;

	private float flickDelay = 0.1f;

	private float RDelay = 0;
	private float LDelay = 0;

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
			InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_Z);
			InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_X);
		}

		if (hands.Count == 1) {
			if (hands.Leftmost.IsLeft) {
				InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_E);
				InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_R);
				InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_X);
			} else {
				InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_Q);
				InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
				InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_Z);
			}
		}

		if (TimerScript.timePass > -1) {
			CheckFlick (hands);

			if (isLflicking) {
				LDelay += Time.deltaTime;
				InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_Q);
				InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_W);
			}
			if (isRflicking) {
				RDelay += Time.deltaTime;
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
				InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_Q);
				InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_Q);
			}
			if (CheckPressingDistance (leftHand, previousLeftHand, 1, 0) && leftHand.IsLeft) {
				InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_W);
				InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_W);
			}
		}
		if (!isRflicking) {
			if (CheckPressingDistance (rightHand, previousRightHand, 2, 0) && rightHand.IsRight) {
				InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_E);
				InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_E);
			}
			if (CheckPressingDistance (rightHand, previousRightHand, 3, 0) && rightHand.IsRight) {
				InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_R);
				InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_R);
			}
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
		float speed = 155;

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

		float speed = 120;

		InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_X);
		InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_Z);

		//		if (rightHandYaw > 1.5 && rightHand.PalmVelocity.x > speed) {
		if (rightHandYaw > 1.2f && rightHand.PalmVelocity.x > speed && rightHand.IsRight) {
			//Debug.Log ("right swipe");
			//			Debug.Log(rightHandYaw);
			InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_X);
			isRflicking = true;
			LDelay = 0;
		}
		//		if (leftHandYaw > 1.5  && leftHand.PalmVelocity.x < -speed) {
		if (leftHandYaw > 1.2f  && leftHand.PalmVelocity.x < -speed && leftHand.IsLeft) {
			//Debug.Log ("left swipe");
			//			Debug.Log(leftHandYaw);
			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_Z);
			isLflicking = true;
			RDelay = 0;
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
}
