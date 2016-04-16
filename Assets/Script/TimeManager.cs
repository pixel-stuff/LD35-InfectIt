using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {

	private float m_currentTime = 0f;

	// Use this for initialization
	void Start () {
		AddSecond (10f);
	}
	
	// Update is called once per frame
	void Update () {
		if (m_currentTime > 0) {
			m_currentTime -= Time.deltaTime;
			this.GetComponent<Image> ().fillAmount = m_currentTime * 100 / 10 * 1 / 100;
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
