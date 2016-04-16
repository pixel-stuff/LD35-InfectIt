using System;
using UnityEngine;

	public class CameraFollow : MonoBehaviour
	{
		public Transform target;
		public float damping = 0.4f;
		public float lookAheadFactor = 0.6f;
		public float lookAheadReturnSpeed = 0.6f;
		public float lookAheadMoveThreshold = 0.6f;

		public float lookUpFactor = 0.6f;
		public float lookUpReturnSpeed = 0.6f;
		public float lookUpMoveThreshold = 0.6f;

		public float bottomThreshold =0.0f;

		private float m_OffsetZ;
		private Vector3 m_LastTargetPosition;
		private Vector3 m_CurrentVelocity;
		private Vector3 m_LookAheadPos;
		private Vector3 m_LookUpPos;

		// Use this for initialization
		private void Start()
		{
			m_LastTargetPosition = target.position;
			m_OffsetZ = (transform.position - target.position).z;
			transform.parent = null;
		}


		// Update is called once per frame
		private void Update()
		{
			//float bottomThresholdWithCameraSize = bottomThreshold + this.GetComponent<Camera> ().orthographicSize;
			// only update lookahead pos if accelerating or changed direction
			float xMoveDelta = (target.position - m_LastTargetPosition).x;

			bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

			if (updateLookAheadTarget)
			{
			m_LookAheadPos = lookAheadFactor*Vector3.right*Mathf.Sign(xMoveDelta);
			}
			else
			{
				m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);
			}


		float yMoveDelta = (target.position - m_LastTargetPosition).y;
		bool updateLookUpTarget = Mathf.Abs(yMoveDelta) > lookUpMoveThreshold;
		if (updateLookUpTarget)
		{
			m_LookUpPos = lookUpFactor*Vector3.up*Mathf.Sign(yMoveDelta);
		}
		else
		{
			m_LookUpPos = Vector3.MoveTowards(Vector3.zero,m_LookUpPos, Time.deltaTime*lookUpReturnSpeed);
		}

		Vector3 aheadTargetPos = target.position + m_LookAheadPos + m_LookUpPos + Vector3.forward*m_OffsetZ;
			/*if (aheadTargetPos.y < bottomThresholdWithCameraSize) {
				aheadTargetPos = new Vector3(aheadTargetPos.x,bottomThresholdWithCameraSize,aheadTargetPos.z);
			}*/
			Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

			transform.position = newPos;

			m_LastTargetPosition = target.position;
		}
	}

