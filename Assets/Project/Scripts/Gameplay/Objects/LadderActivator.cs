using UnityEngine;

public class LadderActivator : MonoBehaviour, IActivable
{
	[SerializeField] Transform ladders;
	[SerializeField] float animationDuration = 0.5f;

	bool _activated;
	float _elapsedTime;

	void Update()
	{
		if (_activated)
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

		ladders.eulerAngles = new(0f, 180f - (_elapsedTime / animationDuration) * 180f, 0f);
	}

	public void Activate(bool state)
	{
		_activated = state;
		//Debug.Log("State : " + state);
	}
}
