using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

[Serializable]
public struct StepFight{
	public int m_id;	//0 -> triangle/up
						//1 -> carre/left
						//2 -> croix/down
						//3 -> rond/right
	public string m_nameController;
	public Sprite m_sprite;
	public Color m_spriteCoolor;
	public AudioClip m_soundByCell;
	public AudioClip m_soundByVirus;
}


public class FightManager : MonoBehaviour {
	#region Singleton
	private static FightManager m_instance;
	void Awake(){
		if(m_instance == null){
			//If I am the first instance, make me the Singleton
			m_instance = this;
		}else{
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
			if(this != m_instance)
				Destroy(this.gameObject);
		}
	}
	#endregion Singleton
	[Space(10)]
	public StepFight[] m_listOfInput; 	

	[Space(10)]
	public VirusFightManager m_virus;
	[Space(10)]
	public GameObject m_bubbleVirus;
	public GameObject[] m_listOfInputVirus;
	[Space(10)]
	public GameObject m_bubbleCell;
	public GameObject m_InputCell;
	[Space(20)]
	public AudioClip m_sonFailed;
	public Sprite m_spriteInterrogation;
	public Color m_colorInterrogation;

	[Space(20)]
	[SerializeField]
	private GameObject m_CroixExplication;

	private bool m_isInit = false;
	private int m_numberOfStep = 4;
	private List<int> m_listOfIDInputWaited = new List<int> ();
	private float m_timeBetweenBeat = 0.60f;
	private float m_percentErrorAceptable = 25.0f;
	private bool m_waitingForInput = false;

	private int m_currentStepInputWaited = 0;



	public void InitFight(Cell cell){
		m_bubbleVirus.SetActive(false);
		m_bubbleCell.SetActive (false);
		m_InputCell.SetActive (false);
		for (int i = 0; i < m_listOfInputVirus.Length; i++) {
			m_listOfInputVirus [i].SetActive (false);
		}

		TimeManager.m_instance.ChangeState (TimeState.fight);

		m_listOfIDInputWaited.Clear();
		m_currentStepInputWaited = 0;

		for (int i = 0; i < m_listOfInputVirus.Length; i++) {
			if (i < m_numberOfStep) {
				int temp = UnityEngine.Random.Range (0, 4);
				m_listOfIDInputWaited.Add (temp);
			} else {
				m_listOfInputVirus [i].SetActive (false);
			}
		}
		/*for (int i = 0; i < m_listOfIDInputWaited.Count; i++) {
			Debug.Log ("init : " + m_listOfInput[m_listOfIDInputWaited[i]].m_nameController);
		}*/

		AudioManager.m_instance.PlayFightMusic ();
		m_lastBeatInvoke = Time.time;
		Invoke("StartCellStatement", 1.5f);
		m_isInit = true;
	}



	private int m_numberOfBeatDoneInvoke = 0;
	private float m_lastBeatInvoke = 0;
	private bool m_cellStatementActive = false;
	private int m_cellStatement = 0;
	private int m_numberOfBeatWithNoInput = 0;
	private int m_numberOfBeatWithoutInputLimit = 20;
	private int m_numberOfStatementDone = 0;
	public void BeatInvokeHandle(){
		m_numberOfBeatDoneInvoke++;
		m_lastBeatInvoke = Time.time;

		for (int i = 0; i < m_listOfInputVirus.Length; i++) {
			if (!m_listOfInputVirus [i].GetComponent<Animation> ().isPlaying) {
					m_listOfInputVirus [i].GetComponent<Animation> ().Play ("ScaleInputBeatFight");
				}
		}

		if (m_cellStatementActive) {
			NextStepCellStatement ();
		} else {
		}
		m_InputCell.GetComponent<Animation> ().Play ("ScaleInputBeatFight");
		//m_bubbleCell.GetComponent<Animation> ().Play ("ScaleBubbleAnimation");
		//m_bubbleVirus.GetComponent<Animation> ().Play ("ScaleBubbleAnimation");
		if (m_waitingForInput) {
			m_numberOfBeatWithNoInput++;
		}
		//m_virus.BeatAnim ();
	}

	public void BeforeBeatInvokeHandle(){
		//m_virus.SquareAnim ();
	}


	// Use this for initialization
	void Start () {
		m_timeBetweenBeat = AudioManager.m_instance.timeBetweenFightBeat;
		m_bubbleVirus.SetActive(false);
		m_bubbleCell.SetActive (false);
		m_InputCell.SetActive (false);
		for (int i = 0; i < m_listOfInputVirus.Length; i++) {
			m_listOfInputVirus [i].SetActive (false);
		}
		AudioManager.m_instance.m_beatFightEvent += BeatInvokeHandle;
		AudioManager.m_instance.m_beforeBeatFightEvent += BeforeBeatInvokeHandle;

		AudioManager.m_instance.StopMenuBeat ();
		/*InitFight (null);
		GameStateManager.m_instance.setGameState (GameState.Playing);*/
	}

	// Update is called once per frame
	void Update () {
		if (m_waitingForInput && m_numberOfBeatWithNoInput >= m_numberOfBeatWithoutInputLimit) {
			m_waitingForInput = false;
			if (m_numberOfStatementDone >= 3) {
				EndFight (false);
			} else {
				m_bubbleVirus.SetActive (false);
				Invoke ("StartCellStatement", 1.5f);
			}
		}
	}


	private void EndFight(bool isWin){
		m_bubbleVirus.SetActive (false);
		m_bubbleCell.SetActive (false);
		if (isWin) {
			Debug.Log ("YOU WIN");
		} else {
			Debug.Log ("YOU DIIEIEEEIEIEE");
		}
		/*Debug.Log ("RESTART");
		InitFight (null);
		AudioManager.m_instance.PlayFightMusic ();	
*/

		AudioManager.m_instance.StopFightBeat ();
		try{
			PlayerManager.m_instance.FightOver (isWin);
		}catch(Exception e){}
	}

	#region CellStatement
	private void StartCellStatement(){
		m_cellStatementActive = true;
		m_cellStatement = 0;
		m_numberOfStatementDone++;
	}

	private void NextStepCellStatement(){
		if (m_cellStatement < m_listOfIDInputWaited.Count) {
			//m_listOfIDInputWaited
			m_InputCell.GetComponent<Image> ().sprite = m_listOfInput [m_listOfIDInputWaited [m_cellStatement]].m_sprite;
			m_InputCell.GetComponent<Image> ().color = m_listOfInput [m_listOfIDInputWaited [m_cellStatement]].m_spriteCoolor;
			this.GetComponent<AudioSource> ().clip = m_listOfInput [m_listOfIDInputWaited [m_cellStatement]].m_soundByCell;
			this.GetComponent<AudioSource> ().Play ();
			m_bubbleCell.SetActive (true);
			m_InputCell.SetActive (true);
			m_cellStatement++;
		} else {
			m_cellStatementActive = false;
			m_InputCell.GetComponent<Image> ().sprite = m_spriteInterrogation;
			m_InputCell.GetComponent<Image> ().color = m_colorInterrogation;
			StartWaitingPlayerInput ();
		}
	}

	private void StartWaitingPlayerInput(){
		m_waitingForInput = true;
		m_numberOfBeatWithNoInput = 0;
		m_bubbleVirus.SetActive (true);
	}

	#endregion CellStatement


	#region Input
	public void UpInput(){
		m_numberOfBeatWithNoInput = 0;
		if (!m_waitingForInput)
			return;
		
		if (!IsTimingOK ()) {
			ErrorInput ();
			return;
		}
		if (m_listOfIDInputWaited [m_currentStepInputWaited] != 0) {
			ErrorInput ();
			return;
		}
		m_virus.TriangleAnim ();
		m_virus.StartTriangleParticles ();
		SuccessInput ();
	}

	public void DownInput(){
		m_numberOfBeatWithNoInput = 0;
		if (!m_waitingForInput)
			return;
		
		if (!IsTimingOK ()) {
			ErrorInput ();
			return;
		}
		if (m_listOfIDInputWaited [m_currentStepInputWaited] != 2) {
			ErrorInput ();
			return;
		}
		m_virus.CrossAnim ();
		m_virus.StartCrossParticles ();
		SuccessInput ();
	}

	public void RightInput(){
		m_numberOfBeatWithNoInput = 0;
		if (!m_waitingForInput)
			return;
		
		if (!IsTimingOK ()) {
			ErrorInput ();
			return;
		}
		if (m_listOfIDInputWaited [m_currentStepInputWaited] != 3) {
			ErrorInput ();
			return;
		}
		m_virus.DonutsAnim ();
		m_virus.StartDonutsParticles ();
		SuccessInput ();
	}

	public void LeftInput(){
		m_numberOfBeatWithNoInput = 0;
		if (!m_waitingForInput)
			return;
		
		if (!IsTimingOK ()) {
			ErrorInput ();
			return;
		}
		if (m_listOfIDInputWaited [m_currentStepInputWaited] != 1) {
			ErrorInput ();
			return;
		}
		m_virus.SquareAnim ();
		m_virus.StartSquareParticles ();
		SuccessInput ();
	}

	public bool IsTimingOK(){
		bool isOK = false;
		//Debug.Log ("diff1 : " + Mathf.Abs (Time.time - m_lastBeatInvoke) + " -> " + (m_timeBetweenBeat * m_percentErrorAceptable / 100f));
		if (Mathf.Abs (Time.time - m_lastBeatInvoke) <= m_timeBetweenBeat * m_percentErrorAceptable / 100f) {
			isOK = true;
		}

		//Debug.Log ("diff2 : " + Mathf.Abs (Time.time - m_timeBetweenBeat - m_lastBeatInvoke) + " -> " + (m_timeBetweenBeat * m_percentErrorAceptable / 100f));
		if (Mathf.Abs (Time.time - m_timeBetweenBeat - m_lastBeatInvoke) <= m_timeBetweenBeat * m_percentErrorAceptable / 100f) {
			isOK = true;
		}
		return isOK;
	}

	public void ErrorInput(){
		this.GetComponent<AudioSource> ().clip = m_sonFailed;
		this.GetComponent<AudioSource> ().Play ();
		m_bubbleVirus.SetActive (false);
		m_currentStepInputWaited = 0;
		for (int i = 0; i < m_listOfInputVirus.Length; i++) {
			m_listOfInputVirus [i].SetActive (false);
		}

		m_waitingForInput = false;
		if (m_numberOfStatementDone >= 3) {
			EndFight (false);
		} else {
			Invoke ("StartCellStatement", 1.5f);
		}
	}

	public void SuccessInput(){
		m_CroixExplication.GetComponent<Image> ().color = Color.white;
		Invoke ("resetInputCroix", 0.3f);
		if (m_currentStepInputWaited < m_listOfIDInputWaited.Count-1) {
			m_listOfInputVirus [m_currentStepInputWaited].GetComponent<Image> ().sprite = m_listOfInput [m_listOfIDInputWaited [m_currentStepInputWaited]].m_sprite;
			m_listOfInputVirus [m_currentStepInputWaited].GetComponent<Image> ().color = m_listOfInput [m_listOfIDInputWaited [m_currentStepInputWaited]].m_spriteCoolor;
			m_listOfInputVirus [m_currentStepInputWaited].SetActive (true);
			this.GetComponent<AudioSource> ().clip = m_listOfInput [m_listOfIDInputWaited [m_currentStepInputWaited]].m_soundByVirus;
			this.GetComponent<AudioSource> ().Play ();
			m_listOfInputVirus [m_currentStepInputWaited].GetComponent<Animation> ().Play ("ScaleInputEnterFight");
			m_currentStepInputWaited++;
		} else {
			m_listOfInputVirus [m_currentStepInputWaited].GetComponent<Image> ().sprite = m_listOfInput [m_listOfIDInputWaited [m_currentStepInputWaited]].m_sprite;
			m_listOfInputVirus [m_currentStepInputWaited].GetComponent<Image> ().color = m_listOfInput [m_listOfIDInputWaited [m_currentStepInputWaited]].m_spriteCoolor;
			m_listOfInputVirus [m_currentStepInputWaited].SetActive (true);
			this.GetComponent<AudioSource> ().clip = m_listOfInput [m_listOfIDInputWaited [m_currentStepInputWaited]].m_soundByVirus;
			this.GetComponent<AudioSource> ().Play ();
			m_listOfInputVirus [m_currentStepInputWaited].GetComponent<Animation> ().Play ("ScaleInputEnterFight");
			Invoke ("InvokeWinAfterDelay",1.0f);
		}
	}

	public void resetInputCroix(){
		m_CroixExplication.GetComponent<Image> ().color = new Color (1f, 1f, 1f, 0.5f);
	}

	public void InvokeWinAfterDelay(){
		EndFight (true);
	}
	#endregion Input


}
