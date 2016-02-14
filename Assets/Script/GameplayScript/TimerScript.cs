using UnityEngine;
using System.Collections;

public class TimerScript : MonoBehaviour {

	public static float delay = -2;
	public static float timePass = delay;

	void Start(){
		timePass = delay;
	}
	
	// Update is called once per frame
	void Update () {
		timePass += Time.deltaTime;
	}
}
