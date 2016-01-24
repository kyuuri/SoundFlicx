using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour {

	public Transform camera;
	public static float speed = 5*2*1.5f;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		camera.transform.Translate (0, 0, Time.deltaTime * speed); // 10 meters per second
	}
}
