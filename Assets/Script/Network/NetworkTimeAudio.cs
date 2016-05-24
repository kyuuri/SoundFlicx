using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkTimeAudio : NetworkBehaviour {

	public GameObject timer;
	public GameObject audio;

	public override void OnStartServer(){

		GameObject time = (GameObject)Instantiate (timer, Vector3.zero, new Quaternion (0, 0, 0, 0));
		GameObject aud = (GameObject)Instantiate (audio, Vector3.zero, new Quaternion (0, 0, 0, 0));
		Debug.Log ("Net work spawn");
		NetworkServer.Spawn (time);
		NetworkServer.Spawn (aud);
	}

	// Use this for initialization
	void Start () {
		Debug.Log("Start NetworkTime");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
