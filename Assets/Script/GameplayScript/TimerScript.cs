using UnityEngine;
using System.Collections;

public class TimerScript : MonoBehaviour {

	public static float timePass = 0;
	
	// Update is called once per frame
	void Update () {
		timePass += Time.deltaTime;
	}
}
