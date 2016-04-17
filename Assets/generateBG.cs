using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class RandomizeGO : System.Object
{
	[Header("Parralax plan prefab")]
	public GameObject randomGO;
	public int popProbability;
}


public class generateBG : MonoBehaviour {


	public RandomizeGO[] randomGO;
	public float fullinessFactor = 100;
	public float scale = 1;

	public int minForce;
	public int maxForce;


	public GameObject UpRigthLimit;
	public GameObject downLeftLimit;
	public float stepMax;
	public float stepMin;

	public bool Generate;
	private List<GameObject> m_gameObject;
	public Vector2 m_iterator;

	private int m_propabilitySomme=0;

	// Use this for initialization
	void Start () {
		m_gameObject = new List<GameObject>();
		if (randomGO.Length > 0) {
			foreach (RandomizeGO g in randomGO) {
				m_propabilitySomme += g.popProbability;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Generate) {
			generation ();
			Generate = false;
		}
	}


	float genereStep(){
		return Random.Range (stepMin,stepMax);
	}

	private int getIdOfNextAsset() {
		int random = Random.Range(0,m_propabilitySomme);
		for (int i = 0; i < randomGO.Length; i++) {
			random -= randomGO[i].popProbability;
			if (random < 0){
				return i;
			}
		}
		return -1;
	}


	void generation() {
		if (m_gameObject.Count > 0) {
			foreach (GameObject g in m_gameObject) {
				Destroy (g);
			}
		}
		m_iterator = downLeftLimit.transform.position;

		while(m_iterator.y < UpRigthLimit.transform.position.y){
			while (m_iterator.x < UpRigthLimit.transform.position.x) {
				float random = Random.Range (0, 100f);
				if(fullinessFactor>random) {
					int AssetId = getIdOfNextAsset ();
					GameObject asset = Instantiate (randomGO [AssetId].randomGO);
					asset.transform.position = new Vector3 (m_iterator.x,m_iterator.y,this.transform.position.z);
					asset.transform.parent = this.transform;
					asset.GetComponentInChildren<randomMove> ().maxForce = maxForce;
					asset.GetComponentInChildren<randomMove> ().minForce = maxForce;
					asset.transform.localScale = new Vector3 (scale,scale,1);
				
					m_gameObject.Add (asset);
				}

				m_iterator += genereStep () * Vector2.right;
			}
			m_iterator.x = downLeftLimit.transform.position.x;
			m_iterator += genereStep () * Vector2.up;
		}
	}
}
