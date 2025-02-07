#if UNITY_EDITOR
using Codice.CM.Common.Tree;
using UnityEditor;
#endif
using UnityEngine;

public class Collectable : MonoBehaviour
{
	[Header("Object")]
	[SerializeField] CollectableObject collectableObject;
	[SerializeField, Tooltip("Revolutions per second")] float rotationFrequency;
	[SerializeField, Tooltip("Hovers per second")] float hoverFrequency;
	[SerializeField, Tooltip("Hover displacement in both up and down directions")] float hoverDelta;
	[SerializeField, Tooltip("Curve hover will follow (keep a bell curve)")] AnimationCurve hoverCurve;

	[Header("Collect")]
	[SerializeField, Tooltip("Time it will take for the coin to be collected")] float collectDuration;
	[SerializeField, Tooltip("Amount the collectable will be displaced upward")] float collectDelta;
	[SerializeField, Tooltip("Revolutions per seconds")] float collectRotationFrequency;
	[SerializeField, Tooltip("Curve the collectable will follow upward")] AnimationCurve collectCurve;
	[SerializeField] GameObject collectParticles;

	[Header("Technical")]
	[SerializeField] Transform parentTransform;
	[SerializeField] MeshFilter meshFilter;
	[SerializeField] MeshRenderer meshRenderer;

	// Rotation
	float _rotationDuration;
	float _elapsedTimeRotation;

	// Hover
	float _hoverDuration;
	float _elapsedTimeHover;

	// Collect
	Vector3 _startPosition;
	bool _isCollected;
	float _elapsedTimeCollect;

	void Start()
	{
		_rotationDuration = 1f / rotationFrequency;
		_hoverDuration = 1f / hoverFrequency;
		_startPosition = transform.position;
	}

	void Update()
	{

		if (_isCollected)
		{
			float value = collectCurve.Evaluate(_elapsedTimeCollect / collectDuration);
			transform.position = _startPosition + new Vector3(0f, value * collectDelta, 0f);
			transform.localScale = new(1f - value, 1f - value, 1f - value);

			if (_elapsedTimeCollect < collectDuration)
				_elapsedTimeCollect += Time.deltaTime;
			else
				Destroy(gameObject, 0.2f);
		}

		// Rotation
		parentTransform.eulerAngles = new(0f, (_elapsedTimeRotation / _rotationDuration) * 360f, 0f);
		if (_elapsedTimeRotation < _rotationDuration)
			_elapsedTimeRotation += Time.deltaTime;
		else
			_elapsedTimeRotation = 0f;

		// Hover
		parentTransform.localPosition = new(0f, (hoverCurve.Evaluate(_elapsedTimeHover / _hoverDuration) * (hoverDelta * 2f)) - hoverDelta, 0f);
		if (_elapsedTimeHover < _hoverDuration)
			_elapsedTimeHover += Time.deltaTime;
		else
			_elapsedTimeHover = 0f;
	}

	public void GetReward()
	{
		Debug.Log("Bingo !");
		_rotationDuration = 1f / collectRotationFrequency;
		foreach (ParticleSystem ps in collectParticles.GetComponentsInChildren<ParticleSystem>())
			ps.Play();
		GetComponent<Collider>().enabled = false;
		_isCollected = true;
	}

	private void OnValidate()
	{
	#if UNITY_EDITOR
		EditorApplication.delayCall += ApplyChanges;
	#endif
	}

	void ApplyChanges()
	{
		if (meshFilter != null)
			meshFilter.mesh = collectableObject != null ? collectableObject.mesh : null;

		if (meshRenderer != null)
			meshRenderer.material = collectableObject != null ? collectableObject.material : null;
	}
}
