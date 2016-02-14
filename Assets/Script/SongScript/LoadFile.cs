using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Leap;

public class LoadFile : MonoBehaviour {

	[System.Serializable]
	public class Mtemplate
	{
		public string M_Name;
		public AudioClip M_Music;
		public Texture2D M_Texture;
		public TextAsset M_Text;
	}

	public enum Layers
	{
		NORMAL_LAYER = 0,
		SPEED_LAYER = 1,
		DIFFICULTY_LAYER = 2
	};


	private Leap.Controller controller;
	public float offset;
	private float timeDelay;
	private bool isFlicking;
	private float flickDelay = 0.4f;
	private bool isGrab;

	public List <Mtemplate> Mtems = new List<Mtemplate> ();
	private List <GameObject> buttonList = new List<GameObject> ();
	private List <SampleButton> sampleButonList = new List<SampleButton> ();
	private List <string> descriptionList = new List<string> ();
	private List <string> nameList = new List<string> ();
	public string Root_Path;

	public GameObject sampleButton;
	public Transform contentPanel;
	private int indexColorChange = 0;

	public Transform LoadingBar;
	public Transform TextLevel;
	public Transform Easy_TextLevel;
	public Transform Normal_TextLevel;
	public Transform Hard_TextLevel;
	public Transform Easy_NumLevel;
	public Transform Normal_NumLevel;
	public Transform Hard_NumLevel;
	public Transform Easy_LoadingBar;
	public Transform Normal_LoadingBar;
	public Transform Hard_LoadingBar;
	[SerializeField] private float currentAmount;
	[SerializeField] private float progressBarSpeed;


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

	private Layers layerState;

	public void Awake ()
	{
		GetFiles ();
	}

	public void Start(){
		layerState = Layers.NORMAL_LAYER;
		foreach (Mtemplate temp in Mtems) {
			buttonList.Add(CreateButton (temp));
		}

		//GlobalData.descriptionList = descriptionList;


//		GameObject _temp = Instantiate (Bullets, this.transform.position, Quaternion.identity) as GameObject;
//		//_temp.GetComponent<Renderer> ().material.color = new Color32 ((byte)Random.Range (0, 255), (byte)Random.Range (0, 255),(byte)Random.Range (0, 255), (byte)Random.Range (0, 255));
//		_temp.GetComponent<Renderer> ().material.color = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f),1f);

		if (buttonList.Count >= 1) {
//			Debug.Log (buttonList.Count+" = "+indexColorChange);
			buttonList [indexColorChange].GetComponent<UnityEngine.UI.Image> ().color = new Color (103/255f, (172/255f), 1f,1f);
//			buttonList [indexColorChange].GetComponent<Image> ().color = Color.blue;
//			if (indexColorChange != 0) {
//				buttonList [indexColorChange - 1].GetComponent<Image> ().color = new Color (255, 255, 255);
//			}
		}

		contentPanel.localPosition = new Vector3 (contentPanel.localPosition.x + 200, contentPanel.localPosition.y);

		//leap motion
		controller = new Leap.Controller ();
		isFlicking = false;
		timeDelay = 0;
		offset = 5;
		isGrab = false;

		//song selection controller
		speedPanelPosition = speedPanel.position;
		speedPanel.position = Vector3.Slerp (speedPanel.position, destination,5);
		difficultyPanelPosition = difficultyPanel.position;
		difficultyPanel.position = Vector3.Slerp (speedPanel.position, destination,5);
	}

	void Update(){
		Frame frame = controller.Frame ();
		HandList hands = frame.Hands;

		MapGesture (hands);

	}
	public void selectedListDown(){
		if (buttonList.Count >= 1) {
			++indexColorChange;
			if (indexColorChange < buttonList.Count) {
				buttonList [indexColorChange].GetComponent<UnityEngine.UI.Image> ().color = new Color (103 / 255f, (172 / 255f), 1f, 1f);
				buttonList [indexColorChange - 1].GetComponent<UnityEngine.UI.Image> ().color = new Color (1, 1, 1, 1);

				contentPanel.localPosition = new Vector3 (contentPanel.localPosition.x - 200, contentPanel.localPosition.y);
//				buttonList.RemoveAt (indexColorChange - 1);
//				sampleButonList.RemoveAt (indexColorChange - 1);
//				contentPanel.
			} else {
				--indexColorChange;
			}
		

		}
	}

	public void selectedListUp(){
		if (buttonList.Count >= 1) {
			--indexColorChange;
			if (indexColorChange >= 0) {
				buttonList [indexColorChange].GetComponent<UnityEngine.UI.Image> ().color = new Color (103 / 255f, (172 / 255f), 1f, 1f);
				buttonList [indexColorChange + 1].GetComponent<UnityEngine.UI.Image> ().color = new Color (1, 1, 1, 1);
				contentPanel.localPosition = new Vector3 (contentPanel.localPosition.x + 200, contentPanel.localPosition.y);
			} else {
				++indexColorChange;
			}
		}
	}

	private void GetFiles ()
	{
		FileInfo[] Files = new DirectoryInfo (Application.dataPath + "/Resources/" + Root_Path).GetFiles ();

		foreach (FileInfo File in Files) {

			if (Path.GetExtension (File.FullName) == ".meta") {
				string FileName = (Path.GetFileNameWithoutExtension (File.Name));
				string Dir = Application.dataPath + "/Resources/" + Root_Path + "/" + FileName;

				Mtemplate temp = AddIt(new DirectoryInfo (Dir).GetFiles (), Root_Path + "/" + FileName, FileName);

				Mtems.Add (temp);
//				CreateButton (temp);
			}

		}
	}

	Mtemplate AddIt (FileInfo [] Files, string Root_Path, string fileName)
	{
		Mtemplate New_Tem = new Mtemplate ();

		foreach (FileInfo Item in Files) {
			New_Tem.M_Name = fileName;

			if (Path.GetExtension (Item.FullName) == ".mp3") {

				New_Tem.M_Music = (AudioClip) UnityEditor.AssetDatabase.LoadAssetAtPath ("Assets/Resources/" + Root_Path + "/" + Item.Name, typeof(AudioClip));

			} else if (Path.GetExtension (Item.FullName) == ".png") {

				New_Tem.M_Texture = (Texture2D) UnityEditor.AssetDatabase.LoadAssetAtPath ("Assets/Resources/" + Root_Path + "/" + Item.Name, typeof(Texture2D));
			} else if (Path.GetExtension (Item.FullName) == ".txt") {

				New_Tem.M_Text = (TextAsset) UnityEditor.AssetDatabase.LoadAssetAtPath ("Assets/Resources/" + Root_Path + "/" + Item.Name, typeof(TextAsset));
//				Debug.Log ("Read TXT = " + New_Tem.M_Text.text);
			} 
		}

		return New_Tem;
	}
		

	private GameObject CreateButton(Mtemplate temp){

//		foreach (Mtemplate temp in Mtems) {
			GameObject newButton = Instantiate (sampleButton) as GameObject;
			SampleButton button = newButton.GetComponent <SampleButton> ();

		if(temp.M_Text != null){
		string theWholeFileAsOneLongString = temp.M_Text.text;
		descriptionList.Add (theWholeFileAsOneLongString);
		List<string> eachLine = new List<string>();
		eachLine.AddRange(
			theWholeFileAsOneLongString.Split("\n"[0]) );
		button.nameLabel.text = temp.M_Name;
			nameList.Add (temp.M_Name);
		button.composer.text = eachLine [1];
		button.bpm.text = "BPM "+eachLine [2];
		}

//			Debug.Log ("///// " + temp.M_Name);


		button.icon.GetComponent<UnityEngine.UI.Image>().sprite = Sprite.Create(temp.M_Texture,new Rect(0, 0, 256,256), new Vector2(0, 0),100.0f);

//			button.button.onClick.AddListener (delegate {
////				Debug.Log(temp.M_Name);
//				ChangeSceen (temp.M_Name);
//			});
//		sampleButonList.Add (button);
		newButton.transform.SetParent (contentPanel);
		RectTransform rect = newButton.GetComponent<RectTransform> ();
		rect.localScale = new Vector3 (1, 1, 1);
		rect.localPosition = new Vector3 (rect.position.x, rect.position.y, 0);

//		}

		return newButton;
	}

	public void selectedSong(){
		GlobalData.songIndex = indexColorChange;
	}

//	public void ChangeSceen(string songName){
//		GlobalData.selectedTrack.songName = songName;
////		Debug.Log (songName);
//		UnityEngine.Application.LoadLevel("Gameplay");
//	}
//
	private void MapGesture(HandList hands){

		Hand rightHand = hands.Rightmost;
		if (!isGrab) {
			if (!isFlicking) {
				CheckFlick (hands);
			} else if (isFlicking) {
				timeDelay += Time.deltaTime;
			}
			if (timeDelay >= flickDelay) {
				timeDelay = 0;
				isFlicking = false;
			}
		}
		FillProgressBar (rightHand);
	}
	private void CheckFlick(HandList hands){
		Hand rightHand = hands.Rightmost;
		Hand leftHand = hands.Leftmost;

		float rightHandYaw = rightHand.Direction.Yaw * offset;
		float leftHandYaw = leftHand.Direction.Yaw * offset * -1;

		float speed = 120;

		if (layerState == Layers.NORMAL_LAYER) {
			if (rightHandYaw > 1.2f && rightHand.PalmVelocity.x > speed && rightHand.IsRight) {
				isFlicking = true;
				this.selectedListDown ();
			}
			if (leftHandYaw > 1.2f && leftHand.PalmVelocity.x < -speed && leftHand.IsLeft) {
				isFlicking = true;
				this.selectedListUp ();
			}
		} else if (layerState == Layers.SPEED_LAYER) {

			if (rightHandYaw > 1.2f && rightHand.PalmVelocity.x > speed && rightHand.IsRight) {
				isFlicking = true;
				this.speedUp ();
			}
			if (leftHandYaw > 1.2f && leftHand.PalmVelocity.x < -speed && leftHand.IsLeft) {
				isFlicking = true;
				this.speedDown ();
			}
		} else {

			if (rightHandYaw > 1.2f && rightHand.PalmVelocity.x > speed && rightHand.IsRight) {
				isFlicking = true;
				this.increaseLevel ();
				currentAmount = 0;
			}
			if (leftHandYaw > 1.2f && leftHand.PalmVelocity.x < -speed && leftHand.IsLeft) {
				isFlicking = true;
				this.decreaseLevel ();
				currentAmount = 0;
			}
			if (level == 1) {
				Easy_LoadingBar.gameObject.SetActive (true);
				Normal_LoadingBar.gameObject.SetActive (false);
				Hard_LoadingBar.gameObject.SetActive (false);
			} else if (level == 2) {
				Easy_LoadingBar.gameObject.SetActive (false);
				Normal_LoadingBar.gameObject.SetActive (true);
				Hard_LoadingBar.gameObject.SetActive (false);
			} else {
				Easy_LoadingBar.gameObject.SetActive (false);
				Normal_LoadingBar.gameObject.SetActive (false);
				Hard_LoadingBar.gameObject.SetActive (true);
			}
		}
	}

	private void FillProgressBar(Hand hand){
		if (currentAmount >= 100) {
			TextLevel.GetComponent<Text> ().text = "Done!";
			if (layerState == Layers.NORMAL_LAYER) {
				showSpeed (); 
			} else if (layerState == Layers.SPEED_LAYER) {
				showDifficulty ();
				string temp = descriptionList [indexColorChange];
				List<string> eachLine = new List<string>();
				eachLine.AddRange(
					temp.Split("\n"[0]) );
				Easy_NumLevel.GetComponent<Text>().text = "Lv. " + eachLine [3];

				Normal_NumLevel.GetComponent<Text>().text = "Lv. " + eachLine [4];

				Hard_NumLevel.GetComponent<Text>().text = "Lv. " + eachLine [5];
				
			} else {
				changeScene ();
			}

		} else if (hand.GrabStrength == 1) {
			isGrab = true;
			currentAmount += progressBarSpeed * Time.deltaTime;
			if (layerState == Layers.DIFFICULTY_LAYER) {
				if (level == 1) {
					Easy_TextLevel.GetComponent<Text> ().text = ((int)currentAmount).ToString() + "%";
				} else if (level == 2) {
					Normal_TextLevel.GetComponent<Text> ().text = ((int)currentAmount).ToString() + "%";
				} else {
					Hard_TextLevel.GetComponent<Text> ().text = ((int)currentAmount).ToString() + "%";
				}
			} else {
				TextLevel.GetComponent<Text> ().text = ((int)currentAmount).ToString () + "%";
			}
		} else {
			isGrab = false;
			if(currentAmount <= 0){
				if (layerState == Layers.DIFFICULTY_LAYER) {
					if (level == 1) {
						Easy_TextLevel.GetComponent<Text> ().text = "EASY";
						Normal_TextLevel.GetComponent<Text> ().text = "NORMAL";
						Hard_TextLevel.GetComponent<Text> ().text = "HARD";
					} else if (level == 2) {
						Easy_TextLevel.GetComponent<Text> ().text = "EASY";
						Normal_TextLevel.GetComponent<Text> ().text = "NORMAL";
						Hard_TextLevel.GetComponent<Text> ().text = "HARD";
					} else {
						Easy_TextLevel.GetComponent<Text> ().text = "EASY";
						Normal_TextLevel.GetComponent<Text> ().text = "NORMAL";
						Hard_TextLevel.GetComponent<Text> ().text = "HARD";
					}
				}
				
			} else if (currentAmount > 0) {
				if (layerState == Layers.DIFFICULTY_LAYER) {
					if (level == 1) {
						Easy_TextLevel.GetComponent<Text> ().text = ((int)currentAmount).ToString () + "%";
					} else if (level == 2) {
						Normal_TextLevel.GetComponent<Text> ().text = ((int)currentAmount).ToString () + "%";
					} else {
						Hard_TextLevel.GetComponent<Text> ().text = ((int)currentAmount).ToString () + "%";
					}
				} else {
					TextLevel.GetComponent<Text> ().text = ((int)currentAmount).ToString () + "%";
				}
				currentAmount -= progressBarSpeed * Time.deltaTime * 2;
			}
		}


		LoadingBar.GetComponent<UnityEngine.UI.Image> ().fillAmount = currentAmount / 100;

	}

	public void showSpeed (){
		layerState = Layers.SPEED_LAYER;
		currentAmount = 0;
		selectedSong ();
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
		layerState = Layers.DIFFICULTY_LAYER;
		currentAmount = 0;
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
		//List <string> descriptionList = new List<string> ();
		int index = GlobalData.songIndex;
		//descriptionList = GlobalData.descriptionList;
		Debug.Log ("dd : " + descriptionList.Count);
		string temp = descriptionList [index];
		List<string> eachLine = new List<string>();
		Difficulty difficult;
		eachLine.AddRange(temp.Split("\n"[0]) );
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

		Track track = new Track(nameList[index],eachLine[1], difficult,level, float.Parse(eachLine[2]));
		GlobalData.selectedTrack = track;

		GlobalData.speed = speed;

		UnityEngine.Application.LoadLevel("Gameplay");
	}
}
