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
		if (difficulty.text == "EASY") {
			difficulty.color = new Color (19 / 255f, (255 / 255f), 0f, 1f);
		} else if (difficulty.text == "NORMAL") {
			difficulty.color = new Color (255 / 255f, (247 / 255f), 0f, 1f);
		} else if (difficulty.text == "HARD") {
			difficulty.color = new Color (255 / 255f, (23 / 255f), 0f, 1f);
		}

		Debug.Log (resultScore.score);

		finalScore.text = resultScore.score+"";
		fantastic.text =   resultScore.fantastic+"";
		fantastic.color = Color.Lerp(Color.white, Color.yellow, 0.4f);
		great.text =  resultScore.great+"";
		great.color = Color.Lerp(Color.white, Color.green, 0.4f);
		good.text =  resultScore.good+"";
		good.color = Color.Lerp(Color.white, Color.blue, 0.5f);
		miss.text =  resultScore.miss+"";
		maxCombo.text =  resultScore.maxCombo+"";
		float accuractNumber = resultScore.getAccuracy ()*10; 
		accuracy.text =  string.Format("{0:0.0}", Mathf.Round(accuractNumber)/10f);

		string ranking =  resultScore.getRank ();
		rank.text = ranking;
		if (ranking == "C") {
			rank.color = Color.green;
		} else if (ranking == "B") {
			rank.color = Color.blue;
		} else if (ranking == "A") {
			rank.color = Color.yellow;
		} else if (ranking == "S" || ranking == "SS" || ranking == "SSS") {
			rank.color =  new Color (255/255f, (200/255f), 0f,1f);
		}

	}

	public void retry(){
		UnityEngine.Application.LoadLevel("Gameplay");
	}

	public void done(){
		UnityEngine.Application.LoadLevel("SongSelection");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
