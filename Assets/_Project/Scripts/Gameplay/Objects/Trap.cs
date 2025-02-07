using UnityEngine;

public class Trap : MonoBehaviour, ILightable
{
	[SerializeField] float animDuration = 0.2f;
	[SerializeField] GameObject spikeObj;

	bool _lit;
	bool _inAnim;
	float _elapsedTime;

	private void Update()
	{
		if (_inAnim)
		{
			Vector3 scale = Vector3.one;

			if (_lit)
			{
				if (_elapsedTime < animDuration)
					_elapsedTime += Time.deltaTime;
				else
				{
					_inAnim = false;
					_elapsedTime = animDuration;
				}
			}
			else
			{
				if (_elapsedTime > 0f)
					_elapsedTime -= Time.deltaTime;
				else
				{
					_inAnim = false;
					_elapsedTime = 0f;
				}
			}

			scale.y = Mathf.Clamp(1f - (_elapsedTime / animDuration), 0.1f, 1f);
			spikeObj.transform.localScale = scale;
		}
	}

	public void Light(bool state)
	{
		_inAnim = true;
		_lit = state;
	}
}
