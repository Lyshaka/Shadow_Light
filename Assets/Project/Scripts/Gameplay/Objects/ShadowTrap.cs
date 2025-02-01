using UnityEngine;

public class ShadowTrap : MonoBehaviour
{
	[SerializeField] Transform spikes;
	[SerializeField] Collider coll;

	public void SetSpikesSize(float size)
	{
		spikes.localScale = new Vector3(1f, size, 1f);

		coll.enabled = (size >= 0.3f);

	}
}
