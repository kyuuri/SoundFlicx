using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DifficultyBarController : MonoBehaviour {

	public Transform Easy_TextLevel;
	public Transform Normal_TextLevel;
	public Transform Hard_TextLevel;
	public Transform Easy_NumLevel;
	public Transform Normal_NumLevel;
	public Transform Hard_NumLevel;
	public Transform Easy_LoadingBar;
	public Transform Normal_LoadingBar;
	public Transform Hard_LoadingBar;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SelectDifficulty(float level){
		if (level == 1) {
			Easy_LoadingBar.gameObject.SetActive (true);
			Normal_LoadingBar.gameObject.SetActive (false);
			Hard_LoadingBar.gameObject.SetActive (false);
		} else if (level == 2) {
			Easy_LoadingBar.gameObject.SetActive (false);
			Normal_LoadingBar.gameObject.SetActive (true);
			Hard_LoadingBar.gameObject.SetActive (false);
		} else {
			Easy_LoadingBar.gameObject.SetActive (false);
			Normal_LoadingBar.gameObject.SetActive (false);
			Hard_LoadingBar.gameObject.SetActive (true);
		}
	}

	public void SetTextLevel(string easyLevel, string normalLevel, string hardLevel){

		Easy_NumLevel.GetComponent<Text>().text = "Lv. " + easyLevel;

		Normal_NumLevel.GetComponent<Text>().text = "Lv. " + normalLevel;

		Hard_NumLevel.GetComponent<Text>().text = "Lv. " + hardLevel;
	}
}

