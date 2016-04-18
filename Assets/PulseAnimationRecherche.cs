using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animation))]
public class PulseAnimationRecherche : MonoBehaviour {

		// Use this for initialization
		void Start () {
		FindObjectOfType<AudioManager> ().m_beatRechercheEvent += BeatHandler;
		}

		public void BeatHandler(){
			this.GetComponent<Animation> ().Play ("ScaleBeatAnimation");
		}
}
