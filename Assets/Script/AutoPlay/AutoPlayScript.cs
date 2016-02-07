using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WindowsInput;

public class AutoPlayScript : MonoBehaviour {

	public static AutoPlayScript Instance { get; private set;}
	public bool isOn = true;
	private List<NoteDescription>[] allnotes;


	void Awake(){
		Instance = this;
	}

	void Start () {
		allnotes = NoteRenderer.allnotes;
	}
	
	// Update is called once per frame
	void Update () {
		if (isOn) {
			for (int i = 0; i < allnotes.Length; i++) {
				List<NoteDescription> notes = allnotes [i];
				if (notes.Count > 0) {
					NoteDescription note = notes [0];

					if (note.Length > 0 && TimerScript.timePass >= note.HitTime + note.Length - 0.08f) {
						KeyUp (i);
					}
					if (note.Length > 0 && TimerScript.timePass >= note.HitTime - 0.035f && TimerScript.timePass < note.HitTime + note.Length - 0.075f) {
						KeyDown (i);
					} else if (TimerScript.timePass >= note.HitTime - 0.035f) {
						KeyDown (i);
					} else {
						KeyUp (i);
					}
				}
			}
		}
	}

	void KeyDown(int i){
		if (i == 0) {
			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_Q);
		}
		else if (i == 1) {
			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_W);
		}
		else if (i == 2) {
			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_E);
		}
		else if (i == 3) {
			InputSimulator.SimulateKeyDown (VirtualKeyCode.VK_R);
		}
	}

	void KeyUp(int i){
		if (i == 0) {
			InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_Q);
		}
		else if (i == 1) {
			InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_W);
		}
		else if (i == 2) {
			InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_E);
		}
		else if (i == 3) {
			InputSimulator.SimulateKeyUp (VirtualKeyCode.VK_R);
		}
	}
}
