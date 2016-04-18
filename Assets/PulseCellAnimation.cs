using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animation))]
public class PulseCellAnimation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		FindObjectOfType<AudioManager> ().m_beatFightEvent += BeatHandler;
	}

	public void BeatHandler(){
		this.GetComponent<Animation> ().Play ("ScaleBeatCellAnimation");
	}

	void OnDestroy() {
		AudioManager.m_instance.m_beatFightEvent -= BeatHandler;
	}
}
