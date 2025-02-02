using UnityEngine;

public class Ladder : Activable
{
	[SerializeField] Transform rotator;

	protected override void Animate(float percentage)
	{
		rotator.eulerAngles = new(0f, 180f - (percentage) * 180f, 0f);
	}
}
