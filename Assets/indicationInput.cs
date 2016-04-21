using UnityEngine;
using System.Collections;

public class indicationInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TouchUp(){
		Debug.Log ("Toto");
		PlayerManager.UpFight ();
	}

	public void TouchDown(){
		Debug.Log ("Toto");
		PlayerManager.DownFight ();
	}

	public void TouchLeft(){
		Debug.Log ("Toto");
		PlayerManager.LeftFight ();
	}

	public void TouchRight(){
		Debug.Log ("Toto");
		PlayerManager.RightFight ();
	}
}
