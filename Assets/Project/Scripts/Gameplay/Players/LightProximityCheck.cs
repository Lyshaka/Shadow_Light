using System.Collections.Generic;
using UnityEngine;

public class LightProximityCheck : MonoBehaviour
{
	List<GameObject> nearObjects = new List<GameObject>();


	private void Update()
	{
		foreach (GameObject obj in nearObjects)
		{
			float distance = Vector3.Distance(transform.position, obj.transform.position);

			if (obj.TryGetComponent(out ShadowTrap shadowTrap))
			{
				shadowTrap.SetSpikesSize(Mathf.Clamp01(distance - 3f));
			}



		}
	}



	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 7)
		{
			if (!nearObjects.Contains(other.gameObject))
			{
				nearObjects.Add(other.gameObject);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == 7)
		{
			if (nearObjects.Contains(other.gameObject))
			{
				nearObjects.Remove(other.gameObject);
			}
		}
	}
}
