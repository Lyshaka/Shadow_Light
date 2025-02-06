using UnityEngine;

public class ActivatorLight : Activator
{
	[Header("Parameter")]
	[SerializeField] bool activateOnce;

	[Header("Technical")]
	[SerializeField] MeshRenderer activatorLightMeshRenderer;


	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 10)
		{
			if (activateOnce && !_activated)
				Activate(true);
			else
				Activate(true);
			activatorLightMeshRenderer.material = GameManager.Instance.GetActivatedMaterial(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == 10)
		{
			if (!activateOnce)
			{
				Activate(false);
				activatorLightMeshRenderer.material = GameManager.Instance.GetActivatedMaterial(false);
			}
		}
	}
}
