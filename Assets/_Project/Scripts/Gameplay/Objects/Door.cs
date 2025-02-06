using UnityEngine;

public class Door : Activable
{
	[SerializeField] Transform doorPivot;
	[SerializeField] bool openToTheLeft;

	protected override void Animate(float percentage)
	{
		if (openToTheLeft)
			doorPivot.eulerAngles = new(0f, percentage * 90f, 0f);
		else
			doorPivot.eulerAngles = new(0f, percentage * -90f, 0f);
	}
}
