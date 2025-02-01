using UnityEngine;

public class ActivatorLight : MonoBehaviour
{
	[Header("Parameter")]
	[SerializeField] GameObject activable;
	[SerializeField] bool activateOnce;

	[Header("Technical")]
	[SerializeField] MeshRenderer activatorLightMeshRenderer;
	[SerializeField] Material activatorLightMaterialTrue;
	[SerializeField] Material activatorLightMaterialFalse;

	bool _activated;
	IActivable _activable;

	void Start()
	{
		_activable = activable.GetComponent<IActivable>();
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 10)
		{
			if (activateOnce && !_activated)
			{
				_activated = true;
				_activable.Activate(_activated);
			}
			else
				_activable.Activate(true);
			activatorLightMeshRenderer.material = activatorLightMaterialTrue;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == 10)
		{
			if (!activateOnce)
			{
				_activable.Activate(false);
				activatorLightMeshRenderer.material = activatorLightMaterialFalse;
			}
		}
	}
}
