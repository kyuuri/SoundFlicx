﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemController : MonoBehaviour {

	public enum SkillEffector {NONE, REFLECT, MIST_NEAR, MIST_FAR, MIST_BLIND}

	public int playerNumber;
	public EffectorController otherEffector;
	public RawImage[] slotImages = new RawImage[3];
	public ParticleSystem[] particles = new ParticleSystem[3];
	public ParticleSystem[] particlesLevel = new ParticleSystem[3];

	public AudioSource getSound;
	public AudioSource useSound;

	private SkillEffector[] skills;

	private Texture[] textures;


	void Start(){
		skills = new SkillEffector[3];
		skills [0] = SkillEffector.NONE;
		skills [1] = SkillEffector.NONE;
		skills [2] = SkillEffector.NONE;

		textures = new Texture[5];

		for (int i = 0; i < textures.Length; i++) {
			textures[i] = Resources.Load ("Effector/skill" + (int)(i)) as Texture;
		}
	}

	public bool HasSkill(){
		for (int i = 0; i < skills.Length; i++) {
			if ((int)skills [i] != 0) {
				return true;
			}
		}
		return false;
	}

	public SkillEffector[] GetItems(){
		return skills;
	}

	public SkillEffector PeekFirstItem(){
		for (int i = skills.Length - 1; i >= 0; i--) {
			if ((int)skills [i] != 0) {
				return skills [i];
			}
		}
		return SkillEffector.NONE;
	}

	public void GetItem(){
		float x = Random.value * 100.0f;

		if (IsSkillFree()) {
			//18 30 30 22
			if (x <= 18) {
				AddSkill (1);
			} else if (x <= 40) {
				AddSkill (4);
			} else {
				AddSkill(Random.Range(2,4));
			}
			if (getSound != null) {
				getSound.Play ();
			}
			UpdateSkillArr ();
		}
	}

	public void UseItem(){
		int usingSkill = 0;
		int level = 0;

		//like recursively use the skill if it stacks
		for (int i = skills.Length - 1; i >= 0; i--) {
			if ((int)skills [i] != 0) {
				usingSkill = (int)skills [i];
				skills [i] = SkillEffector.NONE;
				slotImages [i].texture = textures[0];
				level = 1;
				if (particles [i] != null) {
					particles [i].Play ();
				}

				if (i - 1 >= 0) {
					if (usingSkill != 1 && usingSkill == (int)skills [i - 1]) {
						skills [i-1] = SkillEffector.NONE;
						slotImages [i-1].texture = textures[0];
						level = 2;
						if (particles [i - 1] != null) {
							particles [i - 1].Play ();
						}

						if (i - 2 >= 0) {
							if (usingSkill == (int)skills [i - 2]) {
								skills [i-2] = SkillEffector.NONE;
								slotImages [i-2].texture = textures[0];
								level = 3;
								if (particles [i - 2] != null) {
									particles [i - 2].Play ();
								}
							}
						}
					}
				}
				if (level != 0) {
					UpdateSkillArr ();
					otherEffector.ActivateEffect ((SkillEffector)usingSkill, level);
					if (useSound != null) {
						useSound.Play ();
					}
					break;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (playerNumber == 1) {
			if (Input.GetKeyDown ("6")) {
				GetItem ();
			}

			if (Input.GetKeyDown ("space")) {
				UseItem ();
			}
		}

		UpdateImage ();
		if (playerNumber == 1) {
			UpdateAdjacent ();
		}
	}

	void UpdateSkillArr(){

		for (int i = 0; i < skills.Length; i++) {
			int sk = (int)skills [i];
			slotImages [i].texture = textures [sk];
		}
	}

	void UpdateImage(){
		for (int i = 0; i < slotImages.Length; i++) {
			if (slotImages [i].texture.name == "skill0") {
				Color c = slotImages [i].color;
				slotImages [i].color = new Color (c.r, c.g, c.b, 0);
			} else {
				Color c = slotImages [i].color;
				if(c.a < 1.0f){
					slotImages [i].color = new Color (c.r, c.g, c.b, c.a + 0.075f);
				}
			}
		}
	}

	void UpdateAdjacent (){
		if ((int)skills [0] == (int)skills [1] && (int)skills [1] == (int)skills [2] && (int)skills [0] != 0 && (int)skills [0] != 1) {
			particlesLevel [0].gameObject.active = true;
			particlesLevel [1].gameObject.active = true;
			particlesLevel [2].gameObject.active = true;
		} else if ((int)skills [0] == (int)skills [1] && (int)skills [0] != 0 && (int)skills [0] != 1) {
			particlesLevel [0].gameObject.active = true;
			particlesLevel [1].gameObject.active = true;
		} else if ((int)skills [1] == (int)skills [2] && (int)skills [1] != 0 && (int)skills [1] != 1) {
			particlesLevel [1].gameObject.active = true;
			particlesLevel [2].gameObject.active = true;
		} else {
			particlesLevel [0].gameObject.active = false;
			particlesLevel [1].gameObject.active = false;
			particlesLevel [2].gameObject.active = false;
		}
	}

	void AddSkill(int sk){
		for (int i = 0; i < skills.Length; i++) {
			if ((int)skills [i] == 0) {
				skills [i] = (SkillEffector)sk;
				break;
			}
		}
	}

	bool IsSkillFree(){
		for (int i = 0; i < skills.Length; i++) {
			if ((int)skills [i] == 0) {
				return true;
			}
		}
		return false;
	}

//	class SkillSlot{
//
//		private Image skillImage;
//		private SkillEffector skill = SkillEffector.NONE;
//
//		public SkillSlot(){}
//
//		public void UpdateSelf(){
//			if (skill == SkillEffector.NONE) {
//				Color c = skillImage.color;
//				skillImage.color = new Color (c.r,c.g,c.b,0);
//			}
//		}
//	}
}
