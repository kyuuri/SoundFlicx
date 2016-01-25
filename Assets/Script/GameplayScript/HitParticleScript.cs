using UnityEngine;
using System.Collections;

public class HitParticleScript : MonoBehaviour {

	public GameObject obj;
	public ParticleSystem ps;

	// Use this for initialization
	void Start () {
		ps = obj.GetComponent<ParticleSystem> ();
		ps.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!ps.IsAlive ()) {
			Destroy (obj);
		}
	}
}
