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

		finalScore.text = resultScore.score+"";
		fantastic.text =   resultScore.fantastic+"";
		great.text =  resultScore.great+"";
		good.text =  resultScore.good+"";
		miss.text =  resultScore.miss+"";
		maxCombo.text =  resultScore.maxCombo+"";
		songName.text = track.songName;
		rank.text =  resultScore.getRank ();
		accuracy.text =  resultScore.getAccuracy ()+"";
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
