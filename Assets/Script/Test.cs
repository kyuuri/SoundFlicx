using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Test : MonoBehaviour {

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

	public void Awake ()
	{
		GetFiles ();
	}

	private void GetFiles ()
	{
		FileInfo[] Files = new DirectoryInfo (Application.dataPath + "/" + Root_Path).GetFiles ();

		foreach (FileInfo File in Files) {

			Debug.Log (Path.GetExtension (File.FullName));

			if (Path.GetExtension (File.FullName) == ".meta") {
				string Dir = Application.dataPath + "/" + Root_Path + "/" + Path.GetFileNameWithoutExtension (File.Name);

				Mtems.Add (AddIt (new DirectoryInfo (Dir).GetFiles (), Root_Path + "/" + Path.GetFileNameWithoutExtension (File.Name)));


			}
		}
	}

	Mtemplate AddIt (FileInfo [] Files, string Root_Path)
	{
		Mtemplate New_Tem = new Mtemplate ();

		foreach (FileInfo Item in Files) {

			if (Path.GetExtension (Item.FullName) == ".mp3") {

				New_Tem.M_Music = (AudioClip) UnityEditor.AssetDatabase.LoadAssetAtPath ("Assets/" + Root_Path + "/" + Item.Name, typeof(AudioClip));

			} else if (Path.GetExtension (Item.FullName) == ".png") {

				New_Tem.M_Texture = (Texture2D) UnityEditor.AssetDatabase.LoadAssetAtPath ("Assets/" + Root_Path + "/" + Item.Name, typeof(Texture2D));
			} 
		}

		return New_Tem;
	}
}
