using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectWithData : MonoBehaviour
{
	public abstract BaseStatus GetStatus();
	public abstract void SetStatus(BaseStatus status);
	public abstract class BaseStatus
	{

	}
}
