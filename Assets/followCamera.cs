using UnityEngine;
using System.Collections;

public class followCamera : MonoBehaviour {

	private GameObject m_camera;
	public float percentFollow = 100;
	private Vector3 m_oldCamera;
	// Use this for initialization
	void Start () {
		m_camera = GameObject.FindGameObjectWithTag ("MainCamera");
		m_oldCamera = m_camera.transform.position;
	}

	// Update is called once per frame
	void FixedUpdate () {
		Vector3 speed = (m_camera.transform.position - m_oldCamera );
		speed.z = 0;
		m_oldCamera = m_camera.transform.position;
		this.gameObject.transform.position = this.transform.position + speed*(percentFollow/100);
	}
}
