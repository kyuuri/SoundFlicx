using UnityEngine;
using System.Collections.Generic;

public class NoteRenderer : MonoBehaviour {

	public Transform[] lanePosition = new Transform[4];
	public static List<NoteDescription>[] allnotes = new List<NoteDescription>[4];

	// Use this for initialization
	void Start() {
		//temp 1000
		allnotes[0] = new List<NoteDescription>();
		allnotes[1] = new List<NoteDescription>();
		allnotes[2] = new List<NoteDescription>();
		allnotes[3] = new List<NoteDescription>();

		InvokeRepeating("GenTestNote", 0.0f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void GenTestNote(){
		//for generating further notes, will be removed later
		float x = 10;
		GameObject note = Instantiate(Resources.Load("Note")) as GameObject;
		int random = Random.Range (0, 4);
		note.transform.position = new Vector3 (lanePosition [random].position.x, 0.03f, lanePosition [random].position.z - 13f + x);
		NoteDescription noteDescription = note.GetComponent<NoteDescription> ();
		//Debug.Log ("before " + noteDescription.ToString());
		// s = vt
		noteDescription = new NoteDescription(TimerScript.timePass + x/Runner.speed, random, 1, 0);
		noteDescription.NoteObject = note;
		//Debug.Log ("after " + noteDescription);
		allnotes[random].Add(noteDescription);
	}


}
