using UnityEngine;

public class ControllerLight : MonoBehaviour
{
	[HideInInspector] public static ControllerLight Instance { get; private set; }

	// Public properties
	[Header("Movement")]
	[SerializeField] float moveSpeedMax = 10f;
	[SerializeField] float acceleration = 10f;
	[SerializeField] float deceleration = 10f;


	[Header("Grow")]
	[SerializeField] float growDuration = 0.25f;
	[SerializeField] AnimationCurve growCurve;
	[SerializeField] float growMultiplicator = 1.5f;
	[SerializeField] Light pointLight;
	[SerializeField] TrailRenderer trailRenderer;

	[Header("Technical")]
	[SerializeField] Transform mesh;


	// Private properties
	Rigidbody rb;

	// Grow
	Vector3 _originalSize;
	float _originalLightRange;
	float _originalLightIntensity;
	float _growElapsedTime;
	float _originalTrailSize;
	bool _isGrowing;
	bool _isGrown;



	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.useGravity = false;
		_originalSize = mesh.localScale;
		_originalLightRange = pointLight.range;
		_originalLightIntensity = pointLight.intensity;
		_originalTrailSize = trailRenderer.startWidth;
	}

	void FixedUpdate()
	{
		HandleMove();
		HandleGrow();
	}

	void HandleMove()
	{
		Vector3 inputVelocity = new Vector3(InputActionLight.Instance.Move.x, InputActionLight.Instance.Move.y, 0f).normalized;
		if (inputVelocity.sqrMagnitude > 0f && !_isGrown)
		{
			rb.linearVelocity += acceleration * Time.fixedDeltaTime * inputVelocity;
			rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, moveSpeedMax);
		}
		else
		{
			if (rb.linearVelocity.magnitude > 0.1f)
			{
				rb.linearVelocity = rb.linearVelocity.normalized * Mathf.Max(0f, rb.linearVelocity.magnitude - deceleration * Time.fixedDeltaTime);
			}
			else
			{
				rb.linearVelocity = Vector3.zero;
			}
		}
	}

	void HandleGrow()
	{
		if (InputActionLight.Instance.Grow > 0f)
		{
			if (_growElapsedTime < growDuration)
			{
				_growElapsedTime += Time.deltaTime;
				_isGrowing = true;
				_isGrown = true;
			}
			else
			{
				_growElapsedTime = growDuration;
				_isGrowing = false;
			}
		}
		else
		{
			if (_growElapsedTime > 0f)
			{
				_growElapsedTime -= Time.deltaTime;
				_isGrowing = true;
				_isGrown = false;
			}
			else
			{
				_growElapsedTime = 0f;
				_isGrowing = false;
			}
		}

		if (_isGrowing)
		{
			SetLightSize(_growElapsedTime / growDuration);
		}
	}

	void SetLightSize(float percentage)
	{
		float mul = Mathf.Lerp(1f, growMultiplicator, percentage);

		mesh.localScale = _originalSize * mul;
		pointLight.range = _originalLightRange * mul * mul;
		pointLight.intensity = _originalLightIntensity * mul* mul;
		trailRenderer.startWidth = _originalTrailSize * mul;
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	static void ResetSingleton()
	{
		Instance = null;
	}

}
