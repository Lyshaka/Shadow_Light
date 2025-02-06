using UnityEngine;

public abstract class Activator : MonoBehaviour
{
	[Header("Objects to activate")]
	[SerializeField, Tooltip("Object that will be activated when the activator is activated")] Transform[] objectsToActivate;
	[SerializeField, Tooltip("Object that will be deactivated when the activator is deactivated")] Transform[] objectsToDeactivate;

	protected bool _activated;
	protected Activable[] _activableYes;
	protected Activable[] _activableNo;

	private void Start()
	{
		Initialize();
		ActivatorStart();
	}

	protected virtual void ActivatorStart() { }

	void Initialize()
	{
		_activableYes = new Activable[objectsToActivate.Length];
		_activableNo = new Activable[objectsToDeactivate.Length];

		for (int i = 0; i < objectsToActivate.Length; i++)
			_activableYes[i] = objectsToActivate[i].gameObject.GetComponent<Activable>();

		for (int i = 0; i < objectsToDeactivate.Length; i++)
			_activableNo[i] = objectsToDeactivate[i].gameObject.GetComponent<Activable>();

		ApplyChanges();
	}

	protected virtual void Toggle()
	{
		_activated = !_activated;
		ApplyChanges();
	}

	protected virtual void Activate(bool state)
	{
		_activated = state;
		ApplyChanges();
	}

	protected virtual void ApplyChanges()
	{
		for (int i = 0; i < _activableYes.Length; i++)
			_activableYes[i].Activate(_activated);

		for (int i = 0; i < _activableNo.Length; i++)
			_activableNo[i].Activate(!_activated);
	}
}
