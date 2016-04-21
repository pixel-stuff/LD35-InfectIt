using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class UIMenuManager : MonoBehaviour {

    // Time until a switch to a scene is effective (in ms)
    private int m_switchSceneDelay = 1500;

    private Boolean m_isSwitchingToLevelScene = false;
    private float m_switchTimeToLevelScene = 0;

    private Boolean m_isSwitchingToTutorialScene = false;
    private float m_switchTimeToTutorialScene = 0;

    public GameObject virus;
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

        // Processing a potential switch to tutorial scene (delaying the real switch to at least m_switchSceneDelay ms)
        if(m_isSwitchingToTutorialScene)
        {
            if(m_switchTimeToTutorialScene <= m_switchSceneDelay)
            {
                m_switchTimeToTutorialScene += Time.deltaTime * 1000.0f;
            } else
            {
                m_isSwitchingToTutorialScene = false;
                this.GoToTutoScene();
            }
        }

        // Processing a potential switch to level scene (delaying the real switch to at least m_switchSceneDelay ms)
        if (m_isSwitchingToLevelScene)
        {
            if (m_switchTimeToLevelScene <= m_switchSceneDelay)
            {
                m_switchTimeToLevelScene += Time.deltaTime * 1000.0f;
            }
            else
            {
                m_isSwitchingToLevelScene = false;
                this.GoToLevelScene();
            }
        }
    }

	AsyncOperation a;
	float timeStartLoading;
    
    public void PrepareToGoToLevelScene()
    {
        AudioManager.m_instance.PlayMenuButtonSound3();
        AudioManager.m_instance.StartMenuMusicFadeOut();

        m_switchTimeToLevelScene = 0.0f;
        m_isSwitchingToLevelScene = true;
    }

	public void GoToLevelScene(){
		loadAnimation ();
		GameStateManager.m_instance.setGameState (GameState.Playing);
		SceneManager.LoadSceneAsync("LevelScene",LoadSceneMode.Single);//.LoadLevelAsync ("LevelScene");
		AudioManager.m_instance.StopMenuBeat ();
		//a.allowSceneActivation = false;
		timeStartLoading = Time.time;

	}

    public void PrepareToGoToTutoScene()
    {
        AudioManager.m_instance.PlayMenuButtonSound0();
        AudioManager.m_instance.StartMenuMusicFadeOut();

        m_switchTimeToTutorialScene = 0.0f;
        m_isSwitchingToTutorialScene = true;
    }

	public void GoToTutoScene(){
        a =  Application.LoadLevelAsync ("TutorialScene");
	}

	public void loadAnimation() {
		virus.SetActive (true);
	}
}
