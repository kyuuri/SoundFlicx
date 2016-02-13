using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SongSelectionController : MonoBehaviour {

	public RectTransform panel;
	public RectTransform speedPanel;
	public RectTransform difficultyPanel;


	public Text speedNumber;
	private float speed = 2.0f;

	public Transform buttonLeft;
	public Transform buttonRight;
	private Vector3 destination = new Vector3(1000,1000,1000);
	private Vector3 speedPanelPosition;
	private Vector3 difficultyPanelPosition;

	// 1 easy 2 normal 3 hard
	private int level = 2;

	// Use this for initialization
	void Start () {
		speedPanelPosition = speedPanel.position;
		speedPanel.position = Vector3.Slerp (speedPanel.position, destination,5);
		difficultyPanelPosition = difficultyPanel.position;
		difficultyPanel.position = Vector3.Slerp (speedPanel.position, destination,5);
	}
	
	public void showSpeed (){
		speedPanel.position = Vector3.Slerp(destination, speedPanelPosition, 5);

	}

	public void speedUp(){
		if (speed <= 3.5) {
			speed += 0.5f;
			speedNumber.text = string.Format("{0:0.0}", speed);
		}
	}

	public void speedDown(){
		if (speed >= 1) {
			speed -= 0.5f;

			speedNumber.text = string.Format("{0:0.0}", speed);
		}
	}

	public void backFromSpeed(){
		speedPanel.position = Vector3.Slerp (speedPanel.position, destination,5);
	}

	public void showDifficulty(){
		speedPanel.position = Vector3.Slerp (speedPanel.position, destination,5);
		difficultyPanel.position = Vector3.Slerp(destination, difficultyPanelPosition, 5);
	}

	public void backFromDifficulty(){
		difficultyPanel.position = Vector3.Slerp (speedPanel.position, destination,5);
	}

	public int getLevel(){
		return level;
	}

	public void increaseLevel(){
		if (level < 3)
			++level;	
	}

	public void decreaseLevel(){
		if (level > 1) {
			--level;
		}
	}

	public void changeScene(){
		List <string> descriptionList = new List<string> ();
		int index = GlobalData.songIndex;
		descriptionList = GlobalData.descriptionList;
		string temp = descriptionList [index];
		List<string> eachLine = new List<string>();
		Difficulty difficult;
		eachLine.AddRange(
			temp.Split("\n"[0]) );
		if (level == 1) {
			difficult = Difficulty.EASY;
			level = int.Parse (eachLine [3]);
		} else if (level == 2) {
			difficult = Difficulty.NORMAL;
			level = int.Parse (eachLine [4]);
		} else {
			difficult = Difficulty.HARD;
			level = int.Parse (eachLine [5]);
		}
		Track track = new Track(eachLine[0],eachLine[1], difficult,level, float.Parse(eachLine[2]));
		track.offset = float.Parse (eachLine[6]);
		GlobalData.selectedTrack = track;

		UnityEngine.Application.LoadLevel("Gameplay");
	}


}
