using UnityEngine;
using System.Collections;

public class virusHack : MonoBehaviour {

	public Cell cellScript;

	public void startFusion(Vector3 virusPos) {
		cellScript.startFusion (virusPos);
	}

	public void stopFusion() {
		cellScript.stopFusion ();
	}

	public bool acceptFusion() {
		return !cellScript.m_isAfraid;
	}

	public void consume() {
		cellScript.consume ();
		this.gameObject.SetActive (false);
	}
}
