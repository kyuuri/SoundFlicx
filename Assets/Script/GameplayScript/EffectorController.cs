using UnityEngine;
using System.Collections;

public class EffectorController : MonoBehaviour {

	public Camera otherLaneCamera;
	public Camera otherParCamera;

	public ParticleSystem cloudDown;
	public ParticleSystem cloudUp;
	public ParticleSystem cloudBlind;

	private Vector3 initScaleCloudDown;
	private Vector3 initScaleCloudUp;
	private Vector3 initScaleCloudBlind;

	private float counterDown;
	private float counterUp;
	private float counterBlind;

	private float downTime;
	private float upTime;
	private float blindTime;

	void Start(){
		initScaleCloudDown = cloudDown.transform.localScale;
		initScaleCloudUp = cloudUp.transform.localScale;
		initScaleCloudBlind = cloudBlind.transform.localScale;
	}

	void Update () {
		if (Input.GetKeyDown ("1")) {
			//ActivateCloudUp (1);
			//ActivateCloudDown (1);
			ActivateCloudBlind (1);
		}
//		if (Input.GetKeyDown ("2")) {
//			//ActivateCloudUp (2);
//			//ActivateCloudDown (2);
//			ActivateCloudBlind (2);
//		}
//		if (Input.GetKeyDown ("3")) {
//			//ActivateCloudUp (3);
//			//ActivateCloudDown (3);
//			ActivateCloudBlind (3);
//		}
//		if (Input.GetKeyDown ("4")) {
//			ActivateCloudUp (1);
//			ActivateCloudDown (1);
//			ActivateCloudBlind (1);
//		}

//		if (Input.GetKeyDown ("h")) {
//			LevelUp ();
//		}


		if (IsEffected ()) {
			HideOtherCamera ();
		} else {
			ShowOtherCamera ();
		}

		CheckCloudDown ();
		CheckCloudUp ();
		CheckCloudBlind ();
	}

	public bool IsEffected(){
		return cloudDown.gameObject.active || cloudUp.gameObject.active || cloudBlind.gameObject.active;
	}

	private void HideOtherCamera(){
		if (otherLaneCamera != null) {
			otherLaneCamera.gameObject.active = false;
			otherParCamera.gameObject.active = false;
		}
	}

	private void ShowOtherCamera(){
		if (otherLaneCamera != null) {
			otherLaneCamera.gameObject.active = true;
			otherParCamera.gameObject.active = true;
		}
	}

	public void ActivateCloudDown(int level){
		if (level < 1 || level > 3) return;
		downTime = (level + 1) * 4.0f;
		cloudDown.transform.localScale = initScaleCloudDown * Mathf.Pow(1.45f, (level - 1));
		cloudDown.gameObject.active = true;
	}

	void CheckCloudDown(){
		if (cloudDown.gameObject.active) {
			counterDown += Time.deltaTime;

			if (counterDown >= downTime) {
				cloudDown.gameObject.active = false;
				counterDown = 0;
			}
		}
	}

	public void ActivateCloudUp(int level){
		if (level < 1 || level > 3) return;
		upTime = (level + 1) * 4.0f;
		cloudUp.transform.localScale = initScaleCloudUp * Mathf.Pow(1.6f, (level - 1));
		cloudUp.gameObject.active = true;
	}

	void CheckCloudUp(){
		if (cloudUp.gameObject.active) {
			counterUp += Time.deltaTime;

			if (counterUp >= upTime) {
				cloudUp.gameObject.active = false;
				counterUp = 0;
			}
		}
	}

	public void ActivateCloudBlind(int level){
		if (level < 1 || level > 3) return;
		blindTime = (level + 1) * 2.5f;
		cloudBlind.transform.localScale = initScaleCloudBlind * Mathf.Pow(1.3f, (level - 1));
		cloudBlind.gameObject.active = true;
	}

	void CheckCloudBlind(){
		if (cloudBlind.gameObject.active) {
			counterBlind += Time.deltaTime;

			if (counterBlind >= blindTime) {
				cloudBlind.gameObject.active = false;
				counterBlind = 0;
			}
		}
	}

	void LevelUp(){
		cloudBlind.transform.localScale *= 1.3f; 
	}
}
