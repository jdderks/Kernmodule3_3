using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAvailableNode : BTBaseNode
{
	private VariableGameObject target;

	public TargetAvailableNode(VariableGameObject target)
	{
		this.target = target;
	}

	public override TaskStatus Run()
	{
		if (target == null || target.Value.gameObject.activeSelf == false)
		{
			Debug.Log("failed");
			status = TaskStatus.Failed;
		}
		else
		{
			Debug.Log("succes");
			status = TaskStatus.Success;
		}

		return status;
	}
}
