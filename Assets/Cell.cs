using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour {

	public Color afraidColor;
	public Color corruptColor;

	public float speedThreshold;
	public float accelerationSpeedThreshold;
	public int minForce;
	public int maxForce;
	public int nbMoveBeforeStopRun;


	private bool m_run;
	public bool m_isAfraid;
	private int m_nbTimeRun;
	private Vector2 m_endFusionPosition;

	private float m_speed;
	private Vector3 m_OldPosition;
	private bool m_accelerationPhase;

	public SpriteRenderer exterior;
	public SpriteRenderer interior;

	public int nbParticule = 20;

	public bool isTrueCell;
	// Use this for initialization
	void Start () {
		m_speed = 0;
		m_OldPosition = this.transform.position;
		m_accelerationPhase = false;
		m_run = false;
		isTrueCell = true;

		//moveRandomDirection ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 speedVector = this.transform.position - m_OldPosition;
		m_speed = Mathf.Abs (speedVector.x) + Mathf.Abs (speedVector.y);
		m_OldPosition = this.transform.position;

		/*if (!m_accelerationPhase) {
			moveRandomDirection ();
		}*/
		if (m_speed < speedThreshold && !m_accelerationPhase) {
			if (m_run) {
				if (m_nbTimeRun < nbMoveBeforeStopRun) {
					run ();
					m_nbTimeRun++;
				} else {
					m_run = false;
					m_nbTimeRun = 0;
				}
			} else {
				moveRandomDirection ();
			}
		}
		if (m_speed > accelerationSpeedThreshold || m_speed == 0) {
			m_accelerationPhase = false;
		}
	
	}

	void moveRandomDirection() {
		Vector2 direction = Random.insideUnitCircle;
		int force = Random.Range (minForce,maxForce);
		//Vector3 forceVector = new Vector3(direction.x*force,direction.y*force,0);

		this.GetComponent<Rigidbody2D> ().AddRelativeForce (force * direction);
		m_accelerationPhase = true;
	}

	void run() {
		Vector2 direction = -(m_endFusionPosition - new Vector2(this.transform.position.x, this.transform.position.y));
		direction.Normalize();
		int force = Random.Range (minForce,maxForce);
		this.GetComponent<Rigidbody2D> ().AddRelativeForce (force * direction*2);
		m_accelerationPhase = true;
		setColor (afraidColor);
	}

    private float getRightVectorComponent(float x1, float x2) {
        float v = Mathf.Abs(x1-x2);
        if (x1 < x2) v = -v;
        return v;
    }

	public void startFusion(Vector3 virusPos){
		Debug.Log ("StartFusion");
		if (m_isAfraid) {
			m_run = true;
			m_endFusionPosition = this.transform.position;
            //TODO animation afarid here

            //this.gameObject.SetActive(false);
        } else {
			this.gameObject.GetComponent<Rigidbody2D> ().isKinematic = true;
			this.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
            Vector3 dir = transform.position - virusPos;
            float angleP = 0.0f;
            angleP = Mathf.Atan2(dir.y, dir.x);
            angleP = ((-180.0f / Mathf.PI * angleP)%360.0f) * (Mathf.PI / 180.0f);
            if (angleP < 0.0f) angleP += 2.0f * Mathf.PI;
            setFusionVectorDir((virusPos - transform.position).normalized, GetComponent<Rigidbody2D>().rotation);/* Vector3(
                getRightVectorComponent(transform.position.x, virusPos.x),
                getRightVectorComponent(transform.position.y, virusPos.y),
                getRightVectorComponent(transform.position.z, virusPos.z)), angleP);*/
        }
	}

    public void setFusionVectorDir(Vector3 dir, float angleP) {
        if (exterior != null) {
            Debug.Log(dir+" "+angleP);
            exterior.material.SetVector("_DirPenetration", new Vector3(dir.x, dir.y, 0.0f));
            exterior.material.SetFloat("_AnglePenetration", angleP);
        }
    }

	public void stopFusion() {
		
		m_endFusionPosition = this.transform.position;
		m_isAfraid = true;
		m_run = true;
        setFusionVectorDir(Vector3.zero, -1.0f);
        this.gameObject.GetComponent<Rigidbody2D> ().isKinematic = false;
		this.gameObject.GetComponent<BoxCollider2D> ().enabled = true;

		GameObject[] whiteCell = GameObject.FindGameObjectsWithTag("WhiteCell");
		for (int i = 0; i < whiteCell.Length; i++) {
			whiteCell [i].GetComponent<whiteCell> ().alerte();
		}
	}

	public void consume() {
		Debug.Log("CellNOMNOMNOM");
		this.gameObject.GetComponent<Rigidbody2D> ().isKinematic = true;
		this.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
		PlayerManager.m_instance.addDestroyCell ();
		isTrueCell = false;
		//setColor (corruptColor);
		exterior.enabled = false;
		interior.enabled = false;

		this.gameObject.GetComponent<ParticleSystem>().Emit(nbParticule);
		//this.gameObject.SetActive (false);
		//this.GetComponent<Animation> ().Play ("DeathANimation");
	}

	public bool isOnCamera() {
		Vector3 viewport = Camera.main.WorldToViewportPoint (this.transform.position);
		return (viewport.x >0 && viewport.x <1 && viewport.y >0 && viewport.y <1);
	}


	public void setColor(Color color) {
	/*	exterior.color = color;
        exterior.material.SetColor("_Color", color);
		interior.color = color;
		interior.material.SetColor("_Color", color);*/
	}
}
