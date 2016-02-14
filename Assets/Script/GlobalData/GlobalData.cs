using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GlobalData {

	public static Track selectedTrack = new Track("The_Clear_Blue_Sky", "Tsukasa", Difficulty.HARD , 99, 178);
	public static ResultScore result = new ResultScore();
	public static List <string> descriptionList = new List<string> ();
	public static int songIndex;
	public static float speed = 2.0f;

}
