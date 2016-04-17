using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {

	#region Singleton
	public static TimeManager m_instance;
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

	private float m_currentTime = 0f;


	[SerializeField]
	private GameObject m_particules;
	private Vector2 m_particulesStartPos = new Vector2(1800f,0f);
	private Vector2 m_particulesPos;

	// Use this for initialization
	void Start () {
		AddSecond (10f);
		m_particules.GetComponent<RectTransform> ().anchoredPosition = m_particulesPos;
		b = m_particules.GetComponent<ParticleSystem> ().emission;
		b.rate = new ParticleSystem.MinMaxCurve(20f);
	}

	ParticleSystem.EmissionModule b;
	bool anim = false;

	// Update is called once per frame
	void Update () {
		if (m_currentTime > 0) {
			m_currentTime -= Time.deltaTime;
			this.GetComponent<Image> ().fillAmount = m_currentTime * 100 / 10 * 1 / 100;
			m_particulesPos = m_particulesStartPos * m_currentTime * 100 / 10 * 1 / 100;
			m_particules.GetComponent<RectTransform> ().anchoredPosition = m_particulesPos;

			if( ( m_currentTime * 100 / 10 * 1 / 100) <= 0.80f && !anim){
				SubSecond(3f);
				anim = true;
			}
		}else{
		}
	}

	/// <summary>
	/// Set Time In Percent
	/// </summary>
	/// <param name="percent">Percent.</param>
	public void SetTime(float percent){
		this.GetComponent<Image> ().fillAmount = percent;
	}


	public void AddSecond(float second){
		m_currentTime += second;
		this.GetComponent<Image> ().fillAmount = m_currentTime*100/10* 1/100;
	}

	public void SubSecond(float second){
		m_currentTime -= second;
		this.GetComponent<Image> ().fillAmount = m_currentTime*100/10* 1/100;
		StartCoroutine (SubTimeAnimation ());
	}

	public IEnumerator SubTimeAnimation(){
		b.rate = new ParticleSystem.MinMaxCurve(100f);
		yield return new WaitForSeconds(0.5f);

		b.rate = new ParticleSystem.MinMaxCurve(200f);
		yield return new WaitForSeconds(0.5f);

		b.rate = new ParticleSystem.MinMaxCurve(100f);
		yield return new WaitForSeconds(0.5f);

		b.rate = new ParticleSystem.MinMaxCurve(20f);
	}
}
