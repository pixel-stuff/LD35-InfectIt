using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {

	private float m_currentTime = 0f;


	[SerializeField]
	private GameObject m_particules;
	private Vector2 m_particulesStartPos = new Vector2(1800f,0f);
	private Vector2 m_particulesPos;

	// Use this for initialization
	void Start () {
		AddSecond (10f);
		m_particules.GetComponent<RectTransform> ().anchoredPosition = m_particulesPos;

	}
	bool b = false;
	
	// Update is called once per frame
	void Update () {
		if (m_currentTime > 0) {
			m_currentTime -= Time.deltaTime;
			this.GetComponent<Image> ().fillAmount = m_currentTime * 100 / 10 * 1 / 100;
			m_particulesPos = m_particulesStartPos * m_currentTime * 100 / 10 * 1 / 100;
			m_particules.GetComponent<RectTransform> ().anchoredPosition = m_particulesPos;
			if( (m_currentTime * 100 / 10 * 1 / 100) <= 0.6f && (m_currentTime * 100 / 10 * 1 / 100) >= 0.45f ){
				//b = false;
				//m_particules.GetComponent<ParticleSystem> ().emission. = b;
			}
			if((m_currentTime * 100 / 10 * 1 / 100) <= 0.4f){
				//b = true;
				//m_particules.GetComponent<ParticleSystem> ().emission.enabled = b;
			}
		}else{
			//m_particules.GetComponent<ParticleSystem> ().Play();
		}
	}

	/// <summary>
	/// Set Time In Percent
	/// </summary>
	/// <param name="percent">Percent.</param>
	public void SetTime(float percent){
		this.GetComponent<Image> ().fillAmount = percent;
	}

	/// <summary>
	/// Set Time In Second
	/// </summary>
	/// <param name="percent">Percent.</param>
	public void AddSecond(float second){
		m_currentTime += second;
		this.GetComponent<Image> ().fillAmount = m_currentTime*100/10* 1/100;
	}
}
