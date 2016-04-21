using UnityEngine;
using System;

public class UITutoManager : MonoBehaviour {

    // Time until a switch to a scene is effective (in ms)
    private int m_switchSceneDelay = 1500;

    private Boolean m_isSwitchingToMenuScene = false;
    private float m_switchTimeToMenuScene = 0;

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

        // Processing a potential switch to menu scene (delaying the real switch to at least m_switchSceneDelay ms)
        if (m_isSwitchingToMenuScene)
        {
            if (m_switchTimeToMenuScene <= m_switchSceneDelay)
            {
                m_switchTimeToMenuScene += Time.deltaTime * 1000.0f;
            }
            else
            {
                m_isSwitchingToMenuScene = false;
                this.GoToMenuScene();
            }
        }
    }

	AsyncOperation a;


    public void prepareToGoToMenuScene()
    {
        AudioManager.m_instance.PlayMenuButtonSound0();

        m_switchTimeToMenuScene = 0.0f;
        m_isSwitchingToMenuScene = true;
    }

	public void GoToMenuScene(){
		Debug.Log ("LA");
		Application.LoadLevelAsync ("MenuScene");
		Debug.Log ("LA");
	}
}
