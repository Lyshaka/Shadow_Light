using UnityEngine;

public class ControllerLight : MonoBehaviour
{
	[HideInInspector] public static ControllerLight Instance { get; private set; }

	// Public properties
	[Header("Movement")]
	[SerializeField] float moveSpeedMax = 10f;
	[SerializeField] float acceleration = 10f;
	[SerializeField] float deceleration = 10f;

	[Header("Hovering")]
	[SerializeField] Transform hoveringTransform;
	[SerializeField] AnimationCurve hoveringCurve;
	[SerializeField] float hoveringFrequency = 1f;
	[SerializeField] float hoveringDelta = 0.2f;

	// Private properties
	Rigidbody rb;
	float _currentHoveringTime;

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
	}

	void Update()
	{
		HandleHovering();
	}

	void FixedUpdate()
	{
		HandleMove();
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

	void HandleHovering()
	{

		hoveringTransform.localPosition = new Vector3(0f, (hoveringCurve.Evaluate(_currentHoveringTime * hoveringFrequency) * hoveringDelta) - hoveringDelta / 2f, 0f);

		_currentHoveringTime += Time.deltaTime;
		if (_currentHoveringTime >= 1f / hoveringFrequency)
			_currentHoveringTime = 0f;
	}
}
