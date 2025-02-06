using UnityEngine;

public class ShadowTrapCollider : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 11)
		{
			ControllerShadow.Instance.Kill();
		}
	}
}
