using UnityEngine;

public class BlockLightActivator : Activable
{
	[SerializeField] Transform sideA;
	[SerializeField] Transform sideB;
	[SerializeField] Collider coll;

	protected override void Animate(float percentage)
	{
		float zPos = Mathf.Lerp(0.25f, 0.5f, percentage);
		sideA.localPosition = new Vector3(0f, 0f, zPos);
		sideB.localPosition = new Vector3(0f, 0f, -zPos);

		float zScale = Mathf.Lerp(0.5f, 0f, percentage);
		sideA.localScale = new Vector3(sideA.localScale.x, sideA.localScale.y, zScale);
		sideB.localScale = new Vector3(sideB.localScale.x, sideB.localScale.y, zScale);
	}

	protected override void AnimationStart()
	{
		coll.enabled = true;
		sideA.gameObject.SetActive(true);
		sideB.gameObject.SetActive(true);
	}

	protected override void SetActivation()
	{
		coll.enabled = !activated;
		sideA.gameObject.SetActive(!activated);
		sideB.gameObject.SetActive(!activated);
	}
}
