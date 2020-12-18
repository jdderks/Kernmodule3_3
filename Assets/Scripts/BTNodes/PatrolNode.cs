using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolNode : BTBaseNode
{
    WaypointManager waypointManager;
    NavMeshAgent agent;

    Transform target;
    float minDis = 0.5f;
    int wayPointNumber = 0;
    public PatrolNode(WaypointManager wm, NavMeshAgent nma)
    {
        this.waypointManager = wm;
        this.agent = nma;
    }

    public override TaskStatus Run()
    {
        if(!target)
        {
            target = waypointManager.GetBaseWayPoint();
            agent.SetDestination(target.position);
        }

        float distanceCheck = Vector3.Distance(agent.transform.position, target.position);
        if (agent.pathStatus == NavMeshPathStatus.PathInvalid || agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            status = TaskStatus.Failed;
            return status;
        }

        if (distanceCheck < minDis)
        {
            wayPointNumber++;
            if (wayPointNumber >= waypointManager.Waypoints.Count)
            {
                wayPointNumber = 0;
            }
            target = waypointManager.Waypoints[wayPointNumber];
            agent.SetDestination(target.position);
        }

        Debug.Log(wayPointNumber);
        status = TaskStatus.Success;
        return status;
    }
}
