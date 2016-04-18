using UnityEngine;
using System.Collections;
using System;

public class AudioManager : MonoBehaviour {

	#region Singleton
	public static AudioManager m_instance;
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

	[SerializeField]
	private string m_menuMusic = "sneaky_theme_100bpm";
	private GameObject m_menuMusicGB;

	private static Transform m_transform;

	[Space(25)]
	[SerializeField]
	private string m_fightMusic = "infect_it_theme_125bpm";
	public Action m_beatFightEvent;
	private GameObject m_fightMusicGB;
	private float m_timeBetweenFightBeat = 60f / 100f;
	public float timeBetweenFightBeat{
		get { return m_timeBetweenFightBeat; }
	}

	[Space(25)]
	[SerializeField]
	private string m_rechercheMusic = "chase_to_leave_theme_125bpm";
	public Action m_beatRechercheEvent;
	private GameObject m_rechercheMusicGB;
	private float m_timeBetweenRechercheBeat = 60f / 125f;
	public float timeBetweenRechercheBeat{
		get { return m_timeBetweenRechercheBeat; }
	}

	// Use this for initialization
	void Start () {
		m_transform = this.transform;
		PlayMenuMusic ();
	}

	public void PlayMusic(string name){
		//Create an empty game object
		GameObject go = new GameObject ("Audio_" +  name);
		go.transform.parent = m_transform;
		//Load clip from ressources folder
		AudioClip newClip =  Instantiate(Resources.Load (name, typeof(AudioClip))) as AudioClip;

		//Add and bind an audio source
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = newClip;
		//Play and destroy the component
		source.Play();
		Destroy (go, newClip.length);
	}

	#region Menu
	public void PlayMenuMusic(){
		//Create an empty game object
		m_menuMusicGB = new GameObject ("Audio_" +  m_menuMusic);
		m_menuMusicGB.transform.parent = m_transform;
		//Load clip from ressources folder
		AudioClip newClip =  Instantiate(Resources.Load (m_menuMusic, typeof(AudioClip))) as AudioClip;

		//Add and bind an audio source
		AudioSource source = m_menuMusicGB.AddComponent<AudioSource>();
		source.clip = newClip;
		//Play and destroy the component
		source.Play();
		Destroy (m_menuMusicGB, newClip.length);
	}

	public void StopMenuBeat(){
		Destroy (m_menuMusicGB);
	}
	#endregion Menu

	#region Recherche
	public void PlayRechercheMusic(){
		if (m_rechercheMusicGB == null) {
			m_rechercheMusicGB = new GameObject ("Audio_" + m_rechercheMusic);
			m_rechercheMusicGB.transform.parent = m_transform;
			//Load clip from ressources folder
			AudioClip newClip = Instantiate (Resources.Load (m_rechercheMusic, typeof(AudioClip))) as AudioClip;

			//Add and bind an audio source
			AudioSource source = m_rechercheMusicGB.AddComponent<AudioSource> ();
			source.clip = newClip;
			//Play and destroy the component
			source.Play ();
			InvokeRepeating ("BeatRechercheEvent", m_timeBetweenRechercheBeat * 1f, m_timeBetweenRechercheBeat);
		} else {
			m_rechercheMusicGB.GetComponent<AudioSource> ().time = 0f;
			m_rechercheMusicGB.GetComponent<AudioSource> ().Play ();
			InvokeRepeating ("BeatRechercheEvent", m_timeBetweenRechercheBeat * 1f, m_timeBetweenRechercheBeat);
		}
	}

	public void BeatRechercheEvent(){
		if (m_beatRechercheEvent != null) {
			m_beatRechercheEvent ();
		}
	}

	public void StopRechercheBeat(){
		CancelInvoke ("BeatRechercheEvent");
		m_rechercheMusicGB.GetComponent<AudioSource> ().Pause ();
	}
	#endregion Recherche

	#region Fight
	public void PlayFightMusic(){
		if (m_fightMusicGB == null) {
			m_fightMusicGB = new GameObject ("Audio_" + m_fightMusic);
			m_fightMusicGB.transform.parent = m_transform;
			//Load clip from ressources folder
			AudioClip newClip = Instantiate (Resources.Load (m_fightMusic, typeof(AudioClip))) as AudioClip;

			//Add and bind an audio source
			AudioSource source = m_fightMusicGB.AddComponent<AudioSource> ();
			source.clip = newClip;
			//Play and destroy the component
			source.Play ();
			InvokeRepeating ("BeatFightEvent", m_timeBetweenFightBeat * 1f, m_timeBetweenFightBeat);
		} else {
			m_fightMusicGB.GetComponent<AudioSource> ().time = 0f;
			m_fightMusicGB.GetComponent<AudioSource> ().Play ();
			InvokeRepeating ("BeatFightEvent", m_timeBetweenFightBeat * 1f, m_timeBetweenFightBeat);
		}
	}

	public void BeatFightEvent(){
		if (m_beatFightEvent != null) {
			m_beatFightEvent ();
		}
	}

	public void StopFightBeat(){
		CancelInvoke ("BeatFightEvent");
		m_fightMusicGB.GetComponent<AudioSource> ().Pause ();
		//if (m_fightMusicGB != null) {
			//Destroy (m_fightMusicGB);
		//}
	}
	#endregion Fight


	// Update is called once per frame
	void Update () {
	}
}
