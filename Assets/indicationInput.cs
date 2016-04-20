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
		PlayerManager.UpFight ();
	}

	public void TouchDown(){
		PlayerManager.DownFight ();
	}

	public void TouchLeft(){
		PlayerManager.LeftFight ();
	}

	public void TouchRight(){
		PlayerManager.RightFight ();
	}
}
