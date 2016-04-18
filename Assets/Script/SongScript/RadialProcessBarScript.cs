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
	}

	public float GetAmount(){
		return currentAmount;
	}

	public void ResetAmount(){
		currentAmount = 0;
		UpdateLoadingBar ();
	}

	public void SetAmount(float amount){
		currentAmount = amount;
		UpdateLoadingBar ();
	}

	public void IncreaseAmount(){
		currentAmount += speed * Time.deltaTime;
		this.SetText ();
		UpdateLoadingBar ();
	}

	public void DecreaseAmount(){
		currentAmount -= speed * Time.deltaTime * 2;
		this.SetText ();
		UpdateLoadingBar ();
	}

	public void SetText(string text){
		TextLevel.GetComponent<Text>().text = text;
	}

	public void SetText(){
		TextLevel.GetComponent<Text> ().text = ((int)currentAmount).ToString() + "%";
	}
	private void UpdateLoadingBar(){
		LoadingBar.GetComponent<UnityEngine.UI.Image> ().fillAmount = currentAmount / 100;
	}
}
