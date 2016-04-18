using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class TypeController : MonoBehaviour {

	public Text text;
	private PlayerInfo[] players;
	private LeaderBoard leaderBoard;
	private Track track;
	private string trackName;
	private ResultScore result;
//	private float[] testData = new float[10] {100, 90, 80, 50, 40, 30, 20, 18, 15, 10};
	private PlayerInfo[] copyPlayers;
	private PlayerInfo newPlayer;
	public float score;

	void Awake(){
		System.Environment.SetEnvironmentVariable ("MONO_REFLECTION_SERIALIZER", "yes");
	}

	// Use this for initialization
	void Start () {
		text.text = "";
		track = GlobalData.selectedTrack;
		result = GlobalData.result;
		Load ();

		newPlayer = new PlayerInfo ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
//			InsertScore ();
			Print(players);
		}
//		if (Input.GetKeyDown (KeyCode.S)) {
//			string arr = "";
//			for (int i = 0; i < 10; i++) {
//				arr += testData [i] + " ";
//			}
//			Debug.Log (arr);
//		}
	}

	public void AddText (string label){
		text.text = text.text + label;
	}

	public void DeleteLabel(){
		Debug.Log (text.text.Length);
		if(text.text.Length>0)
			text.text = text.text.Remove (text.text.Length-1);
	}

	public void InsertScore(){
		int index;
//		PlayerInfo[] copyPlayers = players.Clone() as PlayerInfo[];
//		float[] copyData = testData.Clone() as float[];
		ClonePlayers ();
//		Print(copyPlayers);

		for (int i = 0; i < 10; i++) {
			if (result.score > players [i].score) {
//			if(score > testData[i]){
				index = i;
				int j = i;
				while (j + 1 < 10) {
					players [j + 1].name = copyPlayers[j].name;
					players [j + 1].score = copyPlayers[j].score;
					players [j + 1].maxCombo = copyPlayers[j].maxCombo;
					players [j + 1].accuracy = copyPlayers[j].accuracy;
					players [j + 1].ranking = copyPlayers[j].ranking;
//					Print (players);
//					testData[j+1] = copyData[j];
//					string kok = "";
//					for(int k = 0;k < 10; k++){
//						kok += copyData [k] +" ";
//					}
//					Debug.Log (kok);
					j++;
				}
			}
			//				testData [i] = score;
			players[i].name = newPlayer.name;
			players[i].score = newPlayer.score;
			players[i].maxCombo = newPlayer.maxCombo;
			players[i].accuracy = newPlayer.accuracy;
			players[i].ranking = newPlayer.ranking;
			//				Debug.Log (players [i].name + ", " + players [i].score + ", " + players [i].maxCombo + ", " + players [i].accuracy + ", " + players [i].ranking);
			break;
		}
	}

	public void ChangeScene(){
		newPlayer.name = text.text;
		newPlayer.score = result.score;
		newPlayer.ranking = result.getRank();
		newPlayer.accuracy = result.getAccuracy ();
		newPlayer.maxCombo = result.maxCombo;
		InsertScore ();
		Save ();
		UnityEngine.Application.LoadLevel ("Ranking");
	}

	public void Save () {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + ("/" + track.songName));

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

	private void ClonePlayers(){
		copyPlayers = new PlayerInfo[10];
		for(int i = 0; i < players.Length; i++){
			copyPlayers[i] = new PlayerInfo();

			copyPlayers [i].name = players [i].name;
			copyPlayers [i].score = players [i].score;
			copyPlayers [i].ranking = players [i].ranking;
			copyPlayers [i].maxCombo = players [i].maxCombo;
			copyPlayers [i].accuracy = players [i].accuracy;
		}
	}

	private void Print(PlayerInfo[] players){
		string a = "";
		for (int i = 0; i < players.Length; i++) {
			a += players [i].score + ", ";			
		} 
		Debug.Log (a);
	}
}
