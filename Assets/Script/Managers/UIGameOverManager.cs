using UnityEngine;
using System.Collections;

public class UIGameOverManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ReturnToSceneMenu(){
		GameStateManager.m_instance.setGameState (GameState.Menu);
		Application.LoadLevelAsync ("MenuScene");
	}
	
	public void ReturnToLevelScene(){
		GameStateManager.m_instance.setGameState (GameState.Playing);
		Application.LoadLevelAsync ("LevelScene");
		
	}
}
