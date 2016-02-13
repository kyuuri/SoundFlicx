using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResultScript : MonoBehaviour {

	public Text finalScore;
	public Text fantastic;
	public Text great;
	public Text good;
	public Text miss;
	public Text maxCombo;
	public Text songName;
	public Text rank;
	public Text accuracy;
	public Texture2D image;
	private ResultScore resultScore;
	private Track track;

	// Use this for initialization
	void Start () {

		resultScore = GlobalData.result;
		track = GlobalData.selectedTrack;

		Debug.Log (resultScore.score);

		finalScore.text = "FINAL SCORE "+resultScore.score;
		fantastic.text =  "FANTASTIC   " + resultScore.fantastic;
		great.text = "GREAT       " + resultScore.great;
		good.text = "GOOD        " + resultScore.good;
		miss.text = "MISS        " + resultScore.miss;
		maxCombo.text =  "MAX COMBO   " + resultScore.maxCombo;
		songName.text = track.songName;
		accuracy.text = "Accuracy " + resultScore.getAccuracy ();
		rank.text = "Rank   " + resultScore.getRank ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
