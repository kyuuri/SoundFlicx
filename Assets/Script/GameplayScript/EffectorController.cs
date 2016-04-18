using UnityEngine;
using System.Collections;

public class EffectorController : MonoBehaviour {

	public Camera otherLaneCamera;
	public Camera otherParCamera;

	public ParticleSystem cloudDown;
	public ParticleSystem cloudUp;
	public ParticleSystem cloudBlind;
	public ParticleSystem reflect1;
	public ParticleSystem reflect2;

	public EffectorController otherEffector;

	private Vector3 initScaleCloudDown;
	private Vector3 initScaleCloudUp;
	private Vector3 initScaleCloudBlind;

	private float counterDown;
	private float counterUp;
	private float counterBlind;

	private float downTime;
	private float upTime;
	private float blindTime;

	public int downLevel = 0;
	public int upLevel = 0;
	public int blindLevel = 0;

	void Start(){
		initScaleCloudDown = cloudDown.transform.localScale;
		initScaleCloudUp = cloudUp.transform.localScale;
		initScaleCloudBlind = cloudBlind.transform.localScale;

		downLevel = 0;
		upLevel = 0;
		blindLevel = 0;
			
		counterDown = 0;
		counterUp = 0;
		counterBlind = 0;

		downTime = 0;
		upTime = 0;
		blindTime = 0;
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
			ActivateReflect ();
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
		downLevel = level;
		counterDown = 0;
		downTime = (level + 1) * 3.5f;
		cloudDown.transform.localScale = initScaleCloudDown * Mathf.Pow(1.40f, (level - 1));
		cloudDown.gameObject.active = true;
	}

	void CheckCloudDown(){
		if (cloudDown.gameObject.active) {
			counterDown += Time.deltaTime;

			if (counterDown >= downTime) {
				cloudDown.gameObject.active = false;
				counterDown = 0;
				downLevel = 0;
			}
		}
	}

	private void ActivateCloudUp(int level){
		if (level < 1 || level > 3) return;
		upLevel = level;
		counterUp = 0;
		upTime = (level + 1) * 3.5f;
		cloudUp.transform.localScale = initScaleCloudUp * Mathf.Pow(1.58f, (level - 1));
		cloudUp.gameObject.active = true;
	}

	void CheckCloudUp(){
		if (cloudUp.gameObject.active) {
			counterUp += Time.deltaTime;

			if (counterUp >= upTime) {
				cloudUp.gameObject.active = false;
				counterUp = 0;
				upLevel = 0;
			}
		}
	}

	private void ActivateCloudBlind(int level){
		if (level < 1 || level > 3) return;
		blindLevel = level;
		counterBlind = 0;
		blindTime = (level + 1) * 2.2f;
		cloudBlind.transform.localScale = initScaleCloudBlind * Mathf.Pow(1.3f, (level - 1));
		cloudBlind.gameObject.active = true;
	}

	void CheckCloudBlind(){
		if (cloudBlind.gameObject.active) {
			counterBlind += Time.deltaTime;

			if (counterBlind >= blindTime) {
				cloudBlind.gameObject.active = false;
				counterBlind = 0;
				blindLevel = 0;
			}
		}
	}

	private void ActivateReflect (){
		reflect1.Play ();
		reflect2.Play ();
		reflect1.startLifetime = reflect1.startLifetime;
		reflect2.startLifetime = reflect2.startLifetime;

		if (otherEffector.IsEffected ()) {
			if (otherEffector.downLevel > 0) {
				ActivateEffect (ItemController.SkillEffector.MIST_NEAR, otherEffector.downLevel);
			}
			if (otherEffector.upLevel > 0) {
				ActivateEffect (ItemController.SkillEffector.MIST_FAR, otherEffector.upLevel);
			}
			if (otherEffector.blindLevel > 0) {
				ActivateEffect (ItemController.SkillEffector.MIST_BLIND, otherEffector.blindLevel);
			}

			otherEffector.cloudDown.gameObject.active = false;
			otherEffector.cloudUp.gameObject.active = false;
			otherEffector.cloudBlind.gameObject.active = false;
		}
	}

//	void LevelUp(){
//		cloudBlind.transform.localScale *= 1.3f; 
//	}
}
