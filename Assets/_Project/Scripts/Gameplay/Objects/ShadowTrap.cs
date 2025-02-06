using UnityEngine;

public class ShadowTrap : MonoBehaviour
{
	[Header("Parameters")]
	[SerializeField] float distanceToDeactivate = 4f;


	[Header("Technical")]
	[SerializeField] Transform spikes;
	[SerializeField] Collider coll;

	bool _inRange;
	float _maxDistance;
	float _delta;

	private void Start()
	{
		_maxDistance = GetComponent<SphereCollider>().radius;
		_delta = Mathf.Max(0f, _maxDistance - distanceToDeactivate);
	}

	void SetSpikesSize(float size)
	{
		spikes.localScale = new Vector3(1f, size, 1f);

		coll.enabled = (size >= 0.3f);

	}

	private void Update()
	{
		if (_inRange)
		{
			float distance = Vector3.Distance(transform.position, ControllerLight.Instance.gameObject.transform.position);

			if (distance >= distanceToDeactivate && distance < _maxDistance)
			{
				SetSpikesSize(Mathf.Clamp01((distance - distanceToDeactivate) / _delta));
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 10)
		{
			_inRange = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == 10)
		{
			_inRange = false;
		}
	}
}
