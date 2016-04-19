using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WindowsInput;

public class AutoPlayScript : MonoBehaviour {

	public static AutoPlayScript Instance { get; private set;}

	public PlayInformation playInformation;

	public bool isAutoTest = false;
	public bool isOn = true;
	private List<NoteDescription>[] allnotes;
	private List<NoteDescription> rightTiltNotes;
	private List<NoteDescription> leftTiltNotes;

	private List<NoteDescription>[] hitNotes;

	private List<NoteDescription> notes;
	private List<NoteDescription> tiltNotes;

	public float percent = 100.0f;
	public int level = 0;

	private float[] levelPercent = { 0, 51, 60, 70, 78, 86.5f, 100};
	//private float[] levelPercent = { 0, 47, 55, 63, 71, 79, 87, 100};
	//100 ALL perfect

	//85 Level MX
	//77 Level 5
	//69 Level 4
	//61 Level 3
	//53 Level 2
	//45 Level 1

	public LineHitChecker b1;
	public LineHitChecker b2;
	public LineHitChecker b3;
	public LineHitChecker b4;

	public LineFlickChecker f1;
	public LineFlickChecker f2;

	public LineRTiltChecker rt;
	public LineLTiltChecker lt;

	private float holdTime;
	private float holdTimeCounter;


	void Awake(){
		Instance = this;
	}

	void Start () {
		allnotes = playInformation.noteRenderer.allnotes;
		rightTiltNotes = playInformation.noteRenderer.rightTiltNotes[0];
		leftTiltNotes = playInformation.noteRenderer.leftTiltNotes[0];

		if(!isAutoTest){
			level = GlobalData.botLv;
			percent = levelPercent [level];
		}

		hitNotes = new List<NoteDescription>[6];
		hitNotes[0] = new List<NoteDescription> ();
		hitNotes[1] = new List<NoteDescription> ();
		hitNotes[2] = new List<NoteDescription> ();
		hitNotes[3] = new List<NoteDescription> ();
		hitNotes[4] = new List<NoteDescription> ();
		hitNotes[5] = new List<NoteDescription> ();
	}

	private float RamdomValue(){
		return (float) ((0.4 * Random.value * (100 - percent) / 100.0f) * Mathf.Pow (-1, Random.Range (0, 2)));
	}

	private bool CheckHitNote(int index, NoteDescription note){
		for(int i = 0 ; i < hitNotes[index].Count ; i++){
			NoteDescription other = hitNotes [index] [i];
			if (note.NoteID == other.NoteID) {
				return true;
			}
		}
		return false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isOn) {
			for (int i = 0; i < allnotes.Length; i++) {
				notes = allnotes [i];
				if (notes.Count > 0) {
					NoteDescription note = notes [0];

					if (note.Length > 0 && TimerScript.timePass >= note.HitTime + note.Length - 0.07f) {
						KeyUp (i);
					}
					if (note.Length > 0 && TimerScript.timePass >= note.HitTime - 0.035f + RamdomValue () && TimerScript.timePass < note.HitTime + note.Length - 0.075f) {
						if (!CheckHitNote(i, note) || TimerScript.timePass < note.HitTime + note.Length - 0.075f) {
							KeyDown (i);
							if (!CheckHitNote (i, note)) {
								hitNotes [i].Add (note);
							}
						}
					} else if (TimerScript.timePass >= note.HitTime - 0.035f + RamdomValue ()) {
						if (!CheckHitNote(i, note)) {
							KeyDown (i);
							hitNotes [i].Add (note);
						}
					} else if (note.IsFlick && TimerScript.timePass > note.HitTime) {
						KeyUp (i);
					} else {
						KeyUp (i);
					}
				} else {
					KeyUp (i);
				}
			}

			CheckTilt("R");
			CheckTilt("L");

			CheckItem ();
			CheckPercentOnEffect ();
		}
	}

	void CheckTilt(string side){
		if (side == "R") {
			tiltNotes = rightTiltNotes;
		} else {
			tiltNotes = leftTiltNotes;
		}

		NoteDescription note = null;

		for (int i = 0; i < tiltNotes.Count; i++) {
			note = tiltNotes [i];
			if (note.Length > 0) {
				if (TimerScript.timePass > note.HitTime + note.Length - 0.02f) {
					if (i + 2 < tiltNotes.Count - 2) {
						note = notes [i + 2];
						if (note.Length == 0) {
							continue;
						}
					}
				}
				break;
			}
		}

		if (tiltNotes.Count != 0) {
			if (TimerScript.timePass >= note.HitTime - 0.05f && TimerScript.timePass < note.HitTime + note.Length - 0.01f) {
				TiltKeyDown (note);
			}
		}
	}

	void CheckItem(){
		if (level <= 1) {
			if (playInformation.itemController.HasSkill ()) {
				playInformation.itemController.UseItem ();
			}

			if (playInformation.effector.IsEffected ()) {

				ItemController.SkillEffector[] skills = playInformation.itemController.GetItems ();
				int index = SearchReflect (skills);
				if (index > -1) {
					for (int i = skills.Length - 1; i >= 0; i--) {
						if ((int)skills [i] == 0) {
							continue;
						}
						else if ((int)skills [i] != 1) {
							playInformation.itemController.UseItem ();
						}
						else if ((int)skills [i] == 1) {
							playInformation.itemController.UseItem ();
							break;
						}
					}
				}
			} 
		} else {
			ItemController.SkillEffector[] skills = playInformation.itemController.GetItems ();
			//always reflect
			if (playInformation.effector.IsEffected ()) {
				int index = SearchReflect (skills);
				if (index > -1) {
					for (int i = skills.Length - 1; i >= 0; i--) {
						if ((int)skills [i] == 0) {
							continue;
						}
						else if ((int)skills [i] != 1) {
							playInformation.itemController.UseItem ();
						}
						else if ((int)skills [i] == 1) {
							playInformation.itemController.UseItem ();
							break;
						}
					}
				}
			} 

			//3 slot
			if ((int)skills [2] != 0) {
				if (SearchReflect (skills) > -1) {
					if ((int)skills [0] == 1 && (int)skills [1] == 1) {
						playInformation.itemController.UseItem ();
						// waste reflect;
					}
				}	
			}

			if (holdTimeCounter >= holdTime) {
				holdTime = Random.value * 13 + 3;
				holdTimeCounter = 0;

				int numItem = Random.Range (0,4);

				for (int i = 0; i < numItem; i++) {
					if ((int)playInformation.itemController.PeekFirstItem () != 1) {
						playInformation.itemController.UseItem ();
					}
				}
			}
			holdTimeCounter += Time.deltaTime;
		}
	}

	void CheckPercentOnEffect(){
		if(!isAutoTest){
			if (playInformation.effector.IsEffected ()) {
				int downlvl = playInformation.effector.downLevel;
				int uplvl = playInformation.effector.upLevel;
				int blindlvl = playInformation.effector.blindLevel;
				float dec = 0;

				// 2 4 7
				if (downlvl == 3)
					dec += 9;
				else
					dec += downlvl * 2.5f;
				
				// 2 4 7
				if (uplvl == 3)
					dec += 9;
				else
					dec += uplvl * 2.5f;


				// 2.5 5 8
				dec += blindlvl * 3.2f;
				if (blindlvl == 3)
					dec += 1.0f;

				percent = levelPercent [level] - dec;
			}
		}
	}

	private int SearchReflect(ItemController.SkillEffector[] skills){
		for (int i = skills.Length - 1; i >= 0; i--) {
			if ((int)skills [i] == 1) {
				return i;
			}
		}
		return -1;
	}

	void TiltKeyDown (NoteDescription note){
		if (note.IsRTilt) {
			if (note.TiltAngle > 0.1f) {
				rt.KeyRToR ();
			} else if (note.TiltAngle < -0.1f) {
				rt.KeyRToL ();
			} else if (note.TiltAngle >= -0.1f && note.TiltAngle <= 0.1f) {
				rt.KeyRIDLE ();
			}
		} else {
			if (note.TiltAngle > 0.1f) {
				lt.KeyLToR ();
			} else if (note.TiltAngle < -0.1f) {
				lt.KeyLToL ();
			} else if (note.TiltAngle >= -0.1f && note.TiltAngle <= 0.1f) {
				lt.KeyLIDLE ();
			}
		}
	}

//	void TiltKeyUp (NoteDescription note){
//		if (note.IsRTilt) {
//			rt.KeyRIDLE ();
//		} else {
//			;
//		}
//	}

	void KeyDown(int i){
		if (i == 0) {
			b1.KeyDown ();
		}
		else if (i == 1) {
			b2.KeyDown ();
		}
		else if (i == 2) {
			b3.KeyDown ();
		}
		else if (i == 3) {
			b4.KeyDown ();
		}
		else if (i == 4) {
			f2.KeyFlick ();
			//InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_X);
		}
		else if (i == 5) {
			f1.KeyFlick ();
			//InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_Z);
		}
	}

	void KeyUp(int i){
		if (i == 0) {
			b1.KeyUp ();
		}
		else if (i == 1) {
			b2.KeyUp ();
		}
		else if (i == 2) {
			b3.KeyUp ();
		}
		else if (i == 3) {
			b4.KeyUp ();
		}
		else if (i == 4) {
			f2.KeyNone ();
			//InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_X);
		}
		else if (i == 5) {
			f1.KeyNone ();
			//InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_Z);
		}
	}
}
