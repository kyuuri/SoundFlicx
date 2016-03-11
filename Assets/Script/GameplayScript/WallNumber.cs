using UnityEngine;
using System.Collections;

public class WallNumber : MonoBehaviour {

	public ParticleSystem[] pars = new ParticleSystem[6];
	private float t = 0;

	// Use this for initialization
	void Start () {
		float bpm = GlobalData.selectedTrack.bpm;
		pars [0].startSpeed = (int)(bpm / 2.5);
		pars [1].startSpeed = (int)(bpm / 2.5);

		for (int i = 2; i < pars.Length; i++) {
			
			pars [i].emissionRate = Random.Range (3, 5);
			pars [i].startSpeed = (int)(bpm / 3);
		}
	}
	
	// Update is called once per frame
	void Update () {
		t += Time.deltaTime;

		if (t >= 3.0f) {
			for (int i = 2; i < pars.Length; i++) {
				pars [i].emissionRate = Random.Range (3, 6);
			}
			t = 0;
		}
	}
}
