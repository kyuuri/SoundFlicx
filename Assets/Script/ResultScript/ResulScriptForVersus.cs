using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResulScriptForVersus : MonoBehaviour {

	public Text songName;
	public Text difficulty;
	public Text rivalText;
	public Text winResult;
	//public Text difficulty2;


	public Text finalScore;
	public Text fantastic;
	public Text great;
	public Text good;
	public Text miss;
	public Text maxCombo;
	public Text rank;
	public Text accuracy;

	public Text finalScore2;
	public Text fantastic2;
	public Text great2;
	public Text good2;
	public Text miss2;
	public Text maxCombo2;
	public Text rank2;
	public Text accuracy2;

	private ResultScore resultScore;
	private ResultScore resultScore2;
	private Track track;

	// Use this for initialization
	void Start () {

		resultScore = GlobalData.result;
		resultScore2 = GlobalData.result2;
		track = GlobalData.selectedTrack;

		rivalText.text = "Rival Score (Lv " + GlobalData.botLv + ")";

		float sc1 = GlobalData.result.score;
		float sc2 = GlobalData.result2.score;
		if (sc1 > sc2) {
			winResult.text = "WIN";
			winResult.color = new Color (19 / 255f, (255 / 255f), 0f, 1f);
		}
		else if (sc1 < sc2) {
			winResult.text = "LOSE";
			winResult.color = new Color (255 / 255f, (23 / 255f), 0f, 1f);
		}
		else {
			winResult.text = "DRAW";
			winResult.color = new Color (255 / 255f, (247 / 255f), 0f, 1f);
		}

		if (track.songName == null) {
			songName.text = "THE CLEAR BLUE SKY";
			difficulty.text = "EASY";
			//difficulty2.text = "EASY";
		} else {
			songName.text = track.songName.Replace("_"," ") + " - " + track.composer;
			difficulty.text = track.difficulty + " (" + track.level + ")";
			//difficulty2.text = track.difficulty + " (" + track.level + ")";
		}
		if (difficulty.text.Contains("EASY")) {
			difficulty.color = new Color (19 / 255f, (255 / 255f), 0f, 1f);
			//difficulty2.color = new Color (19 / 255f, (255 / 255f), 0f, 1f);
		} else if (difficulty.text.Contains("NORMAL")) {
			difficulty.color = new Color (255 / 255f, (247 / 255f), 0f, 1f);
			//difficulty2.color = new Color (255 / 255f, (247 / 255f), 0f, 1f);
		} else if (difficulty.text.Contains("HARD")) {
			difficulty.color = new Color (255 / 255f, (23 / 255f), 0f, 1f);
			//difficulty2.color = new Color (255 / 255f, (23 / 255f), 0f, 1f);
		}


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


		finalScore2.text = resultScore2.score+"";
		fantastic2.text =   resultScore2.fantastic+"";
		fantastic2.color = Color.Lerp(Color.white, Color.yellow, 0.4f);
		great2.text =  resultScore2.great+"";
		great2.color = Color.Lerp(Color.white, Color.green, 0.4f);
		good2.text =  resultScore2.good+"";
		good2.color = Color.Lerp(Color.white, Color.blue, 0.5f);
		miss2.text =  resultScore2.miss+"";
		maxCombo2.text =  resultScore2.maxCombo+"";
		float accuractNumber2 = resultScore2.getAccuracy ()*10; 
		accuracy2.text =  string.Format("{0:0.0}", Mathf.Round(accuractNumber2)/10f);

		string ranking2 =  resultScore2.getRank ();
		rank2.text = ranking2;
		if (ranking2 == "C") {
			rank2.color = Color.green;
		} else if (ranking2 == "B") {
			rank2.color = Color.blue;
		} else if (ranking2 == "A") {
			rank2.color = Color.yellow;
		} else if (ranking2 == "S" || ranking2 == "SS" || ranking2 == "SSS") {
			rank2.color =  new Color (255/255f, (200/255f), 0f,1f);
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

