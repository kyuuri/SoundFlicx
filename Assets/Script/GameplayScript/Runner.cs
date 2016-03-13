using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour {

	public Transform camera;
	public static float speed = 5 * GlobalData.speed;

	void Awake(){
		Application.targetFrameRate = 90;
		speed = 5 * 2 * GlobalData.speed;
		camera.transform.position = new Vector3 (0,1,0);
	}

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		camera.transform.Translate (0, 0, Time.deltaTime * speed); // 10 meters per second
	}
}
