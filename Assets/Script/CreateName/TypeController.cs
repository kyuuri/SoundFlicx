using UnityEngine;
using Leap;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
//using System.Runtime.InteropServices;

public class TypeController : MonoBehaviour {
//	[DllImport("user32.dll")]
//	public static extern bool SetCursorPos(int X, int Y);
//	[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
//	public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

	public Leap.Controller controller;

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
	private string fileName;
	public AudioSource sound;
//	private bool isSelected = false;

	void Awake(){
		System.Environment.SetEnvironmentVariable ("MONO_REFLECTION_SERIALIZER", "yes");
	}

	// Use this for initialization
	void Start () {
		controller = new Leap.Controller ();

		text.text = "";
		track = GlobalData.selectedTrack;
		result = GlobalData.result;
		fileName = "/" + track.songName + "_" + track.difficulty;
		Load ();

		newPlayer = new PlayerInfo ();
	}
	
	void Update () {
//		text.text += Input.inputString;

	// Update is called once per frame
//		Frame frame = controller.Frame ();
//		Hand hand = frame.Hands.Rightmost;

//		Vector position = hand.Fingers.FingerType(Finger.FingerType.TYPE_INDEX)[0].StabilizedTipPosition;
//		Vector position = hand.Fingers.FingerType(Finger.FingerType.TYPE_INDEX)[0].Bone(Bone.BoneType.TYPE_METACARPAL).NextJoint;
//		float screenWidth = UnityEngine.Screen.width;
//		float screenHeight = UnityEngine.Screen.height;

//		int posX = (int)(position.x * 4 + screenWidth / 2);
//		int posY = (int)(-position.y + screenHeight / 2) * 3;

//		int posX = (int)(position.x * 4 + screenWidth / 2);
//		int posY = (int)(-position.y + screenHeight / 2) * 4;
//		SetCursorPos(posX,posY);

//		if (hand.IsValid) {
//			if (hand.PinchStrength > 0.9f && !isSelected) {
//				isSelected = true;
//				mouse_event (0x0002 | 0x0004, 0, posX, posY, 0);
//			} else if(hand.PinchStrength < 0.5f) {
//				isSelected = false;
//			}
//		}

//		if (Input.GetKeyDown (KeyCode.Space)) {
//			//			InsertScore ();
//			Print(players);
//		}
	}

	public void AddText (string label){
		sound.Play ();
		text.text = text.text + label;
	}

	public void DeleteLabel(){
		sound.Play ();
		if(text.text.Length>0)
			text.text = text.text.Remove (text.text.Length-1);
	}

	public void ClearText(){
		sound.Play ();
		text.text = "";
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
				players[i].name = newPlayer.name;
				players[i].score = newPlayer.score;
				players[i].maxCombo = newPlayer.maxCombo;
				players[i].accuracy = newPlayer.accuracy;
				players[i].ranking = newPlayer.ranking;
				//				Debug.Log (players [i].name + ", " + players [i].score + ", " + players [i].maxCombo + ", " + players [i].accuracy + ", " + players [i].ranking);
				break;
			}
			//				testData [i] = score;
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
		FileStream file = File.Create (Application.persistentDataPath + fileName);

		LeaderBoard data = new LeaderBoard ();
		data.players = players;

		//		data.health = health;
		//		data.experience = experience;

		bf.Serialize (file, data);
		file.Close ();
	}

	public void Load(){
		if (File.Exists (Application.persistentDataPath + fileName)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + fileName, FileMode.Open);
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
