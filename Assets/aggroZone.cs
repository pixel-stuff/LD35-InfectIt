using UnityEngine;
using System.Collections;

public class aggroZone : MonoBehaviour {

	public whiteCell m_whiteCell;

	public float m_maxRadiusAgroZone;
	public float m_degradationPerFrame;
	private float m_initialAggroRadius;

	private float m_radius;
	// Use this for initialization
	void Start () {
		m_initialAggroRadius = this.GetComponent<CircleCollider2D> ().radius;
		m_radius = m_initialAggroRadius;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		this.GetComponent<CircleCollider2D> ().radius = m_radius;

		if (m_radius > m_initialAggroRadius) {
			m_radius -= m_degradationPerFrame;
		}

	}

	public void setMaxRadius() {
		m_radius = m_maxRadiusAgroZone;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.layer == LayerMask.NameToLayer ("Virus")) {
			m_whiteCell.m_target = other.gameObject;
		}
	}

	void OnTriggerExit2D(Collider2D other) {

		if (other.gameObject.layer == LayerMask.NameToLayer ("Virus")) {
			m_whiteCell.m_target =null;
		}
	}
}
