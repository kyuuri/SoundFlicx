using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TypeController : MonoBehaviour {

	public Text text;
	// Use this for initialization
	void Start () {
		text.text = "";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddText (string label){
		text.text = text.text + label;
	}

	public void DeleteLabel(){
		Debug.Log (text.text.Length);
		if(text.text.Length>0)
			text.text = text.text.Remove (text.text.Length-1);
	}
}
