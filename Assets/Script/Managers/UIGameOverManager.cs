using UnityEngine;
using System.Collections;

public class UIGameOverManager : MonoBehaviour {
	public GameObject virus;
	// Use this for initialization
	void Start () {
		AudioManager.m_instance.PlayMenuMusic ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ReturnToSceneMenu(){
		GameStateManager.m_instance.setGameState (GameState.Menu);
		Application.LoadLevelAsync ("MenuScene");
	}
	
	public void ReturnToLevelScene(){
		loadAnimation ();
		GameStateManager.m_instance.setGameState (GameState.Playing);
		Application.LoadLevelAsync ("LevelScene");
		
	}

	public void loadAnimation() {
		virus.SetActive (true);
	}
}
