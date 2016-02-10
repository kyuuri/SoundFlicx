using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LoadFile : MonoBehaviour {

	[System.Serializable]
	public class Mtemplate
	{
		public string M_Name;
		public AudioClip M_Music;
		public Texture2D M_Texture;
	}

	public List <Mtemplate> Mtems = new List<Mtemplate> ();
	public string Root_Path;

	public GameObject sampleButton;
	public Transform contentPanel;

	public void Start ()
	{
		GetFiles ();
	}

	private void GetFiles ()
	{
		FileInfo[] Files = new DirectoryInfo (Application.dataPath + "/Resources/" + Root_Path).GetFiles ();

		foreach (FileInfo File in Files) {

			if (Path.GetExtension (File.FullName) == ".meta") {
				string FileName = (Path.GetFileNameWithoutExtension (File.Name));
				string Dir = Application.dataPath + "/Resources/" + Root_Path + "/" + FileName;

				Mtemplate temp = AddIt(new DirectoryInfo (Dir).GetFiles (), Root_Path + "/" + FileName, FileName);


//				Mtems.Add (temp);
				CreateButton (temp);
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
			} 
		}

		return New_Tem;
	}
		

	private void CreateButton(Mtemplate temp){
		GameObject newButton = Instantiate (sampleButton) as GameObject;
		SampleButton button = newButton.GetComponent <SampleButton> ();
		button.nameLabel.text = temp.M_Name;
		button.icon = temp.M_Texture;

		button.button.onClick.AddListener (delegate {
			ChangeSceen (temp.M_Name);
		});
		newButton.transform.SetParent (contentPanel);
	}

	public void ChangeSceen(string songName){
		GlobalData.SelectedSong = songName;
	//	Debug.Log (songName);
		Application.LoadLevel("Gameplay");
	}
}
