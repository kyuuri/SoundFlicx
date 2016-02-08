using UnityEngine;
using System.Collections;
using Leap;

public class StartScript : MonoBehaviour {
	Leap.Controller controller;
	// Use this for initialization
	void Start () {
		controller = new Leap.Controller ();
		controller.EnableGesture (Gesture.GestureType.TYPESWIPE);
	}

	// Update is called once per frame
	void Update () {
		Leap.Frame frame = controller.Frame ();
		GestureList gestures = frame.Gestures();

		for (int i = 0; i < gestures.Count; i++) {
			if (gestures [i].Type == Gesture.GestureType.TYPE_SWIPE) {
				ChangeScene ();
			}
		}

	}

	public void ChangeScene(){
		Debug.Log("Change Sceen");
		Application.LoadLevel("SongSelection");
	}

	//	void OnGUI(){


	//		if(GUI.Button(new Rect( PercentWidth(10), PercentHeight(5), 100, 20), "Display 10")){
	//			Debug.Log ("On GUI 1");
	////			textMesh.text = "10";
	//
	//		}

	//		if (GUI.Button (new Rect (10, 90, 100, 20), button)) {
	//			Debug.Log ("On GUI 2");
	////			textMesh.text = "5";
	//
	//		}

	//	}

	float PercentWidth(float width){
		return (width / 100) * UnityEngine.Screen.width;
	}

	float PercentHeight(float height){
		return (height / 100) * UnityEngine.Screen.height;
	}
}