using UnityEngine;
using System.Collections;

public class AudioSpectrum : MonoBehaviour {
	public GameObject prefab;
	public int numberOfObjects = 24;
	public float radius = 5f;

	public GameObject[] cube;

	void Start() {
		for (int i = 0; i < numberOfObjects/2; i++) {
			Vector3 pos = new Vector3 ((i-numberOfObjects/2)/4.0f,0,0);
			GameObject obj = (GameObject)Instantiate(prefab, pos, Quaternion.identity);
			obj.GetComponent<Renderer> ().material.color = Color.HSVToRGB (i / ((numberOfObjects / 2.0f) - 1), 0.7f, 0.7f );

			pos = new Vector3 (-(i-numberOfObjects/2)/4.0f,0,0);
			obj = (GameObject)Instantiate(prefab, pos, Quaternion.identity);
			obj.GetComponent<Renderer> ().material.color = Color.HSVToRGB (i / ((numberOfObjects / 2.0f) - 1), 0.7f, 0.7f );


		}

		cube = GameObject.FindGameObjectsWithTag ("cubes");
	}


	
	// Update is called once per frame
	void Update () {
		float[] spectrum = AudioListener.GetSpectrumData (1024, 0, FFTWindow.Hamming);
		for (int i = 0; i < numberOfObjects; i++) {
			Vector3 previousScale = cube [i].transform.localScale;
			previousScale.y = spectrum [i] * 40;
			cube [i].transform.localScale = previousScale;
		}
	}
}
