using UnityEngine;

public class BlockLightActivator : MonoBehaviour, IActivable
{
	[SerializeField] float animationDuration = 0.2f;

	[SerializeField] Transform leftSide;
	[SerializeField] Transform rightSide;
	[SerializeField] Collider coll;

	bool _activated;
	float _elapsedTime;

	public void Activate(bool state)
	{
		_activated = state;
	}

	private void Update()
	{
		if (_activated)
		{
			if (_elapsedTime < animationDuration)
				_elapsedTime += Time.deltaTime;
			else
			{
				_elapsedTime = animationDuration;
				coll.enabled = false;
				leftSide.gameObject.SetActive(false);
				rightSide.gameObject.SetActive(false);
			}
		}
		else
		{
			coll.enabled = true;
			leftSide.gameObject.SetActive(true);
			rightSide.gameObject.SetActive(true);
			if (_elapsedTime > 0f)
				_elapsedTime -= Time.deltaTime;
			else
				_elapsedTime = 0f;
		}

		float zPos = Mathf.Lerp(0.25f, 0.5f, _elapsedTime / animationDuration);

		leftSide.localPosition = new Vector3 (0f, 0f, zPos);
		rightSide.localPosition = new Vector3 (0f, 0f, -zPos);

		float zScale = Mathf.Lerp(0.5f, 0f, _elapsedTime / animationDuration);

		leftSide.localScale = new Vector3(leftSide.localScale.x, leftSide.localScale.y, zScale);
		rightSide.localScale = new Vector3(leftSide.localScale.x, leftSide.localScale.y, zScale);
	}
}
