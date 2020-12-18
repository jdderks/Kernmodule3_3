﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
	[SerializeField] private float viewAngle;
	[SerializeField] private float viewRadius;
	[SerializeField] private LayerMask targetMask;
	[SerializeField] private LayerMask obstacleMask;
	[SerializeField] private List<Transform> visibleTargets = new List<Transform>();

	public VariableFloat ViewAngle { get; set; }
	public VariableFloat ViewRadius { get; set; }
	public List<Transform> VisibleTargets { get => visibleTargets; set => visibleTargets = value; }

	private void Start()
	{
		StartCoroutine(FindTargetsWithDelay(0.2f));
	}

	public IEnumerator FindTargetsWithDelay(float delay)
	{
		while(true)
		{
			yield return new WaitForSeconds(delay);
			FindVisibleTargets();
        }
	}

	public Transform GetNearestTarget(Transform origin)
	{
		if(visibleTargets.Count == 0) return null;

		float nearestDistToTarget = Vector3.Distance(origin.position, visibleTargets[0].position);
		Transform nearestTarget = visibleTargets[0];
		foreach(Transform target in visibleTargets)
		{
			if(Vector3.Distance(transform.position, target.position) < nearestDistToTarget)
			{
				nearestDistToTarget = Vector3.Distance(transform.position, target.position);
				nearestTarget = target;
			}
		}
		return nearestTarget;
	}

	void FindVisibleTargets()
	{
		visibleTargets.Clear();

		Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

		for(int i = 0; i < targetsInViewRadius.Length; i++)
		{
			Transform target = targetsInViewRadius[i].transform;
			Vector3 dirToTarget = (target.position - transform.position).normalized;
			if(Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
			{
				float dstToTarget = Vector3.Distance(transform.position, target.position);
				if(!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
				{
					if(!visibleTargets.Contains(targetsInViewRadius[i].transform))
						visibleTargets.Add(target);
				}
			}
		}
	}

	public Vector3 DirFromAngle(VariableFloat angleInDegrees, bool angleIsGlobal)
	{
		if(!angleIsGlobal)
			angleInDegrees.Value += transform.eulerAngles.y;

		return new Vector3(Mathf.Sin(angleInDegrees.Value * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees.Value * Mathf.Deg2Rad));
	}
	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
	{
		if(!angleIsGlobal)
			angleInDegrees += transform.eulerAngles.y;

		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}
}
