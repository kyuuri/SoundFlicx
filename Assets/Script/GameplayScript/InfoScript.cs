using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InfoScript : MonoBehaviour {

	public Text trackName;
	public Text composerName;
	public Text diffLvlBpm;

	// Use this for initialization
	void Start () {
		trackName.text = GlobalData.selectedTrack.songName.Replace("_"," ");
		composerName.text = GlobalData.selectedTrack.composer;
		composerName.color = Color.Lerp (Color.black, Color.white, 0.8f);
		diffLvlBpm.text = GlobalData.selectedTrack.difficulty + "  Level : " + GlobalData.selectedTrack.level + "  Bpm : " + GlobalData.selectedTrack.bpm;
		if(GlobalData.selectedTrack.difficulty == Difficulty.HARD){
			diffLvlBpm.color = Color.red;
		} else if(GlobalData.selectedTrack.difficulty == Difficulty.NORMAL){
			diffLvlBpm.color = Color.yellow;
		} else{
			diffLvlBpm.color = Color.green;
		}
		diffLvlBpm.color = Color.Lerp (diffLvlBpm.color, Color.white, 0.25f);
	}


}
