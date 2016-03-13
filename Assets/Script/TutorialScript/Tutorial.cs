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
	// Use this for initialization
	void Awake(){
		spriteList = new List<Sprite>();
		for(int i=0 ;i<5;i++){
			Sprite temp = (Sprite.Create(Resources.Load ("basic" + i) as Texture2D,new Rect(0, 0, 500,500), new Vector2(0, 0),100.0f));
			spriteList.Add (temp);
		}
	}

	void Start () {
		Debug.Log (spriteList.Count);
		picture = images.GetComponent<Image> ().sprite;
		rect = images.GetComponent<RectTransform> ();
		images.GetComponent<UnityEngine.UI.Image> ().sprite = Sprite.Create(Resources.Load ("basic0") as Texture2D,new Rect(0, 0, 500,500), new Vector2(0, 0),100.0f);
		//		

		speedSizeImage = new Vector3 (0.1f, 0.1f);	
	}
	
	// Update is called once per frame
	void Update () {
		ScaleUp ();
		if (TimerScript.timePass >= 5f) {
			Debug.Log ("5f");
			images.GetComponent<UnityEngine.UI.Image> ().sprite  = Resources.Load<Sprite>("basic2");
//			ScaleDown ();
		}else if (TimerScript.timePass >= 2f ) {
			Debug.Log("2f");
			images.GetComponent<UnityEngine.UI.Image> ().sprite = spriteList [1];
			ScaleUp ();
		}
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


}


