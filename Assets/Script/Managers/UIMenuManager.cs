using UnityEngine;
using System.Collections;

public class UIMenuManager : MonoBehaviour {



	// Use this for initialization
	void Start () {
		AudioManager.m_instance.PlayMenuMusic ();
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
	float timeStartLoading;

	public void GoToLevelScene(){
		GameStateManager.m_instance.setGameState (GameState.Playing);
		a =  Application.LoadLevelAsync ("LevelScene");
		AudioManager.m_instance.StopMenuBeat ();
		//a.allowSceneActivation = false;
		timeStartLoading = Time.time;
	}

	public void GoToTutoScene(){
		a =  Application.LoadLevelAsync ("TutorialScene");
	}
}
