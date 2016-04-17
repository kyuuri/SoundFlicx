using UnityEngine;
using System.Collections;

public class FlickNoteAnimation : MonoBehaviour {

	public Material[] materials;
	public float changeInterval = 0.03F;
	private float songbpm = 128;
	private Renderer rend;

	void Start() {
		rend = GetComponent<Renderer>();
		rend.enabled = true;
		changeInterval = 60.0f / GlobalData.selectedTrack.bpm / 5;
	}

	void Update() {
		if (materials.Length == 0)
			return;

		int index = Mathf.FloorToInt(Time.time / changeInterval);

		if (index >= 0) {
			index = index % materials.Length;
			rend.sharedMaterial = materials [index];
		}

	}
}
