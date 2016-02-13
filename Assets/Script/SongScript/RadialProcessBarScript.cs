using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Leap;

public class RadialProcessBarScript : MonoBehaviour {

	private Leap.Controller controller;
	public Transform LoadingBar;
	public Transform TextLevel;
	[SerializeField] private float currentAmount;
	[SerializeField] private float speed;


	// Use this for initialization
	void Start () {
		controller = new Leap.Controller ();
	}
	
	// Update is called once per frame
	void Update () {
		Frame frame = controller.Frame ();
		Hand hand = frame.Hands.Rightmost;

		if (currentAmount >= 100) {
			TextLevel.GetComponent<Text>().text = "Done!";

		}else if (hand.GrabStrength > 0.7) {
			currentAmount += speed * Time.deltaTime;
			TextLevel.GetComponent<Text> ().text = ((int)currentAmount).ToString();
		} else {
			if(currentAmount > 0)
				currentAmount -= speed * Time.deltaTime;	
		}

		LoadingBar.GetComponent<UnityEngine.UI.Image> ().fillAmount = currentAmount / 100;
	}


}
