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

	public void ActivateEffect(ItemController.SkillEffector skill, int level){
		if (skill == ItemController.SkillEffector.MIST_NEAR) {
			ActivateCloudDown (level);
		}
		else if (skill == ItemController.SkillEffector.MIST_FAR) {
			ActivateCloudUp (level);
		}
		else if (skill == ItemController.SkillEffector.MIST_BLIND) {
			ActivateCloudBlind (level);
		}
		else if (skill == ItemController.SkillEffector.REFLECT) {
			//ActivateReflect ();
		}
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

	private void ActivateCloudDown(int level){
		if (level < 1 || level > 3) return;
		counterDown = 0;
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

	private void ActivateCloudUp(int level){
		if (level < 1 || level > 3) return;
		counterUp = 0;
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

	private void ActivateCloudBlind(int level){
		if (level < 1 || level > 3) return;
		counterBlind = 0;
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

//	void LevelUp(){
//		cloudBlind.transform.localScale *= 1.3f; 
//	}
}
