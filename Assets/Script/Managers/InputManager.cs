using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	#region Singleton
	private static InputManager m_instance;
	void Awake(){
		if(m_instance == null){
			//If I am the first instance, make me the Singleton
			m_instance = this;
			DontDestroyOnLoad(this.gameObject);
		}else{
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
			if(this != m_instance)
				Destroy(this.gameObject);
		}
	}
	#endregion Singleton

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		switch (GameStateManager.getGameState ()) {
		case GameState.Menu:
			UpdateMenuState();
			break;
		case GameState.Playing:
			UpdatePlayingState();
			break;
		case GameState.Pause:
			UpdatePauseState();
			break;
		case GameState.GameOver:
			UpdateGameOverState();
			break;
		}
	}

	void UpdateMenuState(){
		if(Input.GetKeyDown(KeyCode.Return)){
			GameStateManager.m_instance.setGameState (GameState.Playing);
			Application.LoadLevelAsync ("LevelScene");
		}
	}

	void UpdatePlayingState(){
		if(Input.GetKeyDown("p")){
			Debug.Log("PAUSE ! ");
			GameStateManager.m_instance.setGameState(GameState.Pause);
		}

		//jmorel
		if(Input.GetKey(KeyCode.UpArrow)  || Input.GetKey ("z") || Input.GetKey ("w") || (Mathf.Abs(Input.acceleration.y) > 0.1f)){
			PlayerManager.UP();
		}


		if (Input.GetKeyDown ("z") || Input.GetKeyDown ("w") || Input.GetKeyDown (KeyCode.UpArrow)) {
			PlayerManager.UpFight ();
		}
		//morel
		if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey ("q") || Input.GetKey ("a") || (Mathf.Abs(Input.acceleration.x) < -0.1f)){
			PlayerManager.LEFT();
		}


		if (Input.GetKeyDown ("q") || Input.GetKeyDown ("a") || Input.GetKeyDown ("left")) {
			PlayerManager.LeftFight ();
		}

		//jmorel
		if(Input.GetKey(KeyCode.DownArrow)  || Input.GetKey ("s") || (Mathf.Abs(Input.acceleration.y) < -0.1f)){

			PlayerManager.DOWN ();
		}


		if (Input.GetKeyDown ("s") || Input.GetKeyDown ("down")) {
			PlayerManager.DownFight ();
		}

		if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey ("d") || (Mathf.Abs(Input.acceleration.x) > 0.1f)){
			PlayerManager.RIGHT();
		}
		if (Input.GetKeyDown ("d") || Input.GetKeyDown ("right")) {
			PlayerManager.RightFight ();
		}
	}

	void UpdatePauseState(){
		if(Input.GetKeyDown("p")){
			Debug.Log("DÉPAUSE ! ");
			GameStateManager.m_instance.setGameState(GameState.Playing);
		}
	}

	void UpdateGameOverState(){
	}

}
