using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class RankingController : MonoBehaviour {

	private PlayerInfo[] players;
	private LeaderBoard leaderBoard;
	private Track track;

	void Awake(){
		System.Environment.SetEnvironmentVariable ("MONO_REFLECTION_SERIALIZER", "yes");
	}

	// Use this for initialization
	void Start () {
		track = GlobalData.selectedTrack;
		Load ();

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
