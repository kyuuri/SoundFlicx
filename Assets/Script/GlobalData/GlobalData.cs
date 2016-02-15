using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GlobalData {

	public static Track selectedTrack = new Track("The_Clear_Blue_Sky", "Tsukasa", Difficulty.HARD , 99, 178);
	public static ResultScore result = new ResultScore();
	public static List <string> descriptionList = new List<string> ();
	public static int songIndex;
	public static float speed = 2.0f;
	public static string[] textFile = new string[] {
		"Exargon,5argon & encX,170,2,6,9,0.0",
		"The_Clear_Blue_Sky,Tsukasa,178,3,6,8,0.0"
	};

}
