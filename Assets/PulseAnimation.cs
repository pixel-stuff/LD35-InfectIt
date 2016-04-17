﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animation))]
public class PulseAnimation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		FindObjectOfType<AudioManager> ().m_beatEvent += BeatHandler;
	}

	public void BeatHandler(){
		this.GetComponent<Animation> ().Play ("ScaleBeatAnimation");
	}
}