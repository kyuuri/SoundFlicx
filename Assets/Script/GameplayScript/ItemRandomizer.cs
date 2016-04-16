using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemRandomizer : MonoBehaviour {

	public Material noteItemMaterial;

	public NoteRenderer noteRendererP1;
	public NoteRenderer noteRendererP2;

	public float percentRate = 40;
	private bool isRandom = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!isRandom) {
			RandomItem ();
			isRandom = true;
		}
	}

	void RandomItem(){
		for (int i = 0; i < noteRendererP1.allnotes.Length - 2; i++) {
			List<NoteDescription> list = noteRendererP1.allnotes [i];
			for (int j = 0; j < list.Count; j++) {
				NoteDescription note1 = list [j];
				if (note1.Length == 0) {
					float ran = Random.value * 100.0f;
					if (ran <= percentRate) {
						NoteDescription note2 = noteRendererP2.allnotes [i] [j];
						// same note same spot but different player

						note1.ContainItem = true;
						note2.ContainItem = true;

						GameObject par1 = Instantiate (Resources.Load ("NoteItem")) as GameObject;
						par1.transform.parent = note1.NoteObject.transform;
						par1.transform.localPosition = new Vector3 (0, 0, -1);
						par1.transform.localScale = new Vector3 (1, 1, 1);

						GameObject par2 = Instantiate (Resources.Load ("NoteItem")) as GameObject;
						par2.transform.parent = note2.NoteObject.transform;
						par2.transform.localPosition = new Vector3 (0, 0, -1);
						par2.transform.localScale = new Vector3 (1, 1, 1);
					}
				}
			}
		}
	}
}
