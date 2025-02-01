using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
	[Header("Objects to activate")]
	[SerializeField, Tooltip("Object that will be activated when the lever is turned on")] Transform[] objectsToActivate;
	[SerializeField, Tooltip("Object that will be deactivated when the lever is turned on")] Transform[] objectsToDeactivate;

	[Header("Parameters")]
	[SerializeField] Transform leverPivot;
	[SerializeField] float angle = 50f;
	[SerializeField] float animationDuration = 0.2f;
	[SerializeField] MeshRenderer activatedLight;
	[SerializeField] Material activatedMaterialTrue;
	[SerializeField] Material activatedMaterialFalse;

	bool _activated;
	float _elapsedTime;
	IActivable[] _activableYes;
	IActivable[] _activableNo;


	private void Start()
	{
		_activableYes = new IActivable[objectsToActivate.Length];
		_activableNo = new IActivable[objectsToDeactivate.Length];

		for (int i = 0; i < objectsToActivate.Length; i++)
			_activableYes[i] = objectsToActivate[i].gameObject.GetComponent<IActivable>();

		for (int i = 0; i < objectsToDeactivate.Length; i++)
			_activableNo[i] = objectsToDeactivate[i].gameObject.GetComponent<IActivable>();

		_activated = true;
		Toggle();
	}

	private void Update()
	{
		if (!_activated)
		{
			if (_elapsedTime < animationDuration)
				_elapsedTime += Time.deltaTime;
			else
				_elapsedTime = animationDuration;
		}
		else
		{
			if (_elapsedTime > 0f)
				_elapsedTime -= Time.deltaTime;
			else
				_elapsedTime = 0f;
		}

		leverPivot.eulerAngles = new(((_elapsedTime / animationDuration) * angle * 2f) - angle, 0f, 0f);
	}

	public void Interact()
	{
		Toggle();
	}

	void Toggle()
	{
		_activated = !_activated;

		for (int i = 0; i < _activableYes.Length; i++)
			_activableYes[i].Activate(!_activated);

		for (int i = 0; i < _activableNo.Length; i++)
			_activableNo[i].Activate(_activated);

		activatedLight.material = _activated ? activatedMaterialTrue : activatedMaterialFalse;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 11)
		{
			ControllerShadow.Instance.interactor = this;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == 11)
		{
			ControllerShadow.Instance.interactor = null;
		}
	}
}
