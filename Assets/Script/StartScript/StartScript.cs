using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Leap;

public class StartScript : MonoBehaviour {

	public AudioSource source;

	public Text startText;
	private static float delay = 0.5f;
	private float timer = 0;
	private bool blinkUp = false;
	private float opac = 1.0f;
	private float opacRate = 0.06f;

	private bool goingNext = false;
	private float sceneTimer = 0;

	Leap.Controller controller;
	// Use this for initialization
	void Start () {
		Application.runInBackground = true;
		controller = new Leap.Controller ();
		controller.EnableGesture (Gesture.GestureType.TYPESWIPE);
	}

	// Update is called once per frame
	void Update () {
		Blink ();

		Leap.Frame frame = controller.Frame ();
		GestureList gestures = frame.Gestures();

		for (int i = 0; i < gestures.Count; i++) {
			if (gestures [i].Type == Gesture.GestureType.TYPE_SWIPE) {
				if(!source.isPlaying && ! goingNext){
				goingNext = true;
					source.Play ();
				}
				delay = 0.1f;
				opacRate = 0.65f;
				startText.color = new Color (startText.color.r, startText.color.g, startText.color.b, opac);
			}
		}

		if(goingNext){
			sceneTimer += Time.deltaTime;
		}

		if (sceneTimer > 1.7f) {
			ChangeScene ();
		}
	}

	void Blink(){
		//startText.enabled = blinkUp;
		if (blinkUp) {
			opac += opacRate;
		} else {
			opac -= opacRate;
		}
		startText.color = new Color (startText.color.r, startText.color.g, startText.color.b, opac);
		timer += Time.deltaTime;

		if (timer >= delay) {
			timer = 0;
			blinkUp = !blinkUp;
		}
	}

	public void ChangeScene(){
		SceneManager.LoadSceneAsync("SongSelection");
	}

	float PercentWidth(float width){
		return (width / 100) * UnityEngine.Screen.width;
	}

	float PercentHeight(float height){
		return (height / 100) * UnityEngine.Screen.height;
	}


}