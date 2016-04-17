using UnityEngine;
using System.Collections;

public class virusLight : MonoBehaviour {


	private GameObject m_virus;
	private Material m_material;
	// Use this for initialization
	void Start () {
		m_virus = GameObject.FindGameObjectWithTag ("Virus");
		m_material = this.gameObject.GetComponent<SpriteRenderer> ().material;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 transform = m_virus.transform.position;
		m_material.SetVector ("_LightPos", new Vector4(transform.x,transform.y, transform.z, 0));
	}
}
