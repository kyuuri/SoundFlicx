using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SongSelectionController : MonoBehaviour {

	public RectTransform panel;
	public RectTransform speedPanel;
	public RectTransform contentPanel;
	public RectTransform songSelection;
	public RectTransform level;


	public Transform buttonLeft;
	public Transform buttonRight;
	private Vector3 destination = new Vector3(1000,1000,1000);

	// Use this for initialization
	void Start () {
		speedPanel.position = Vector3.Slerp (speedPanel.position, destination,5);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
