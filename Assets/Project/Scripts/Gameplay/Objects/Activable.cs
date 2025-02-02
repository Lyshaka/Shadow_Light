using UnityEngine;

public abstract class Activable : MonoBehaviour
{
	[SerializeField] protected float animationDuration = 0.2f;
	[SerializeField] protected bool activated;

	private float _elapsedTime = 0f;
	private bool _inAnimation = false;


	void Start()
	{
		ActivableStart();
		Animate(activated ? 1f : 0f);
		SetActivation();
	}

	void Update()
	{
		HandleAnimation();
	}

	protected virtual void ActivableStart() { }
	protected virtual void Animate(float percentage) { }
	protected virtual void AnimationStart() { }
	protected virtual void SetActivation() { }

	public void Activate(bool state)
	{
		if (activated != state)
		{
			if (!_inAnimation)
				AnimationStart();
			_inAnimation = true;
		}
		activated = state;
	}

	void HandleAnimation()
	{
		if (_inAnimation)
		{
			if (activated)
			{
				if (_elapsedTime < animationDuration)
					_elapsedTime += Time.deltaTime;
				else
				{
					SetActivation();
					_elapsedTime = animationDuration;
					_inAnimation = false;
				}
			}
			else
			{
				if (_elapsedTime > 0f)
					_elapsedTime -= Time.deltaTime;
				else
				{
					SetActivation();
					_elapsedTime = 0f;
					_inAnimation = false;
				}
			}

			Animate(Mathf.Clamp01(_elapsedTime / animationDuration));
		}
	}
}
