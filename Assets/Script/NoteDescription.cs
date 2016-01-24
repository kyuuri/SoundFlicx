using UnityEngine;
using System.Collections;

public class NoteDescription : MonoBehaviour {

	private GameObject noteObject;
	public static int noteIDRunner = 1;
	private int noteID {get; set;}
	private float hitTime {get; set;}
	private int lane {get; set;} // 0 leftmost
	private int combo {get; set;} // default = 1
	private float length {get; set;} // 0, normal note
	private int noteState {get; set;} //0 ready, 1 passed, 2 missed

	public NoteDescription(float time, int ln, int com, float len){
		noteID = noteIDRunner++;
		hitTime = time;
		lane = ln;
		if (len > 0) {
			length = len;
			combo = 2;
		} else {
			combo = 1;
			length = 0;
		}
		noteState = 0;
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

	public int NoteState{
		get { return noteState;}
		set { noteState = value;}
	}

	public string ToString(){
		return "Note ID: " + noteID + " Time: " + hitTime + " Lane: " + lane + " Combo: " + combo + " Len: " + length;
	}

}
