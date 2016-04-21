using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class RandomizeGO : System.Object
{
	public GameObject randomGO;
	public int popProbability;
}

[System.Serializable]
public class ParralaxActivateGO : System.Object
{	
	public GameObject chooseGameObject;
	public Vector3 position;
}


public class generateBG : MonoBehaviour {

	public Camera m_camera;

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

	public Color parralaxColor;

	public List<ParralaxActivateGO> m_parralaxList;

	public Vector3 downLeft;
	public Vector3 upRight;

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
			generateList ();
			Generate = false;
		}

		generation ();
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

		if (m_parralaxList.Count > 0) {


			List<ParralaxActivateGO> removeParralaxObject = new List<ParralaxActivateGO>();
			float cameraOrthographiqueSize = m_camera.orthographicSize*2;
			float CameraW = m_camera.rect.width;
			float CameraH = m_camera.rect.height;
		
			upRight = new Vector3 (m_camera.transform.position.x + CameraW * cameraOrthographiqueSize, m_camera.transform.position.y + CameraH * cameraOrthographiqueSize,this.transform.position.z);
			downLeft = new Vector3 (m_camera.transform.position.x - CameraW * cameraOrthographiqueSize, m_camera.transform.position.y - CameraH * cameraOrthographiqueSize,this.transform.position.z);

			foreach(ParralaxActivateGO g in m_parralaxList) {
				if (g.position.x > downLeft.x && g.position.x < upRight.x && g.position.y > downLeft.y && g.position.y < upRight.y) {
					GameObject asset = Instantiate (g.chooseGameObject);
					asset.transform.position = g.position;
					asset.transform.parent = this.transform;
					asset.GetComponentInChildren<randomMove> ().maxForce = maxForce;
					asset.GetComponentInChildren<randomMove> ().minForce = maxForce;
					asset.transform.localScale = new Vector3 (scale,scale,1);
					asset.GetComponentInChildren<SpriteRenderer> ().color = parralaxColor;
					m_gameObject.Add (asset);
					removeParralaxObject.Add (g);
				}
					
			}
			if (removeParralaxObject.Count > 0) {
				foreach (ParralaxActivateGO r in removeParralaxObject) {
					m_parralaxList.Remove (r);
				}
			}

			/*
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
					asset.GetComponentInChildren<SpriteRenderer> ().color = parralaxColor;
					m_gameObject.Add (asset);
				}

				m_iterator += genereStep () * Vector2.right;
			}
			m_iterator.x = downLeftLimit.transform.position.x;
			m_iterator += genereStep () * Vector2.up;
		}*/
		}
	}


	void generateList (){
		if (m_parralaxList.Count > 0) {
			m_parralaxList.Clear();
		}
		m_iterator = downLeftLimit.transform.position;

		while(m_iterator.y < UpRigthLimit.transform.position.y){
			while (m_iterator.x < UpRigthLimit.transform.position.x) {
				float random = Random.Range (0, 100f);
				if(fullinessFactor>random) {
					int AssetId = getIdOfNextAsset ();
					ParralaxActivateGO parralaxGO = new ParralaxActivateGO();
					parralaxGO.chooseGameObject = randomGO [AssetId].randomGO;
					parralaxGO.position =  new Vector3 (m_iterator.x,m_iterator.y,this.transform.position.z);
					m_parralaxList.Add (parralaxGO);
				}

				m_iterator += genereStep () * Vector2.right;
			}
			m_iterator.x = downLeftLimit.transform.position.x;
			m_iterator += genereStep () * Vector2.up;
		}

	}
}
