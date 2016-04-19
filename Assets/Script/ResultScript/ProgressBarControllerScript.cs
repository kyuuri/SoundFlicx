using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Leap;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class ProgressBarControllerScript : MonoBehaviour {

	public AudioSource confirm;
	public AudioSource selectB;

	public UnityEngine.UI.Image loadingPicture;
	public GameObject particle;
	public GameObject leapCamera;

	private bool next = false;
	private float delay = 0;
	private string str = "SongSelection";

	private Leap.Controller controller;
	public Transform Done_LoadingBar;
	public Transform Retry_LoadingBar;
	public Transform Done_LoadingBar_Background;
	public Transform Retry_LoadingBar_Background;
	private Status_State state;
	private float timeDelay;
	private bool isFlicking;
	private float flickDelay = 0.4f;

	private ResultScore resultScore;
	private Track track;
	private PlayerInfo[] players;
	private string fileName;

	[SerializeField] private float doneButton_Amount;
	[SerializeField] private float retryButton_Amount;
	[SerializeField] private float progressBarSpeed;

	private enum Status_State
	{
		DONE = 0,
		RETRY = 1
	};

	// Use this for initialization
	void Awake(){
		System.Environment.SetEnvironmentVariable ("MONO_REFLECTION_SERIALIZER", "yes");
	}

	// Use this for initialization
	void Start () {
		resultScore = GlobalData.result;
		track = GlobalData.selectedTrack;
		fileName = "/" + track.songName;
		controller = new Leap.Controller ();
		particle.active = true;
		leapCamera.active = true;
		state = Status_State.DONE;
		Done_LoadingBar.gameObject.SetActive (true);
		Done_LoadingBar_Background.gameObject.SetActive (true);
		Retry_LoadingBar.gameObject.SetActive (false);
		Retry_LoadingBar_Background.gameObject.SetActive (false);

		next = false;
		delay = 0;

		Load ();
	}

	// Update is called once per frame
	void Update () {
		Frame frame = controller.Frame ();
		HandList hands = frame.Hands;
		Hand hand = hands.Rightmost;

		if (!isFlicking) {
			CheckFlick (hands);
		} else if (isFlicking) {
			timeDelay += Time.deltaTime;
		}
		if (timeDelay >= flickDelay) {
			timeDelay = 0;
			isFlicking = false;
		}
		if (state == Status_State.DONE) {
			FillDoneProgressBar (hand);
		} else {
			FillRetryProgressBar (hand);
		}

		if (next) {
			delay += Time.deltaTime;
		}

		if (delay >= 0.4f) {
			particle.active = false;
			leapCamera.active = false;
			loadingPicture.gameObject.active = true;
			UnityEngine.Application.LoadLevel (str);
		}
	}

	private void FillDoneProgressBar(Hand hand){

		if (doneButton_Amount >= 100) {
			if (!confirm.isPlaying) {
				confirm.Play ();
			}
			next = true;

			if (resultScore.score > players [9].score) {
				str = "Layout Groups";
			} else {
				str = "SongSelection"; 
			}

		} else if (hand.GrabStrength == 1 || Input.GetKey(KeyCode.Space)) {
			doneButton_Amount += progressBarSpeed * Time.deltaTime;
		} else {
			if(doneButton_Amount > 0)
				doneButton_Amount -= progressBarSpeed * Time.deltaTime*2;
		}
		Done_LoadingBar.GetComponent<UnityEngine.UI.Image> ().fillAmount = doneButton_Amount / 100;
	}

	private void FillRetryProgressBar(Hand hand){

		if (retryButton_Amount >= 100) {
			if (!confirm.isPlaying) {
				confirm.Play ();
			}
			next = true;
			str = "Gameplay";
		} else if (hand.GrabStrength == 1 || Input.GetKeyDown(KeyCode.Space)) {
			retryButton_Amount += progressBarSpeed * Time.deltaTime;
		} else {
			if(retryButton_Amount > 0)
				retryButton_Amount -= progressBarSpeed * Time.deltaTime*2;
		}
		Retry_LoadingBar.GetComponent<UnityEngine.UI.Image> ().fillAmount = retryButton_Amount / 100;
	}

	private void CheckFlick(HandList hands){
		Hand rightHand = hands.Rightmost;
		Hand leftHand = hands.Leftmost;
		if (isSwipeRight(rightHand, leftHand) || Input.GetKeyDown(KeyCode.RightArrow)) {
			if (!selectB.isPlaying) {
				selectB.Play ();
			}
			Done_LoadingBar.gameObject.SetActive (true);
			Done_LoadingBar_Background.gameObject.SetActive (true);
			Retry_LoadingBar.gameObject.SetActive (false);
			Retry_LoadingBar_Background.gameObject.SetActive (false);
			state = Status_State.DONE;
			isFlicking = true;
		}
		if (isSwipeLeft(rightHand, leftHand) || Input.GetKeyDown(KeyCode.LeftArrow)) {
			if (!selectB.isPlaying) {
				selectB.Play ();
			}
			Done_LoadingBar.gameObject.SetActive (false);
			Done_LoadingBar_Background.gameObject.SetActive (false);
			Retry_LoadingBar.gameObject.SetActive (true);
			Retry_LoadingBar_Background.gameObject.SetActive (true);
			state = Status_State.RETRY;
			isFlicking = true;
		}

	}

//	private bool isSwipeRight(Hand hand){
//		float speed = 120;
//		float yaw = hand.Direction.Yaw * 5;
//		return yaw > 1.2f && hand.PalmVelocity.x > speed;
//	}
//
//	private bool isSwipeLeft(Hand hand){
//		float speed = 120;
//		float yaw = hand.Direction.Yaw * 5 * -1;
//		return yaw > 1.2f && hand.PalmVelocity.x < -speed;
//	}
	private bool isSwipeRight(Hand rHand, Hand lHand){
		float speed = 120;
		float rYaw = rHand.Direction.Yaw * 5;
		float lYaw = lHand.Direction.Yaw * 5;
		if (rYaw > 1.2f && rHand.PalmVelocity.x > speed) {
			return true;
		} else if (lYaw > 1.2f && lHand.PalmVelocity.x > speed) {
			return true;
		}
		return false;
	}

	private bool isSwipeLeft(Hand rHand, Hand lHand){
		float speed = 120;
		float rYaw = rHand.Direction.Yaw * 5 * -1;
		float lYaw = lHand.Direction.Yaw * 5 * -1;
		if(rYaw > 1.2f && rHand.PalmVelocity.x < -speed){
			return true;	
		} else if(lYaw > 1.2f && lHand.PalmVelocity.x < -speed){
			return true;	
		}
		return false;
	}
	public void Load(){
		if (File.Exists (Application.persistentDataPath + fileName)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + fileName, FileMode.Open);
			LeaderBoard data = (LeaderBoard)bf.Deserialize (file);
			file.Close ();

			players = data.players;
		} else {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Create (Application.persistentDataPath + fileName);

			LeaderBoard data = new LeaderBoard ();
			InitData (ref data);

			players = data.players;
			bf.Serialize (file, data);
			file.Close ();
		}
	}

	void InitData(ref LeaderBoard data){

		for(int i = 0; i < 10; i++){
			PlayerInfo player = new PlayerInfo();

			player.name = "???";
			player.score = 0;
			player.ranking = "NaN";
			player.accuracy = 0;
			player.maxCombo = 0;
			data.players[i] = player;
		}

	}
}