using UnityEngine;
using System.Collections;

public class NoteDescription : MonoBehaviour {

	public enum NoteHitState {READY, HIT, MISSED} //0, 1, 2

	private GameObject noteObject;
	public static int noteIDRunner = 1;
	private int noteID;
	private float hitTime;
	private int lane; // 0 leftmost
	private int combo; // default = 1
	private float length; // 0, normal note
	private NoteHitState noteState; //0 ready, 1 passed, 2 missed

	private int addedCombo; // default = 0 (combo that has been added)
	private float[] eachComboTime;
	private bool[] eachComboAdded;
	public static float ticker = 0.07f;

	public NoteDescription(float time, int ln, int com, float len){
		noteID = noteIDRunner++;
		hitTime = time;
		lane = ln;
		if (len > 0) {
			length = len;
			combo = (int)(len / ticker);
			if (combo < 1)
				combo = 1;
		} else {
			combo = 1;
			length = 0;
		}
		noteState = NoteHitState.READY;

		eachComboTime = new float[combo];
		eachComboAdded = new bool[combo];
		InitEachComboTime();
		//Debug.Log (this.ToString());
	}

	void InitEachComboTime(){
		for (int i = 0; i < combo; i++) {
			eachComboTime [i] = hitTime + ticker*i;
		}
	}

	public void DestroySelf(){
		Destroy (noteObject, 0.01f);
	}

	public GameObject NoteObject{
		get { return noteObject;}
		set { noteObject = value;}
	}

	public int NoteID{
		get { return noteID;}
		set { noteID = value;}
	}

	public float HitTime{
		get { return hitTime;}
		set { hitTime = value;}
	}

	public int Lane{
		get { return lane;}
		set { lane = value;}
	}

	public int Combo{
		get { return combo;}
		set { combo = value;}
	}

	public float Length{
		get { return length;}
		set { length = value;}
	}

	public float[] EachComboTime{
		get { return eachComboTime;}
	}

	public bool[] EachComboAdded{
		get { return eachComboAdded;}
	}

	public NoteHitState NoteState{
		get { return noteState;}
		set { noteState = value;}
	}

	public string ToString(){
		return "Note ID: " + noteID + " Time: " + hitTime + " Lane: " + lane + " Combo: " + combo + " Len: " + length;
	}

}
