using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Leap;
using WindowsInput;

public class SongSelectionController : MonoBehaviour {

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
		DIFFICULTY_LAYER = 2,
		MODE_LAYER = 3,
		BOT_LAYER = 4 
	};

	public AudioSource selectSound;
	public AudioSource currentSong;
	private AudioClip[] songList;

	private float delayTrack = 0.0f;

	private float delayScene = 0.0f;
	private bool goingNext = false;

	private bool okToChange;

	private Leap.Controller controller;
	public float offset;
	private float timeDelay;
	private bool isFlicking;
	private float flickDelay = 0.4f;

	private float cancel_timeDelay;
	private bool cancel_isFlicking;
	private float cancel_flickDelay = 0.4f;
	private bool isGrab;

	public List <Mtemplate> Mtems = new List<Mtemplate> ();
	private List <GameObject> buttonList = new List<GameObject> ();
	private List <SampleButton> sampleButonList = new List<SampleButton> ();
	private List <string> descriptionList = new List<string> ();
	private List <string> nameList = new List<string> ();

	public GameObject sampleButton;
	public Transform contentPanel;
	private int indexColorChange = 0;

	public ParticleSystem[] parBox = new ParticleSystem[4];

	public DifficultyBarController difficultyBar;
	public RadialProcessBarScript progressBar;

	public RectTransform panel;
	public RectTransform speedPanel;
	public RectTransform difficultyPanel;
	public RectTransform modePanel;
	public RectTransform botLvPanel;

	public Text speedNumber;
	private float speed = 2.0f;

	public Transform solo;
	public Transform vsBot;
	private UnityEngine.UI.Image soloImage;
	private UnityEngine.UI.Image vsBotImage;
	private bool isSolo;

	private Vector3 destination = new Vector3(1000,1000,1000);
	private Vector3 speedPanelPosition;
	private Vector3 difficultyPanelPosition;
	private Vector3 modePanelPosition;
	private Vector3 botPanelPosition;

	// 1 easy 2 normal 3 hard
	private int level = 2;

	private bool isWait = false;

	private Layers layerState;

	private bool isNormalState = true;

	//Move Panel
	private float startTime;
	private Vector3 startMarker;
	private Vector3 endMarker;
	private bool isPanelMoving;
	private float journeyLength;
	private bool moveLeft;
	private bool moveRight;

	private Vector3 sizeMin;
	private Vector3 sizeNormal;

	public UnityEngine.UI.Image botStar1;
	public UnityEngine.UI.Image botStar2;
	public UnityEngine.UI.Image botStar3;
	public UnityEngine.UI.Image botStar4;
	public UnityEngine.UI.Image botStar5;
	private UnityEngine.UI.Image[] botStarArray;
	private int botLv;

	public Transform secondCamera;
	public UnityEngine.UI.Image loadingImage;
	private bool fadeLoadingImage;

	public GameObject leapCamera;

	public void Awake ()
	{

		songList = new AudioClip[GlobalData.textFile.Length];
		//GetFiles ();
		for (int i = 0; i < GlobalData.textFile.Length; i++) {
			string[] spt = GlobalData.textFile [i].Split (',');

			songList[i] = Resources.Load ("Tracks/" + spt [0] + "Audio.mp3") as AudioClip;
		}
		currentSong.clip = songList [0];
	}

	public void Start(){
		fadeLoadingImage = false;
		isPanelMoving = false;
		sizeMin = new Vector3 (0.75f, 0.75f, 1);
		sizeNormal = new Vector3 (1, 1, 1);

		soloImage = solo.GetComponent<UnityEngine.UI.Image>();
		vsBotImage = vsBot.GetComponent<UnityEngine.UI.Image> ();
		vsBotImage.color = new Color (vsBotImage.color.r, vsBotImage.color.g, vsBotImage.color.b, 0f);
		isSolo = true;

		botLv = 3;
		botStar4.color = new Color (0.2f, 0.2f, 0.2f, 1.0f);
		botStar5.color = new Color (0.2f, 0.2f, 0.2f, 1.0f);
		botStarArray = new UnityEngine.UI.Image[5];
		botStarArray [0] = botStar1;
		botStarArray [1] = botStar2;
		botStarArray [2] = botStar3;
		botStarArray [3] = botStar4;
		botStarArray [4] = botStar5;

		okToChange = true;
		delayScene = 0.0f;
		layerState = Layers.NORMAL_LAYER;
		for (int i = 0; i < GlobalData.textFile.Length; i++) {
			string[] spt = GlobalData.textFile[i].Split(',');
			Mtemplate mt = new Mtemplate ();
			mt.M_Name = spt [0];
			buttonList.Add (CreateButton(mt, GlobalData.textFile [i]));
		}
		buttonList [0].GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);

		disableParBox ();

		if (buttonList.Count >= 1) {
			//			Debug.Log (buttonList.Count+" = "+indexColorChange);
			buttonList [indexColorChange].GetComponent<UnityEngine.UI.Image> ().color = new Color (103/255f, (172/255f), 1f,1f);
			if (buttonList.Count >= 2) {
				buttonList [indexColorChange + 1].GetComponent<RectTransform> ().localScale = new Vector3 (0.75f, 0.75f, 1);
			}
		}

		contentPanel.localPosition = new Vector3 (contentPanel.localPosition.x + 200, contentPanel.localPosition.y);


		//leap motion
		controller = new Leap.Controller ();
		isFlicking = false;
		timeDelay = 0;
		cancel_isFlicking = false;
		cancel_timeDelay = 0;
		offset = 5;
		isGrab = false;

		//song selection controller
		speedPanelPosition = speedPanel.position;
		speedPanel.position = Vector3.Slerp (speedPanel.position, destination,5);
		difficultyPanelPosition = difficultyPanel.position;
		difficultyPanel.position = Vector3.Slerp (speedPanel.position, destination,5);
		modePanelPosition = modePanel.position;
		modePanel.position = Vector3.Slerp (speedPanel.position, destination,5);
		botPanelPosition = botLvPanel.position;
		botLvPanel.position = Vector3.Slerp (botLvPanel.position, destination, 5);


		delayTrack = 0.0f;
		if (delayTrack >= 0.5f) {
			currentSong.Play ();
		}
		fadeLoadingImage = true;
		secondCamera.gameObject.SetActive (true);
	}

	void Update(){
		if (fadeLoadingImage) {
			//Debug.Log (loadingImage.color.a );
			loadingImage.color = new Color (loadingImage.color.r, loadingImage.color.g, loadingImage.color.b, loadingImage.color.a - 0.05f);
			if (loadingImage.color.a <= 0) {
				fadeLoadingImage = false;
			}
		}

		if(isPanelMoving){
			//Debug.Log ("Move");
			float distCovered = (Time.time - startTime) * 975.0f;
			float fracJourney = distCovered / journeyLength;

			contentPanel.localPosition = Vector3.Lerp(startMarker, endMarker, fracJourney);
			buttonList [indexColorChange].GetComponent<RectTransform> ().localScale = Vector3.Lerp(sizeMin, sizeNormal, fracJourney);
			if (moveRight) {
				if (indexColorChange - 1 >= 0) {
					buttonList [indexColorChange - 1].GetComponent<RectTransform> ().localScale = Vector3.Lerp (sizeNormal, sizeMin, fracJourney);
				}
				if (indexColorChange + 1 < buttonList.Count) {
					buttonList [indexColorChange + 1].GetComponent<RectTransform> ().localScale = sizeMin;
				}
			}
			if (moveLeft) {
				if (indexColorChange + 1 < buttonList.Count) {
					buttonList [indexColorChange + 1].GetComponent<RectTransform> ().localScale = Vector3.Lerp (sizeNormal, sizeMin, fracJourney);
				}
				if (indexColorChange - 1 >= 0) {
					buttonList [indexColorChange - 1].GetComponent<RectTransform> ().localScale = sizeMin;
				}
			}

			if (contentPanel.localPosition == endMarker) {
				//
				isPanelMoving = false;
				moveRight = false;
				moveLeft = false;
			}
		}

		delayTrack += Time.deltaTime;

		Frame frame = controller.Frame ();
		HandList hands = frame.Hands;

		MapGesture (hands);

		if (goingNext) {
			delayScene += Time.deltaTime;
		}

		if (delayScene >= 0.8f) {
			loadingImage.color = new Color (loadingImage.color.r, loadingImage.color.g, loadingImage.color.b, 1);
			leapCamera.SetActive (false);
			secondCamera.gameObject.SetActive (false);
			disableParBox ();
			if (isSolo) {
				UnityEngine.Application.LoadLevel ("Gameplay");
			} else {
				UnityEngine.Application.LoadLevel ("GameplayVSMode");
			}
		}

		if (delayTrack >= 0.5f && !currentSong.isPlaying) {
			currentSong.Play ();
		}

	}


	public void selectedListRight(){
		if (!isPanelMoving) {
			if (buttonList.Count >= 1 && indexColorChange < buttonList.Count - 1) {
				++indexColorChange;
				buttonList [indexColorChange].GetComponent<UnityEngine.UI.Image> ().color = new Color (103 / 255f, (172 / 255f), 1f, 1f);
				//					buttonList [indexColorChange].GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
				buttonList [indexColorChange - 1].GetComponent<UnityEngine.UI.Image> ().color = new Color (1, 1, 1, 1);
				startTime = Time.time;
				startMarker = contentPanel.localPosition;
				endMarker = new Vector3 (contentPanel.localPosition.x - 215, contentPanel.localPosition.y, contentPanel.localPosition.z);
				journeyLength = Vector3.Distance (startMarker, endMarker);
				moveRight = true;
				//contentPanel.localPosition = Vector3.Lerp (contentPanel.localPosition,new Vector3(contentPanel.localPosition.x - 215, contentPanel.localPosition.y, 0f),  (Time.time - startTime) / 5f);
				//				contentPanel.localPosition = new Vector3 (contentPanel.localPosition.x - 215, contentPanel.localPosition.y);

				string songName = nameList [indexColorChange];
				currentSong.clip = songList [indexColorChange];
				delayTrack = 0.0f;
				isPanelMoving = true;
			}
		}
	}

	public void selectedListLeft(){
		if (!isPanelMoving) {
			if (buttonList.Count >= 1 && buttonList.Count >= 1 && indexColorChange > 0) {
				--indexColorChange;
				buttonList [indexColorChange].GetComponent<UnityEngine.UI.Image> ().color = new Color (103 / 255f, (172 / 255f), 1f, 1f);
				buttonList [indexColorChange + 1].GetComponent<UnityEngine.UI.Image> ().color = new Color (1, 1, 1, 1);

				startTime = Time.time;
				startMarker = contentPanel.localPosition;
				endMarker = new Vector3 (contentPanel.localPosition.x + 215, contentPanel.localPosition.y, contentPanel.localPosition.z);
				journeyLength = Vector3.Distance (startMarker, endMarker);
				moveLeft = true;

				string songName = nameList [indexColorChange];
				//currentSong.clip = Resources.Load (songName + "Audio.mp3") as AudioClip;
				currentSong.clip = songList [indexColorChange];
				delayTrack = 0.0f;
				isPanelMoving = true;
			}
		}
	}

	private GameObject CreateButton(Mtemplate temp, string text){

		GameObject newButton = Instantiate (sampleButton) as GameObject;
		SampleButton button = newButton.GetComponent <SampleButton> ();

		descriptionList.Add (text);
		List<string> eachLine = new List<string>();

		eachLine.AddRange(text.Split(","[0]) );
		button.nameLabel.text = temp.M_Name.Replace("_"," ");
		nameList.Add (eachLine [0]);
		button.composer.text = eachLine [1];
		button.bpm.text = "BPM : "+eachLine [2];
		//Debug.Log (eachLine [0]);
		button.icon.GetComponent<UnityEngine.UI.Image> ().sprite = Sprite.Create(Resources.Load ("Tracks/" + eachLine [0] + "Image") as Texture2D,new Rect(0, 0, 256,256), new Vector2(0, 0),100.0f);
		newButton.transform.SetParent (contentPanel);
		RectTransform rect = newButton.GetComponent<RectTransform> ();
		rect.localScale = new Vector3 (0.75f, 0.75f, 1);
		rect.localPosition = new Vector3 (rect.position.x, rect.position.y, 0);

		return newButton;
	}

	public void selectedSong(){
		GlobalData.songIndex = indexColorChange;
	}

	public void showSpeed (){
		isWait = true;
		layerState = Layers.SPEED_LAYER;
		progressBar.ResetAmount ();
		selectedSong ();
		modePanel.position = Vector3.Slerp (modePanel.position, destination,5);
		botLvPanel.position = Vector3.Slerp (botPanelPosition, destination, 5);
		difficultyPanel.position = Vector3.Slerp (speedPanel.position, destination,5);
		speedPanel.position = Vector3.Slerp(destination, speedPanelPosition, 5);
		StartCoroutine(TestCoroutine());
	}

	public void showDifficulty(){
		isWait = true;
		isNormalState = false;
		layerState = Layers.DIFFICULTY_LAYER;
		progressBar.ResetAmount ();
		speedPanel.position = Vector3.Slerp (speedPanel.position, destination,5);
		modePanel.position = Vector3.Slerp (modePanel.position, destination,5);
		botLvPanel.position = Vector3.Slerp (botPanelPosition, destination, 5);
		difficultyPanel.position = Vector3.Slerp(destination, difficultyPanelPosition, 5);
		StartCoroutine(TestCoroutine());

	}

	public void showMode(){
		isWait = true;
		layerState = Layers.MODE_LAYER;
		progressBar.ResetAmount ();
		difficultyPanel.position = Vector3.Slerp (difficultyPanel.position, destination,5);
		speedPanel.position = Vector3.Slerp (speedPanel.position, destination,5);
		botLvPanel.position = Vector3.Slerp (botPanelPosition, destination, 5);
		modePanel.position = Vector3.Slerp(destination, modePanelPosition, 5);
		StartCoroutine(TestCoroutine());

	}

	public void showBot(){
		isWait = true;
		layerState = Layers.BOT_LAYER;
		progressBar.ResetAmount ();
		difficultyPanel.position = Vector3.Slerp (difficultyPanel.position, destination,5);
		speedPanel.position = Vector3.Slerp (speedPanel.position, destination,5);
		modePanel.position = Vector3.Slerp (modePanel.position, destination,5);
		botLvPanel.position = Vector3.Slerp (destination, botPanelPosition, 5);
		StartCoroutine(TestCoroutine());
	}

	private void backFromSpeed(){
		showDifficulty ();
	}

	private void backFromDifficulty(){
		disableParBox ();
		if (isSolo) {
			showMode ();
		} else {
			showBot ();
		}
	}

	private void backFromBot(){
		showMode ();
	}

	private void backFromMode(){
		isNormalState = true;
		difficultyPanel.position = Vector3.Slerp (difficultyPanel.position, destination,5);
		speedPanel.position = Vector3.Slerp (speedPanel.position, destination,5);
		botLvPanel.position = Vector3.Slerp (botPanelPosition, destination, 5);
		modePanel.position = Vector3.Slerp(modePanelPosition, destination, 5);
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

	public void botLvUp(){
		if (botLv < 5) {
			botStarArray[botLv].color = new Color (botStar1.color.r, botStar1.color.g, botStar1.color.b, 1f);
			++botLv;
		}
	}

	public void botLvDown(){
		if (botLv > 1) {
			botStarArray [botLv - 1].color = new Color (0.2f, 0.2f, 0.2f, 1.0f);//new Color (0f, 0f, 0f, 1f);
			--botLv;
		}
	}



	IEnumerator TestCoroutine() {
		yield return new WaitForSeconds(0.7f);
		isWait = false;
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

	private void versusMode(){
		soloImage.color = new Color (vsBotImage.color.r, vsBotImage.color.g, vsBotImage.color.b, 0f);
		vsBotImage.color = new Color (vsBotImage.color.r, vsBotImage.color.g, vsBotImage.color.b, 1f);
		isSolo = false;
	}

	private void singleMode (){
		vsBotImage.color = new Color (vsBotImage.color.r, vsBotImage.color.g, vsBotImage.color.b, 0f);
		soloImage.color = new Color (vsBotImage.color.r, vsBotImage.color.g, vsBotImage.color.b, 1f);
		isSolo = true;
	}

	public void changeScene(){
		int index = GlobalData.songIndex;
		string temp = descriptionList [index];
		List<string> eachLine = new List<string>();
		Difficulty difficult;
		eachLine.AddRange(temp.Split(","[0]) );
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

		Track track = new Track(nameList[index],eachLine[1], difficult,level, float.Parse(eachLine[2]),  float.Parse(eachLine[6]));
		GlobalData.selectedTrack = track;

		GlobalData.speed = speed;
		GlobalData.botLv = botLv;
		GlobalData.isVersus = !isSolo;

		goingNext = true;
	}


	private void MapGesture(HandList hands){

		Hand rightHand = hands.Rightmost;
		Hand leftHand = hands.Leftmost;

		if (!cancel_isFlicking) {
			ManageCancelMotion (rightHand, leftHand);
		} else if (cancel_isFlicking) {
			cancel_timeDelay += Time.deltaTime;
		}
		if (cancel_timeDelay >= cancel_flickDelay) {
			cancel_timeDelay = 0;
			cancel_isFlicking = false;
		}


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


		if (layerState == Layers.NORMAL_LAYER) {
			if (isSwipeRight (rightHand,leftHand) || Input.GetKeyDown (KeyCode.RightArrow)) {
				isFlicking = true;
				this.selectedListRight ();
				selectSound.Play ();
				progressBar.GetAmount ();
			}
			if (isSwipeLeft (rightHand,leftHand) || Input.GetKeyDown (KeyCode.LeftArrow)) {
				isFlicking = true;
				this.selectedListLeft ();
				selectSound.Play ();
				progressBar.ResetAmount ();
			}
		}else if (layerState == Layers.MODE_LAYER){
			if (isSwipeRight (rightHand,leftHand) || Input.GetKeyDown(KeyCode.RightArrow)) {
				isFlicking = true;
				this.versusMode ();
				selectSound.Play ();
				progressBar.ResetAmount ();
			}
			if (isSwipeLeft (rightHand,leftHand) || Input.GetKeyDown(KeyCode.LeftArrow)) {
				isFlicking = true;
				this.singleMode ();
				selectSound.Play ();
				progressBar.ResetAmount ();
			}
			
		}else if(layerState == Layers.BOT_LAYER){
			if (isSwipeRight (rightHand,leftHand) || Input.GetKeyDown(KeyCode.RightArrow)) {
				isFlicking = true;
				this.botLvUp ();
				selectSound.Play ();
				progressBar.ResetAmount ();
			}
			if (isSwipeLeft (rightHand,leftHand) || Input.GetKeyDown(KeyCode.LeftArrow)) {
				isFlicking = true;
				this.botLvDown ();
				selectSound.Play ();
				progressBar.ResetAmount ();
			}
		}else if (layerState == Layers.SPEED_LAYER) {
			if (isSwipeRight (rightHand,leftHand) || Input.GetKeyDown(KeyCode.RightArrow)) {
				isFlicking = true;
				this.speedUp ();
				selectSound.Play ();
				progressBar.ResetAmount ();
			}
			if (isSwipeLeft (rightHand,leftHand) || Input.GetKeyDown(KeyCode.LeftArrow)) {
				isFlicking = true;
				this.speedDown ();
				selectSound.Play ();
				progressBar.ResetAmount ();
			}

		} else if (layerState == Layers.DIFFICULTY_LAYER){
			if (isSwipeRight(rightHand,leftHand) || Input.GetKeyDown(KeyCode.RightArrow)) {
				isFlicking = true;
				this.increaseLevel ();
				selectSound.Play ();
				progressBar.ResetAmount ();
			}
			if (isSwipeLeft(rightHand,leftHand) || Input.GetKeyDown(KeyCode.LeftArrow)) {
				isFlicking = true;
				this.decreaseLevel ();
				selectSound.Play ();
				progressBar.ResetAmount ();
			}
			if (level == 1) {
				difficultyBar.SelectDifficulty (level);
				for (int i = 0; i < 4; i++) {
					parBox [i].startColor = Color.green;
				}
			} else if (level == 2) {
				difficultyBar.SelectDifficulty (level);
				for (int i = 0; i < 4; i++) {
					parBox [i].startColor = Color.yellow;
				}
			} else {
				difficultyBar.SelectDifficulty (level);
				for (int i = 0; i < 4; i++) {
					parBox [i].startColor = Color.red;
				}
			}

		}
	}

	private void FillProgressBar(Hand hand){

		if (progressBar.GetAmount() >= 100) {
			progressBar.SetText ("Done!!");
			cancel_isFlicking = true;
			if (!GetComponent<AudioSource> ().isPlaying) {
				GetComponent<AudioSource> ().Play ();
			}

			if (layerState == Layers.NORMAL_LAYER) {

				if (indexColorChange == 0) {
					for (int i = 0; i < 4; i++) {
						parBox [i].gameObject.active = false;
					}
					speed = 1;
					loadingImage.color = new Color (loadingImage.color.r, loadingImage.color.g, loadingImage.color.b, 1);
					secondCamera.gameObject.SetActive (false);
					leapCamera.SetActive (false);
					//public Track(string songName, string composer,Difficulty dificulty, int level, float bpm, float offset)
					Track track = new Track(nameList[0], "Tutorial", Difficulty.HARD, 0, 120,  -0.075f);
					GlobalData.selectedTrack = track;
					UnityEngine.Application.LoadLevel ("GameplayTutorial");
				}
				else {
					showMode ();
					contentPanel.localPosition = new Vector3 (contentPanel.localPosition.x - 180 + indexColorChange * 214.8f, contentPanel.localPosition.y);
					for (int i = 0; i < buttonList.Count; i++) {
						if (i == indexColorChange) {
							continue;
						}
						GameObject obj = buttonList [i];
						obj.active = false;
					}
				}
			} else if (layerState == Layers.MODE_LAYER){
				if (isSolo) {
					
					if (isNormalState) {
						for (int i = 0; i < 4; i++) {
							Vector3 pos = parBox [i].transform.position;
							parBox [i].transform.position = new Vector3 (pos.x - 8.5f, pos.y, pos.z);
						}
					}
					showDifficulty ();

					difficultyBar.SelectDifficulty (2);
					level = 2;
					for (int i = 0; i < 4; i++) {
						parBox [i].gameObject.active = true;
						parBox [i].startColor = Color.yellow;
					}

					string temp = descriptionList [indexColorChange];
					List<string> eachLine = new List<string> ();
					eachLine.AddRange (temp.Split ("," [0]));

					difficultyBar.SetTextLevel (eachLine [3], eachLine [4], eachLine [5]);
				} else {
					showBot ();
				}
			} else if(layerState == Layers.BOT_LAYER){
				
				if (isNormalState) {
					for (int i = 0; i < 4; i++) {
						Vector3 pos = parBox [i].transform.position;
						parBox [i].transform.position = new Vector3 (pos.x - 8.5f, pos.y, pos.z);
					}
				}
				showDifficulty ();

				difficultyBar.SelectDifficulty (2);
				level = 2;
				for (int i = 0; i < 4; i++) {
					parBox [i].gameObject.active = true;
					parBox [i].startColor = Color.yellow;
				}

				string temp = descriptionList [indexColorChange];
				List<string> eachLine = new List<string> ();
				eachLine.AddRange (temp.Split ("," [0]));

				difficultyBar.SetTextLevel (eachLine [3], eachLine [4], eachLine [5]);
			}else if (layerState == Layers.DIFFICULTY_LAYER) {
				showSpeed (); 

			} else {
				if(okToChange){
					changeScene ();
				}
				okToChange = false;
			}

		} else if (hand.GrabStrength == 1 || Input.GetKey( KeyCode.Space ) ) {
			if (!isWait) {
				isGrab = true;
				progressBar.IncreaseAmount ();
			}
		} else {
			isGrab = false;
			if(progressBar.GetAmount() <= 0){
				progressBar.SetText ("Grap !");

			} else if (progressBar.GetAmount() > 0) {
				progressBar.DecreaseAmount ();
			}
		}
	}

	private void ManageCancelMotion (Hand rightHand, Hand leftHand){
		if ((isSwipeRight (rightHand) && isSwipeLeft (leftHand))||Input.GetKeyDown(KeyCode.Escape) ) {
			cancel_isFlicking = true;
			progressBar.ResetAmount ();
			if (layerState == Layers.MODE_LAYER) {
				Debug.Log ("Normal Mode");
				backFromMode ();
				layerState = Layers.NORMAL_LAYER;

				for (int i = 0; i < buttonList.Count; i++) {
					if (i == indexColorChange) {
						continue;
					}
					GameObject obj = buttonList [i];
					obj.active = true;				
				}
				contentPanel.localPosition = new Vector3 (contentPanel.localPosition.x + 180 - indexColorChange * 214.8f, contentPanel.localPosition.y);

				for (int i = 0; i < 4; i++) {
					Vector3 pos = parBox [i].transform.position;
					parBox [i].gameObject.active = false;
					parBox [i].transform.position = new Vector3 (pos.x + 8.5f, pos.y, pos.z);
					parBox [i].startColor = new Color (236 / 255.0f, 213 / 255.0f, 92 / 255.0f);

				}

			} else if (layerState == Layers.SPEED_LAYER) {
				backFromSpeed ();
				layerState = Layers.DIFFICULTY_LAYER;
			} else if (layerState == Layers.DIFFICULTY_LAYER) {
				backFromDifficulty ();
				if (isSolo) {
					layerState = Layers.MODE_LAYER;
				}else{
					layerState = Layers.BOT_LAYER;
				}
			} else if (layerState == Layers.BOT_LAYER) {
				backFromBot ();
				layerState = Layers.MODE_LAYER;
			}
		}
	}

	private void disableParBox(){
		for (int i = 0; i < 4; i++) {
			parBox [i].startColor = new Color (236/255.0f,213/255.0f,92/255.0f);
			parBox [i].gameObject.active = false;
		}
	}

	private bool isSwipeRight(Hand rHand, Hand lHand){
		float speed = 120;
		float rYaw = rHand.Direction.Yaw * offset;
		float lYaw = lHand.Direction.Yaw * offset;
		if (rYaw > 1.2f && rHand.PalmVelocity.x > speed) {
			return true;
		} else if (lYaw > 1.2f && lHand.PalmVelocity.x > speed) {
			return true;
		}
		return false;
	}

	private bool isSwipeLeft(Hand rHand, Hand lHand){
		float speed = 120;
		float rYaw = rHand.Direction.Yaw * offset * -1;
		float lYaw = lHand.Direction.Yaw * offset * -1;
		if(rYaw > 1.2f && rHand.PalmVelocity.x < -speed){
			return true;	
		} else if(lYaw > 1.2f && lHand.PalmVelocity.x < -speed){
			return true;	
		}
		return false;
	}

	private bool isSwipeRight(Hand hand){
		float speed = 120;
		float yaw = hand.Direction.Yaw * offset;
		return yaw > 1.2f && hand.PalmVelocity.x > speed;
	}

	private bool isSwipeLeft(Hand hand){
		float speed = 120;
		float yaw = hand.Direction.Yaw * offset * -1;
		return yaw > 1.2f && hand.PalmVelocity.x < -speed;
	}
}
