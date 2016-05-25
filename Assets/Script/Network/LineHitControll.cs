using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LineHitControll : NetworkBehaviour {

	public LineHitChecker line1;
	public LineHitChecker line2;
	public LineHitChecker line3;
	public LineHitChecker line4;

	// Use this for initialization
	void Start () {
		line1.isLocal = isLocalPlayer;
		line2.isLocal = isLocalPlayer;
		line3.isLocal = isLocalPlayer;
		line4.isLocal = isLocalPlayer;

	}
	
	// Update is called once per frame
	void Update () {
		line1.isLocal = isLocalPlayer;
		line2.isLocal = isLocalPlayer;
		line3.isLocal = isLocalPlayer;
		line4.isLocal = isLocalPlayer;




	}
}
