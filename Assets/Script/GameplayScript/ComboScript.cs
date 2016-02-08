using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ComboScript : MonoBehaviour {

	public Text comboText;
	public static ComboScript Instance { get; private set;}

	private Vector3 scale;
	private bool isShrinking = false;
	private int count = 0;
	private int limit = 4;

	private int combo = 0;

	void Awake(){
		Instance = this;
	}

	void Start () {
		comboText.text = "";
		scale = comboText.transform.localScale;
	}

	void Update(){
		if (isShrinking) {
			Shrink ();
		}
	}

	public void MissCombo(){
		combo = 0;
		comboText.text = "";
	}

	public void ApplyCombo(int cb){
		combo += cb;
		comboText.transform.localScale = scale;
		comboText.text = combo + "";
		isShrinking = true;
		count = 0;
	}

	private void Shrink(){
		if (count < limit) {
			comboText.transform.localScale *= 0.91f;
			++count;
		} else if (count == limit) {
			isShrinking = false;
			count = 0;
		}
	}
}

