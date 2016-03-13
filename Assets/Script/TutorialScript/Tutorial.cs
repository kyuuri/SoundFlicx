using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Tutorial : MonoBehaviour {

	public GameObject images;
	private Sprite picture;
	private RectTransform rect;
	private Vector3 speedSizeImage;
	private  List<Sprite>  spriteList;
	private float[] timeStart;
	private float[] timeEnd;
	private bool[] isSingle;

	// Zone of time to check
	private int phaseNumber;
	private bool isImageChange;
	//Image number
	private int imageNumber;

	//number of image in loop;
	private int loopImage;
	// number of loop picture in time phase
	private int[] loopPictureNumber;
	private int frameCount;

	private readonly float DELAY = 0.2f;
	void Awake(){
		spriteList = new List<Sprite>();
		for(int i=1 ;i<19;i++){
			Sprite temp = (Sprite.Create(Resources.Load ("Tutorial/" + i) as Texture2D,new Rect(0, 0, 512,512), new Vector2(0, 0),100.0f));
			Debug.Log (temp);
			spriteList.Add (temp);
		}
	}

	void Start () {
		frameCount = 0;
		loopImage = 0;

		imageNumber = 0;
		isImageChange = false;
		phaseNumber = 0;
		timeStart = new float[]{ 0, 4, 8, 12, 20, 28, 32, 38, 48, 54, 64, 70, 88, 90, 92, 94 };
		timeEnd = new float[]{ 4, 8, 12, 16, 24, 32, 38, 40, 54, 56, 70, 72, 90, 92, 94, 98 };
		loopPictureNumber = new int[]{ 1, 1, 5, 2, 1, 1, 5, 1 };
		isSingle = new bool[]{ true, true, false, false, true, true, false, true };
		picture = images.GetComponent<Image> ().sprite;
		rect = images.GetComponent<RectTransform> ();
//		images.GetComponent<UnityEngine.UI.Image> ().sprite = spriteList [0];

		speedSizeImage = new Vector3 (0.1f, 0.1f);	
	}
	
	// Update is called once per frame
	void Update () {
		float timePass = TimerScript.timePass;
		Debug.Log ("Phase Number=" + phaseNumber + " image Number=" + imageNumber);
		if (timePass >= timeStart [phaseNumber] && timePass < timeEnd [phaseNumber] - DELAY) {
			ScaleUp ();
			if (isSingle[phaseNumber]) {
				ShowPictureSingle ();
			} else { //Not single Picture
				ShowLoopPicture();
			}
		} else if (timePass >= timeEnd [phaseNumber] - DELAY) {
			if (phaseNumber + 1 <= timeStart.Length) {
				if (timePass < timeStart [phaseNumber + 1]) {
					ScaleDown ();
				} else { //TimerScript > timeStart [phaseNumber + 1]
					imageNumber += loopPictureNumber [phaseNumber];
					frameCount = 0;
					loopImage = 0;
					ScaleUp ();
					phaseNumber++;
					isImageChange = false;
				}
			} else {
				ScaleDown ();
			}

		}



//		ScaleUp ();
//		if (timePass >= 5f) {
//			Debug.Log ("5f");
//			images.GetComponent<UnityEngine.UI.Image> ().sprite  = Resources.Load<Sprite>("basic2");
////			ScaleDown ();
//		}else if (timePass >= 2f ) {
//			Debug.Log("2f");
//			images.GetComponent<UnityEngine.UI.Image> ().sprite = spriteList [1];
//			ScaleUp ();
//		}
	}

	private void ScaleDown(){
		if (rect.localScale.x > 0) {
			rect.localScale -= speedSizeImage;
			if (rect.localScale.x <= 0) {
				rect.localScale = Vector3.zero;
			} 
		}
	}

	private void ScaleUp(){
		if (rect.localScale.x < 1)
			rect.localScale += speedSizeImage;
	}

	private void ShowPictureSingle(){
		if (!isImageChange) {
			Debug.Log ("Change SingleImage");
			images.GetComponent<UnityEngine.UI.Image> ().sprite = spriteList [imageNumber];
			isImageChange = true;
		}
	}

	private void ShowLoopPicture(){
		if (frameCount % 15 == 0) {
			images.GetComponent<UnityEngine.UI.Image> ().sprite = spriteList [imageNumber + loopImage];
			++loopImage;
			if (loopImage >= loopPictureNumber [phaseNumber]) {
				loopImage = 0;
			}
		}
		++frameCount;
	}
}


