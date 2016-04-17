using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GlobalData {

	//public static Track selectedTrack = new Track("The_Clear_Blue_Sky", "Tsukasa", Difficulty.HARD , 99, 178);
	public static Track selectedTrack = new Track("Exargon", "5argon & Encx", Difficulty.HARD , 9, 170, -0.082f);
	public static ResultScore result = new ResultScore();
	public static ResultScore result2 = new ResultScore();
	public static List <string> descriptionList = new List<string> ();
	public static int songIndex;
	public static bool isVersus = false;
	public static int botLv = 3;
	public static float speed = 1.0f;
	public static string[] textFile = new string[] {
		"Tutorial,Tutorial,120,0,0,0,-0.075",
		"Exargon,5argon & encX,170,2,6,9,-0.082",
		"The_Clear_Blue_Sky,Tsukasa,178,3,6,8,0.0",
		"Little_Witch,Pepper Peach,128,1,7,10,-0.17"
	};

}
