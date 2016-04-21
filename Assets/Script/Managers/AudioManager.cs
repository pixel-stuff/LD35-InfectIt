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
    AudioSource m_menuMusicSource;
    private Boolean m_menuMusicIsFadingIn = false;
    private Boolean m_menuMusicIsFadingOut = false;
    private float m_fadeRate = 0.7f;
	private GameObject m_menuMusicGB;

    // 4 available sounds for UI buttons, let's pick some at the onClick() event
    [SerializeField]
    private string m_menuButtonSound0 = "combination_success_0";
    private GameObject m_menuButtonSound0GO;
    private AudioClip m_menuButtonClip0;
    private AudioSource m_menuButtonSound0Source;

    [SerializeField]
    private string m_menuButtonSound1 = "combination_success_1";
    private GameObject m_menuButtonSound1GO;
    private AudioClip m_menuButtonClip1;
    private AudioSource m_menuButtonSound1Source;

    [SerializeField]
    private string m_menuButtonSound2 = "combination_success_2";
    private GameObject m_menuButtonSound2GO;
    private AudioClip m_menuButtonClip2;
    private AudioSource m_menuButtonSound2Source;

    [SerializeField]
    private string m_menuButtonSound3 = "combination_success_3";
    private GameObject m_menuButtonSound3GO;
    private AudioClip m_menuButtonClip3;
    private AudioSource m_menuButtonSound3Source;

    private static Transform m_transform;

	[Space(25)]
	[SerializeField]
	private string m_fightMusic = "infect_it_theme_125bpm";
	public Action m_beatFightEvent;
	public Action m_beforeBeatFightEvent;
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
	}

	public void PlayMusic(string name){
		if (m_menuMusicGB != null) {
			Destroy (m_menuMusicGB);
		}
		if (m_fightMusicGB != null) {
			Destroy (m_fightMusicGB);
			CancelInvoke ("BeatFightEvent");
		}

		if (m_rechercheMusicGB != null) {
			Destroy (m_rechercheMusicGB);
			CancelInvoke ("BeatRechercheEvent");
		}

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


		if (m_menuMusic.Equals(""))
			return;

		if (m_menuMusicGB != null)
			return;
		
		//Create an empty game object
		m_menuMusicGB = new GameObject ("Audio_" +  m_menuMusic);
		m_menuMusicGB.transform.parent = m_transform;
        //Load clip from ressources folder
        AudioClip newClip =  Instantiate(Resources.Load (m_menuMusic, typeof(AudioClip))) as AudioClip;

		//Add and bind an audio source
		m_menuMusicSource = m_menuMusicGB.AddComponent<AudioSource>();
        m_menuMusicSource.clip = newClip;
        m_menuMusicSource.loop = true;
        // Keep the reference for fade in/fade out

        //Play and destroy the component
        m_menuMusicSource.Play();
		Destroy (m_menuMusicGB, newClip.length);
	}

    public void StartMenuMusicFadeIn()
    {
        m_menuMusicIsFadingIn = true;
    }

    public void StartMenuMusicFadeOut()
    {
        m_menuMusicIsFadingOut = true;
    }

	public void StopMenuBeat(){
		Destroy (m_menuMusicGB);
	}
    
    public void PlayMenuButtonSound0()
    {
        Debug.Log("Playing menu sound 0");
        m_menuButtonSound0GO = new GameObject("Audio_" + m_menuButtonSound0);
        m_menuButtonClip0 = Instantiate(Resources.Load(m_menuButtonSound0, typeof(AudioClip))) as AudioClip;
        m_menuButtonSound0Source = m_menuButtonSound0GO.AddComponent<AudioSource>();
        m_menuButtonSound0Source.PlayOneShot(m_menuButtonClip0, 1.0f);
        Destroy(m_menuButtonSound0GO, m_menuButtonClip0.length);
    }

    public void PlayMenuButtonSound1()
    {
        Debug.Log("Playing menu sound 1");
        m_menuButtonSound1GO = new GameObject("Audio_" + m_menuButtonSound1);
        m_menuButtonClip1 = Instantiate(Resources.Load(m_menuButtonSound1, typeof(AudioClip))) as AudioClip;
        m_menuButtonSound1Source = m_menuButtonSound1GO.AddComponent<AudioSource>();
        m_menuButtonSound1Source.PlayOneShot(m_menuButtonClip1, 1.0f);
        Destroy(m_menuButtonSound1GO, m_menuButtonClip1.length);
    }

    public void PlayMenuButtonSound2()
    {
        Debug.Log("Playing menu sound 2");
        m_menuButtonSound2GO = new GameObject("Audio_" + m_menuButtonSound2);
        m_menuButtonClip2 = Instantiate(Resources.Load(m_menuButtonSound2, typeof(AudioClip))) as AudioClip;
        m_menuButtonSound2Source = m_menuButtonSound2GO.AddComponent<AudioSource>();
        m_menuButtonSound2Source.PlayOneShot(m_menuButtonClip2, 1.0f);
        Destroy(m_menuButtonSound2GO, m_menuButtonClip2.length);
    }

    public void PlayMenuButtonSound3()
    {
        Debug.Log("Playing menu sound 3");
        m_menuButtonSound3GO = new GameObject("Audio_" + m_menuButtonSound3);
        m_menuButtonClip3 = Instantiate(Resources.Load(m_menuButtonSound3, typeof(AudioClip))) as AudioClip;
        m_menuButtonSound3Source = m_menuButtonSound3GO.AddComponent<AudioSource>();
        m_menuButtonSound3Source.PlayOneShot(m_menuButtonClip3, 1.0f);
        Destroy(m_menuButtonSound3GO, m_menuButtonClip3.length);
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
			source.loop = true;
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
		m_rechercheMusicGB.GetComponent<AudioSource> ().volume = 0.0f;
	}

	public void ReprendreRechercheBeat(){
		m_rechercheMusicGB.GetComponent<AudioSource> ().volume = 1.0f;
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
			source.loop = true;
			//Play and destroy the component
			source.Play ();
		} else {
			m_fightMusicGB.GetComponent<AudioSource> ().time = 0f;
			m_fightMusicGB.GetComponent<AudioSource> ().Play ();
		}
		InvokeRepeating ("BeatFightEvent", m_timeBetweenFightBeat * 1f, m_timeBetweenFightBeat);
		//InvokeRepeating ("BeforeBeatFightEvent", m_timeBetweenFightBeat * 1f - 0.10f, m_timeBetweenFightBeat - 0.10f);
	}

	public void BeatFightEvent(){
		if (m_beatFightEvent != null) {
			m_beatFightEvent ();
		}
	}

	public void BeforeBeatFightEvent(){
		if (m_beforeBeatFightEvent != null) {
			m_beforeBeatFightEvent ();
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

	public void DestroyAllMusic(){
		if (m_menuMusicGB != null) {
			Destroy (m_menuMusicGB);
		}
		if (m_fightMusicGB != null) {
			Destroy (m_fightMusicGB);
			CancelInvoke ("BeatFightEvent");
		}

		if (m_rechercheMusicGB != null) {
			Destroy (m_rechercheMusicGB);
			CancelInvoke ("BeatRechercheEvent");
		}
	}

	// Update is called once per frame
	void Update () {
        // If fade in is activated in menu music, we fade in until its volume reaches 1
        if(m_menuMusicIsFadingIn)
        {
            if(m_menuMusicSource.volume < 1.0f)
            {
                float volume = m_menuMusicSource.volume;
                m_menuMusicSource.volume = (volume + m_fadeRate * Time.deltaTime);
            } else
            {
                m_menuMusicIsFadingIn = false;
            }
        }

        // If fade out is activated in menu music, we fade out until its volume reaches 0
        if (m_menuMusicIsFadingOut)
        {
            if (m_menuMusicSource.volume > 0.0f)
            {
                float volume = m_menuMusicSource.volume;
                m_menuMusicSource.volume = (volume - m_fadeRate * Time.deltaTime);
            }
            else
            {
                m_menuMusicIsFadingOut = false;
            }
        }
    }
}
