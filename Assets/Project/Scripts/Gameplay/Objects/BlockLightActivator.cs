using UnityEngine;

public class BlockLightActivator : Activable
{
	[SerializeField] Transform leftSide;
	[SerializeField] Transform rightSide;
	[SerializeField] Collider coll;

	protected override void Animate(float percentage)
	{
		float zPos = Mathf.Lerp(0.25f, 0.5f, percentage);
		leftSide.localPosition = new Vector3(0f, 0f, zPos);
		rightSide.localPosition = new Vector3(0f, 0f, -zPos);

		float zScale = Mathf.Lerp(0.5f, 0f, percentage);
		leftSide.localScale = new Vector3(leftSide.localScale.x, leftSide.localScale.y, zScale);
		rightSide.localScale = new Vector3(leftSide.localScale.x, leftSide.localScale.y, zScale);
	}

	protected override void AnimationStart()
	{
		coll.enabled = true;
		leftSide.gameObject.SetActive(true);
		rightSide.gameObject.SetActive(true);
	}

	protected override void SetActivation()
	{
		coll.enabled = !activated;
		leftSide.gameObject.SetActive(!activated);
		rightSide.gameObject.SetActive(!activated);
	}
}
