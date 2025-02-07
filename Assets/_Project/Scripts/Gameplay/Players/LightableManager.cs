using UnityEngine;

public class LightableManager : MonoBehaviour
{
	SphereCollider _collider;

	private void Start()
	{
		_collider = GetComponent<SphereCollider>();
	}

	public void SetColliderSize(float size)
	{
		_collider.radius = size;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.TryGetComponent(out ILightable lightable))
		{
			lightable.Light(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.TryGetComponent(out ILightable lightable))
		{
			lightable.Light(false);
		}
	}
}
