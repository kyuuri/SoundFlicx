﻿using UnityEngine;
using System.Collections;

public class StartScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeScence(){
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
		return (width / 100) * Screen.width;
	}

	float PercentHeight(float height){
		return (height / 100) * Screen.height;
	}
}
