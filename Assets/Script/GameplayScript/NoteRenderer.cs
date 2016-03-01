﻿using UnityEngine;
using System.Collections.Generic;
using NAudio.Midi;
using NAudio.Utils;
using System.IO;

public class NoteRenderer : MonoBehaviour {

	public Transform[] lanePosition = new Transform[4];
	public Transform[] outLanePosition = new Transform[2]; // 0 left, 1 right
	public static List<NoteDescription>[] allnotes = new List<NoteDescription>[6];
	public static List<NoteDescription>[] rightTiltNotes = new List<NoteDescription>[1];
	public static List<NoteDescription>[] leftTiltNotes = new List<NoteDescription>[1];
	public static bool rightTilting = false;
	public static bool leftTilting = false;

	void Awake(){
		rightTilting = false;
		leftTilting = false;

		allnotes[0] = new List<NoteDescription>();
		allnotes[1] = new List<NoteDescription>();
		allnotes[2] = new List<NoteDescription>();
		allnotes[3] = new List<NoteDescription>();
		allnotes[4] = new List<NoteDescription>();
		allnotes[5] = new List<NoteDescription>();

		rightTiltNotes[0] = new List<NoteDescription>();
		leftTiltNotes[0] = new List<NoteDescription>();

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
			int actualLane = 0;

			if (noteDescription.IsRTilt || noteDescription.IsLTilt) {
				if (noteDescription.IsRTilt) {
					CreateTiltNote (noteDescription, note, ref actualLane, "R", ref rightTilting);
				} else {
					CreateTiltNote (noteDescription, note, ref actualLane, "L", ref leftTilting);
				}
			}
			else if (noteDescription.IsFlick) { // flick note
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

			if (noteDescription.IsRTilt) {
				rightTiltNotes [0].Add (noteDescription);
			} else if (noteDescription.IsLTilt) {
				leftTiltNotes [0].Add (noteDescription);
			}
			else {
				allnotes [lane].Add (noteDescription);
			}
		}

	}

	private NoteDescription ToNoteDescription (NoteMidiEvent midiNote){
		NoteDescription tmp = new NoteDescription (midiNote.hitTime, midiNote.lane, 0 ,midiNote.length - 0.02f);
		tmp.IsFlick = midiNote.isFlick;
		tmp.StartOrEnd = midiNote.startOrEnd;
		if(tmp.Lane >= 10){
			if (tmp.Lane < 12) {
				tmp.IsRTilt = true;
			} else if (tmp.Lane >= 12) {
				tmp.IsLTilt = true;
			}
		}
		return tmp;
	}

	private void CreateTiltNote(NoteDescription noteDescription, GameObject note, ref int actualLane, string side, ref bool isTilting){

		note = Instantiate (Resources.Load (side + "TiltNote")) as GameObject;
		noteDescription.NoteObject = note;
		actualLane = noteDescription.Lane - 10;
		if (side == "L") {
			actualLane -= 2;
		}
		actualLane = 1 - actualLane; 

		if (noteDescription.StartOrEnd && !isTilting) {
			isTilting = true;
			note.transform.position = new Vector3 (outLanePosition [actualLane].position.x, 0.04f, outLanePosition [actualLane].position.z - 13f + Runner.speed * (noteDescription.HitTime - TimerScript.delay));			

		} else if (noteDescription.StartOrEnd) {
			isTilting = false;
			note.transform.position = new Vector3 (outLanePosition [actualLane].position.x, 0.04f, outLanePosition [actualLane].position.z - 13f + Runner.speed * (noteDescription.HitTime - TimerScript.delay));			

			NoteDescription previousNote;

			if(side == "R"){
				previousNote = rightTiltNotes [0] [rightTiltNotes [0].Count - 1];
			}
			else{
				previousNote = leftTiltNotes [0] [leftTiltNotes [0].Count - 1];
			}
			float diffTime = noteDescription.HitTime - previousNote.HitTime;
			float diffX = noteDescription.NoteObject.transform.position.x - previousNote.NoteObject.transform.position.x;

			float ratio = 0.0075f;
			float smallNotes = diffTime / ratio;
			float smallX = diffX / smallNotes;

			int k = 1;
			for (float j = ratio; j < diffTime; j += ratio) {
				NoteDescription noteDesc = new NoteDescription (previousNote.HitTime + j, -1, 1, 0);
				noteDesc.IsRTilt = side == "R";
				noteDesc.IsLTilt = side == "L";
				GameObject obj = Instantiate (Resources.Load (side + "TiltNote")) as GameObject;
				noteDesc.NoteObject = obj;
				obj.transform.position = new Vector3 (previousNote.NoteObject.transform.position.x + smallX * k, 0.04f, outLanePosition [actualLane].position.z - 13f + Runner.speed * (noteDesc.HitTime - TimerScript.delay));			
				++k;
			}
		} else {
			note.transform.position = new Vector3 (outLanePosition [actualLane].position.x, 0.04f, outLanePosition [actualLane].position.z - 13f + Runner.speed * (noteDescription.HitTime - TimerScript.delay));			

			NoteDescription previousNote;

			if(side == "R"){
				previousNote = rightTiltNotes [0] [rightTiltNotes [0].Count - 1];
			}
			else{
				previousNote = leftTiltNotes [0] [leftTiltNotes [0].Count - 1];
			}
			float diffTime = noteDescription.HitTime - previousNote.HitTime;
			float diffX = noteDescription.NoteObject.transform.position.x - previousNote.NoteObject.transform.position.x;

			float ratio = 0.0075f;
			float smallNotes = diffTime / ratio;
			float smallX = diffX / smallNotes;

			int k = 1;
			for (float j = ratio; j < diffTime; j += ratio) {
				NoteDescription noteDesc = new NoteDescription (previousNote.HitTime + j, -1, 1, 0);
				noteDesc.IsRTilt = side == "R";
				noteDesc.IsLTilt = side == "L";
				GameObject obj = Instantiate (Resources.Load (side + "TiltNote")) as GameObject;
				noteDesc.NoteObject = obj;
				obj.transform.position = new Vector3 (previousNote.NoteObject.transform.position.x + smallX * k, 0.04f, outLanePosition [actualLane].position.z - 13f + Runner.speed * (noteDesc.HitTime - TimerScript.delay));			
				++k;
			}
		}
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
