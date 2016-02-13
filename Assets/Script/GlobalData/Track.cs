using UnityEngine;
using System.Collections;

public class Track : MonoBehaviour {

	public string songName;
	public string composer;
	public Difficulty difficulty;
	public int level;
	public float bpm;
	public float offset;

	public Track(string songName, string composer,Difficulty dificulty, int level, float bpm){
		this.songName = songName;
		this.composer = composer;
		this.difficulty = dificulty;
		this.level = level;
		this.bpm = bpm;
	}

	public Track(){}

}
