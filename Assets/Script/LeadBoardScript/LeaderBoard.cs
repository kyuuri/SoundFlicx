using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class LeaderBoard {

	public PlayerInfo[] players;
	private int number = 10;
	// Use this for initialization
	public LeaderBoard () {
		players = new PlayerInfo[number];
		InitPlayer ();
	}

	public int lenght(){
		return number;
	}

	private void InitPlayer(){
		for (int i = 0; i < number; i++) {
			players [i] = new PlayerInfo ();
		}
	}
}
