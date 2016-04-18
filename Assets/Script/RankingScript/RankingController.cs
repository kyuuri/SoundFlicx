using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class RankingController : MonoBehaviour {

	private PlayerInfo[] players;
	private LeaderBoard leaderBoard;
	private Track track;
	public Transform rankPanel;
	public GameObject rankLine;

	void Awake(){
		System.Environment.SetEnvironmentVariable ("MONO_REFLECTION_SERIALIZER", "yes");
	}

	// Use this for initialization
	void Start () {
		track = GlobalData.selectedTrack;
		Load ();
		if (players != null) {
			for (int i = 0; i < 10; i++) {
				createRank (players [i]);
			}
		} else {
			PlayerInfo test = new PlayerInfo ();
			test.name = "TEST";
			test.score = 99999f;
			test.ranking = "SAD";
			createRank (test);
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			string text = "";
			for (int i = 0; i < 10; i++) {
				text += players [i].score + ", ";
			}
			Debug.Log (text);
//			Debug.Log(players);
		}
	}

	private void createRank(PlayerInfo player){
		GameObject newRank = Instantiate (rankLine) as GameObject;
		RankLine rank = newRank.GetComponent<RankLine> ();
		rank.name.text = player.name;
		Debug.Log (player.name);
		rank.ranking.text = player.ranking;
		rank.score.text = player.score+"";
		newRank.transform.SetParent (rankPanel);
	}
//	private GameObject CreateButton(Mtemplate temp, string text){
//
//		GameObject newButton = Instantiate (sampleButton) as GameObject;
//		SampleButton button = newButton.GetComponent <SampleButton> ();
//
//		descriptionList.Add (text);
//		List<string> eachLine = new List<string>();
//
//		eachLine.AddRange(text.Split(","[0]) );
//		button.nameLabel.text = temp.M_Name.Replace("_"," ");
//		nameList.Add (eachLine [0]);
//		button.composer.text = eachLine [1];
//		button.bpm.text = "BPM : "+eachLine [2];
//		//Debug.Log (eachLine [0]);
//		button.icon.GetComponent<UnityEngine.UI.Image> ().sprite = Sprite.Create(Resources.Load ("Tracks/" + eachLine [0] + "Image") as Texture2D,new Rect(0, 0, 256,256), new Vector2(0, 0),100.0f);
//		newButton.transform.SetParent (contentPanel);
//		RectTransform rect = newButton.GetComponent<RectTransform> ();
//		rect.localScale = new Vector3 (0.75f, 0.75f, 1);
//		rect.localPosition = new Vector3 (rect.position.x, rect.position.y, 0);
//
//		return newButton;
//	}
		
	public void Save () {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + ("/"+track.songName));

		LeaderBoard data = new LeaderBoard ();
		data.players = players;

//		data.health = health;
//		data.experience = experience;

		bf.Serialize (file, data);
		file.Close ();
	}

	public void Load(){
		if (File.Exists (Application.persistentDataPath + ("/" + track.songName))) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + ("/" + track.songName), FileMode.Open);
			LeaderBoard data = (LeaderBoard)bf.Deserialize (file);
			file.Close ();

			players = data.players;

//			health = data.health;
//			experience = data.experience;
		}
	}
}
