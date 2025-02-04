using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
	[HideInInspector] public static CameraBehavior Instance { get; private set; }

	[SerializeField, Range(1f, 100f)] float cameraDistanceMin = 20f;
	[SerializeField, Range(1f, 100f)] float cameraDistanceMax = 40f;
	[SerializeField, Range(1f, 2f)] float distanceMultiplicator = 1.1f;
	[SerializeField] float smoothDampTime = 0.3f;

	float _cameraDistance = 20f;
	Vector3 _currentVelocity;

	Vector3 _lightPos;
	Vector3 _shadowPos;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(this);
	}

	void LateUpdate()
	{
		_lightPos = ControllerLight.Instance.transform.position;
		_shadowPos = ControllerShadow.Instance.transform.position;

		Vector3 direction = _lightPos - _shadowPos;
		float verticality = Mathf.Abs(Mathf.Cos(Mathf.Atan2(direction.y, direction.x)));
		float multiplier = Mathf.Lerp(Camera.main.aspect, 1f, verticality);

		_cameraDistance = (Vector3.Distance(_lightPos, _shadowPos) * multiplier * distanceMultiplicator) / Mathf.Tan(Camera.main.fieldOfView * Mathf.Deg2Rad);
		_cameraDistance = Mathf.Clamp(_cameraDistance, cameraDistanceMin, cameraDistanceMax);
		
		Vector3 targetPosition = Vector3.Lerp(_lightPos, _shadowPos, 0.5f);
		targetPosition.z = -_cameraDistance;
		
		Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, targetPosition, ref _currentVelocity, smoothDampTime);
	}

	private void OnValidate()
	{
		if (cameraDistanceMax < cameraDistanceMin || cameraDistanceMin > cameraDistanceMax)
		{
			cameraDistanceMax = cameraDistanceMin;
			cameraDistanceMin = cameraDistanceMax;
		}
	}
}
