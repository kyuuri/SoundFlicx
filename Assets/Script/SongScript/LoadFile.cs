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


	private Leap.Controller controller;
	public float offset;
	private float timeDelay;
	private bool isFlicking;
	private float flickDelay = 0.5f;

	public List <Mtemplate> Mtems = new List<Mtemplate> ();
	private List <GameObject> buttonList = new List<GameObject> ();
	private List <SampleButton> sampleButonList = new List<SampleButton> ();
	public string Root_Path;

	public GameObject sampleButton;
	public Transform contentPanel;
	private int indexColorChange = 0;

	public void Awake ()
	{
		GetFiles ();
	}

	public void Start(){
		foreach (Mtemplate temp in Mtems) {
			buttonList.Add(CreateButton (temp));
		}

//		GameObject _temp = Instantiate (Bullets, this.transform.position, Quaternion.identity) as GameObject;
//		//_temp.GetComponent<Renderer> ().material.color = new Color32 ((byte)Random.Range (0, 255), (byte)Random.Range (0, 255),(byte)Random.Range (0, 255), (byte)Random.Range (0, 255));
//		_temp.GetComponent<Renderer> ().material.color = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f),1f);

		if (buttonList.Count >= 1) {
			Debug.Log (buttonList.Count+" = "+indexColorChange);
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


	}

	void Update(){
		Frame frame = controller.Frame ();
		HandList hands = frame.Hands;

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
	public void selectedListDown(){
		Debug.Log (sampleButonList.Count);
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

		List<string> eachLine = new List<string>();
		eachLine.AddRange(
			theWholeFileAsOneLongString.Split("\n"[0]) );
		Debug.Log (eachLine.Count);
		button.nameLabel.text = temp.M_Name;
		button.composer.text = eachLine [1];
		button.bpm.text = "BPM "+eachLine [2];
		}

//			Debug.Log ("///// " + temp.M_Name);


		button.icon.GetComponent<UnityEngine.UI.Image>().sprite = Sprite.Create(temp.M_Texture,new Rect(0, 0, 256,256), new Vector2(0, 0),100.0f);

			button.button.onClick.AddListener (delegate {
//				Debug.Log(temp.M_Name);
				ChangeSceen (temp.M_Name);
			});
//		sampleButonList.Add (button);
		newButton.transform.SetParent (contentPanel);
		RectTransform rect = newButton.GetComponent<RectTransform> ();
		rect.localScale = new Vector3 (1, 1, 1);
		rect.localPosition = new Vector3 (rect.position.x, rect.position.y, 0);

//		}

		return newButton;
	}

	public void ChangeSceen(string songName){
		GlobalData.selectedTrack.songName = songName;
//		Debug.Log (songName);
		UnityEngine.Application.LoadLevel("Gameplay");
	}

	private void CheckFlick(HandList hands){
		Hand rightHand = hands.Rightmost;
		Hand leftHand = hands.Leftmost;

		float rightHandYaw = rightHand.Direction.Yaw * offset;
		float leftHandYaw = leftHand.Direction.Yaw * offset * -1;

		float speed = 120;

		//		if (rightHandYaw > 1.5 && rightHand.PalmVelocity.x > speed) {
		if (rightHandYaw > 1.2f && rightHand.PalmVelocity.x > speed && rightHand.IsRight) {
			//			Debug.Log ("right swipe");
			isFlicking = true;
			this.selectedListDown();

		}
		//		if (leftHandYaw > 1.5  && leftHand.PalmVelocity.x < -speed) {
		if (leftHandYaw > 1.2f  && leftHand.PalmVelocity.x < -speed && leftHand.IsLeft) {
			//			Debug.Log ("left swipe");
			isFlicking = true;
			this.selectedListUp();
		}
	}
}
