using UnityEngine;
using System.Collections;

public class UITutoManager : MonoBehaviour {



	// Use this for initialization
	void Start () {
		Debug.Log ("ICI");
	}

	// Update is called once per frame
	void Update () {
		/*if (a != null) {
			Debug.Log ("LOADING : " + a.progress);
			Debug.Log ("is done : " + a.isDone + "(" + a.progress*100f +"%)" );
		}
		if (Time.time - timeStartLoading >= 10f) {
			a.allowSceneActivation = true;
		}*/
	}

	AsyncOperation a;


	public void GoToMenuScene(){
		Debug.Log ("LA");
		Application.LoadLevelAsync ("MenuScene");
		Debug.Log ("LA");
	}
}
