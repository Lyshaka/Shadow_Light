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
	[SerializeField] float growDuration = 0.5f;
	[SerializeField] float growMultiplicator = 1.5f;
	[SerializeField] Light pointLight;

	[Header("Technical")]
	[SerializeField] Transform mesh;


	// Private properties
	Rigidbody rb;

	// Grow
	Vector3 _originalSize;
	float _originalLightRange;
	float _originalLightIntensity;
	float _growElapsedTime;


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
	}

	void FixedUpdate()
	{
		HandleMove();
		HandleInteract();
	}

	void HandleMove()
	{
		Vector3 inputVelocity = new Vector3(InputActionLight.Instance.Move.x, InputActionLight.Instance.Move.y, 0f).normalized;
		if (inputVelocity.magnitude > 0f)
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

	void HandleInteract()
	{
		if (InputActionLight.Instance.Interact > 0f)
		{
			mesh.localScale = _originalSize * growMultiplicator;
			pointLight.range = _originalLightRange * growMultiplicator * growMultiplicator;
			pointLight.intensity = _originalLightIntensity * growMultiplicator * growMultiplicator;
		}
		else
		{
			mesh.localScale = _originalSize;
			pointLight.range = _originalLightRange;
			pointLight.intensity = _originalLightIntensity;
		}
	}

	void SetLightSize(float percentage)
	{
		mesh.localScale = _originalSize * growMultiplicator * percentage;
		pointLight.range = _originalLightRange * growMultiplicator * growMultiplicator * percentage;
		pointLight.intensity = _originalLightIntensity * growMultiplicator * growMultiplicator * percentage;
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	static void ResetSingleton()
	{
		Instance = null;
	}

}
