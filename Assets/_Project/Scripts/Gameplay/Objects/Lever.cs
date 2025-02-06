using UnityEngine;

public class Lever : Activator, IInteractable
{
	[Header("Parameters")]
	[SerializeField] Transform leverPivot;
	[SerializeField] float angle = 50f;
	[SerializeField] float animationDuration = 0.2f;
	[SerializeField] MeshRenderer activatedLight;

	float _elapsedTime;

	protected override void ActivatorStart()
	{
		_elapsedTime = animationDuration;
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

	protected override void Toggle()
	{
		base.Toggle();
		activatedLight.material = GameManager.Instance.GetActivatedMaterial(_activated);
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
