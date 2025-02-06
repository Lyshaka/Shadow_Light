using UnityEngine;

public class Attachable : MonoBehaviour
{
	[SerializeField] protected Transform pointToAttachTo;

	public Vector3 AttachPoint => pointToAttachTo.position;

	protected void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 10)
		{
			ControllerLight.Instance.objToAttach = transform;
		}
	}

	protected void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == 10)
		{
			ControllerLight.Instance.objToAttach = null;
		}
	}
}
