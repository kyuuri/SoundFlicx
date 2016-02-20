using UnityEngine;
using System.Collections.Generic;
using NAudio.Midi;
using NAudio.Utils;
using System.IO;

public class NoteRenderer : MonoBehaviour {

	public Transform[] lanePosition = new Transform[4];
	public static List<NoteDescription>[] allnotes = new List<NoteDescription>[6];

	void Awake(){
		allnotes[0] = new List<NoteDescription>();
		allnotes[1] = new List<NoteDescription>();
		allnotes[2] = new List<NoteDescription>();
		allnotes[3] = new List<NoteDescription>();
		allnotes[4] = new List<NoteDescription>();
		allnotes[5] = new List<NoteDescription>();

		//InvokeRepeating("GenTestNote", 0.3f, 0.46875f);
		GenerateNoteFromMidi("");
		GlobalData.result = new ResultScore ();
	}

	// Use this for initialization
	void Start() {
		
	}

	void GenerateNoteFromMidi(string midiPath){
		string songName = GlobalData.selectedTrack.songName;
		MidiFile midi;
		if (songName == null) {
			//songName = "Test";
			//GlobalData.selectedTrack.offset = -0.17f;

			songName = "The_Clear_Blue_Sky";
			GlobalData.selectedTrack.offset = -0.0f;
		}
		TextAsset asset = Resources.Load (songName + "Midi") as TextAsset;
		Stream strm = new MemoryStream (asset.bytes);
		BinaryReader br = new BinaryReader (strm);
		//midi = new MidiFile ("Assets/Resources/" + songName + "Midi.bytes");
		midi = new MidiFile(br, false);

		MidiNoteData noteData = MidiFileReader.ParseNote (midi);

		//test, wil be removed later
		List<NoteMidiEvent> events;
		events = noteData.midiEvents [GlobalData.selectedTrack.difficulty];


		for (int i = 0; i < events.Count ; i++) {
			NoteDescription noteDescription = ToNoteDescription(events [i]);
			int lane = -noteDescription.Lane + 3;
			if (noteDescription.IsFlick) {
				if (noteDescription.Lane <= 1) {
					lane = 4;
				} else {
					lane = 5;
				}
			}

			GameObject note = Instantiate (Resources.Load ("Note")) as GameObject;
			noteDescription.NoteObject = note;

			if (noteDescription.IsFlick) { // flick note
				note = Instantiate (Resources.Load ("FlickNote")) as GameObject;
				noteDescription.NoteObject = note;
				if (noteDescription.Lane <= 1) {
					note.transform.position = new Vector3 (0.46f, 0.35f, lanePosition [0].position.z - 13f + Runner.speed * (noteDescription.HitTime - TimerScript.delay) - 1.586f/2.7f); //1.586f
				} else {
					note.transform.position = new Vector3 (-0.46f, 0.35f, lanePosition [3].position.z - 13f + Runner.speed * (noteDescription.HitTime - TimerScript.delay) - 1.586f/2.7f);
					note.transform.rotation = Quaternion.Euler(-90.0f,270.0f,-270.0f);
				}
			}
			else if (noteDescription.Length == 0) { //normal note
				// s = vt
				note.transform.position = new Vector3 (lanePosition [lane].position.x, 0.04f, lanePosition [lane].position.z - 13f + Runner.speed * (noteDescription.HitTime - TimerScript.delay));
			} 
			else { // long note
				// s = vt
				float length = noteDescription.Length * Runner.speed;
				note.transform.localScale = new Vector3 (note.transform.localScale.x, note.transform.localScale.y, length);
				note.transform.position = new Vector3 (lanePosition [lane].position.x, 0.01f, lanePosition [lane].position.z - 13f + Runner.speed * (noteDescription.HitTime - TimerScript.delay) + length/2);
			}

			allnotes [lane].Add (noteDescription);
		}

	}

	private NoteDescription ToNoteDescription (NoteMidiEvent midiNote){
		NoteDescription tmp = new NoteDescription (midiNote.hitTime, midiNote.lane, 0 ,midiNote.length - 0.02f);
		tmp.IsFlick = midiNote.isFlick;
		return tmp;
	}

	/*
	void GenTestNote(){

		int ranNote = 5;//Random.Range (0, 10);

		if (ranNote > 0) {
			//for generating further notes, will be removed later.
			float x = 20;
			GameObject note = Instantiate (Resources.Load ("Note")) as GameObject;
			int random = Random.Range (0, 4);
			note.transform.position = new Vector3 (lanePosition [random].position.x, 0.03f, lanePosition [random].position.z - 13f + x);
			NoteDescription noteDescription = note.GetComponent<NoteDescription> ();
			//Debug.Log ("before " + noteDescription.ToString());
			// s = vt
			noteDescription = new NoteDescription (TimerScript.timePass + x / Runner.speed, random, 1, 0);
			noteDescription.NoteObject = note;
			//Debug.Log ("after " + noteDescription);
			allnotes [random].Add (noteDescription);
		} else {
			//for generating further notes, will be removed later.
			float x = 20;
			GameObject note = Instantiate (Resources.Load ("Note")) as GameObject;
			int random = Random.Range (0, 4);
			note.transform.localScale = new Vector3 (note.transform.localScale.x, note.transform.localScale.y, 4.1f);
			note.transform.position = new Vector3 (lanePosition [random].position.x, 0.03f, lanePosition [random].position.z - 13f + x + 2.0f);
			NoteDescription noteDescription = note.GetComponent<NoteDescription> ();
			//Debug.Log ("before " + noteDescription.ToString());
			// s = vt
			noteDescription = new NoteDescription (TimerScript.timePass + x / Runner.speed, random, 1, 0);
			noteDescription.NoteObject = note;
			//Debug.Log ("after " + noteDescription);
			allnotes [random].Add (noteDescription);
		}
	}
	*/


}
