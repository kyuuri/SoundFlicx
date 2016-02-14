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
	public Text difficulty;
	public Texture2D image;
	private ResultScore resultScore;
	private Track track;

	// Use this for initialization
	void Start () {

		resultScore = GlobalData.result;
		track = GlobalData.selectedTrack;
		Debug.Log (track.songName);
		if (track.songName == null) {
			songName.text = "THE CLEAR BLUE SKY";
			difficulty.text = "EASY";
		} else {
			songName.text = track.songName;
			difficulty.text = track.difficulty+"";
		}
		Debug.Log (resultScore.score);

		finalScore.text = resultScore.score+"";
		fantastic.text =   resultScore.fantastic+"";
		great.text =  resultScore.great+"";
		good.text =  resultScore.good+"";
		miss.text =  resultScore.miss+"";
		maxCombo.text =  resultScore.maxCombo+"";
		float accuractNumber = resultScore.getAccuracy ()*10; 
		accuracy.text =  string.Format("{0:0.0}", Mathf.Round(accuractNumber)/10f);
		rank.text =  resultScore.getRank ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
