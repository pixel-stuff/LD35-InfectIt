using UnityEngine;
using System.Collections;

public class virusHack : MonoBehaviour {

	public Cell cellScript;

	public void startFusion() {
		cellScript.startFusion ();
	}

	public void stopFusion() {
		cellScript.stopFusion ();
	}

	public bool acceptFusion() {
		return !cellScript.m_isAfraid;
	}
}
