using UnityEngine;

public class ControllerShadow : MonoBehaviour
{
	[HideInInspector] public static ControllerShadow Instance { get; private set; }

	// Public properties
	[HideInInspector] public IInteractable interactor;

	// Private properties
	[Header("Parameters")]

	[Header("Movement")]
	[SerializeField] float maxSpeed = 10f;
	[SerializeField] float acceleration = 20f;
	[SerializeField] float deceleration = 30f;

	[Header("Jump")]
	[SerializeField] float jumpForce = 50f;
	[SerializeField] float gravityForce = 50f;

	[Header("Ladders")]
	[SerializeField] float climbSpeed = 6f;

	[Header("Ground Check")]
	[SerializeField] Transform groundCheckTr;
	[SerializeField] LayerMask groundLayerMask;
	[SerializeField] LayerMask safeGroundLayerMask;
	[SerializeField] Vector3 groundCheckBoxSize;

	[Header("Reset")]
	[SerializeField, Tooltip("Time it takes for SHADOW to reset to a safe position when dying")] float resetDuration = 0.5f;
	[SerializeField, Tooltip("Curve to follow when SHADOW is resetting")] AnimationCurve resetCurve;
	[SerializeField, Tooltip("Curve to scale SHADOW when he is resetting")] AnimationCurve resetScaleCurve;
	[SerializeField, Tooltip("VFX Object to activate when SHADOW is resetting")] GameObject resetVfxObject;

	[Header("Technical")]
	[SerializeField] Transform mesh;

	bool _grounded;
	int _inLadder;
	bool _climbing;
	Rigidbody rb;

	// Movement
	float _currentHVelocity;

	// Reset
	bool _isResetting = false;
	float _resetElapsedTime = 0f;
	Vector3 _resetStartPosition;
	Vector3 _lastSafePosition;

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
		_isResetting = false;
	}

	void Update()
	{
		if (_isResetting)
			HandleReset();
		else
		{
			GroundCheck();
			HandleMove();
			HandleJump();
			HandleClimbing();
			HandleInteract();
		}
	}

	void FixedUpdate()
	{
		if (!_isResetting)
			HandleGravity();
	}

	void HandleReset()
	{
		_resetElapsedTime += Time.deltaTime;

		float percentage = _resetElapsedTime / resetDuration;

		rb.MovePosition(Vector3.Lerp(_resetStartPosition, _lastSafePosition, resetCurve.Evaluate(percentage)));

		float s = resetScaleCurve.Evaluate(percentage);
		mesh.localScale = new(s, s, s);

		


		if (_resetElapsedTime > resetDuration)
		{
			_resetElapsedTime = 0;
			mesh.localScale = Vector3.one;
			rb.linearVelocity = Vector3.zero;
			GetComponent<Collider>().enabled = true;
			resetVfxObject.SetActive(false);
			resetVfxObject.GetComponentInChildren<TrailRenderer>().emitting = false;
			_isResetting = false;
		}
	}

	void HandleMove()
	{
		
		if (Mathf.Abs(InputActionShadow.Instance.Move) > 0f)
		{
			_currentHVelocity += InputActionShadow.Instance.Move * Time.deltaTime * acceleration;
			_currentHVelocity = Mathf.Clamp( _currentHVelocity, -maxSpeed, maxSpeed);
		}
		else
		{
			if (Mathf.Abs(_currentHVelocity) > 0f)
				_currentHVelocity -= Time.deltaTime * deceleration * Mathf.Sign(_currentHVelocity);
			else
				_currentHVelocity = 0f;
		}

		Vector3 velocity = rb.linearVelocity;
		velocity.x = _currentHVelocity;
		rb.linearVelocity = velocity;

		// Rotate the sphere according to horizontal speed
		mesh.eulerAngles -= new Vector3(0f, 0f, (velocity.x / (mesh.localScale.x * 0.5f)) * Time.deltaTime * Mathf.Rad2Deg);
	}

	void HandleJump()
	{
		if (InputActionShadow.Instance.Jump)
		{
			InputActionShadow.Instance.Jump = false;
			if (_grounded || _climbing)
			{
				_climbing = false;
				rb.AddForce(Vector3.up * jumpForce * rb.mass, ForceMode.Impulse);
			}
		}
	}
	void HandleGravity()
	{
		if (!_climbing)
		{
			Vector3 velocity = rb.linearVelocity;
			velocity.y -= gravityForce * Time.fixedDeltaTime;
			rb.linearVelocity = velocity;
		}
	}

	void HandleClimbing()
	{
		if (_inLadder > 0)
		{
			if (Mathf.Abs(InputActionShadow.Instance.Climb) > 0.2f)
				_climbing = true;


			if (_climbing)
			{
				Vector3 velocity = rb.linearVelocity;
				velocity.y = InputActionShadow.Instance.Climb * climbSpeed;
				rb.linearVelocity = velocity;
			}

			if (_grounded)
				_climbing = false;
		}
	}

	void HandleInteract()
	{
		if (interactor != null)
			UserInterface.Instance.ShowKeyShadow(UserInterface.Actions.Interact);
		else
			UserInterface.Instance.ShowKeyShadow(UserInterface.Actions.None);

		if (InputActionShadow.Instance.Interact)
		{
			InputActionShadow.Instance.Interact = false;
			if (interactor != null)
				interactor.Interact();
		}
	}

	void GroundCheck()
	{
		_grounded = Physics.CheckBox(groundCheckTr.position, groundCheckBoxSize / 2, Quaternion.identity, groundLayerMask);

		Vector3 tryPos = new(Mathf.FloorToInt(transform.position.x) + 0.5f, groundCheckTr.position.y, 0f);

		if (_grounded && Physics.CheckBox(tryPos, groundCheckBoxSize / 2, Quaternion.identity, safeGroundLayerMask))
			_lastSafePosition = tryPos + Vector3.up;
	}

	public void Kill()
	{
		_resetStartPosition = transform.position;
		_currentHVelocity = 0f;
		GetComponent<Collider>().enabled = false;
		resetVfxObject.SetActive(true);
		TrailRenderer tr = resetVfxObject.GetComponentInChildren<TrailRenderer>();
		tr.Clear();
		tr.emitting = true;
		_isResetting = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		switch (other.gameObject.layer)
		{
			case 6: // Ladder
				_inLadder++;
				break;
			default:
				break;

		}
	}

	private void OnTriggerExit(Collider other)
	{
		switch (other.gameObject.layer)
		{
			case 6: // Ladder
				_inLadder--;
				if (_inLadder == 0 && _climbing)
				{
					_climbing = false;
					rb.linearVelocity = Vector3.zero;
				}
				break;
			default:
				break;

		}
	}


	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;

		Gizmos.DrawWireCube(groundCheckTr.position, groundCheckBoxSize);

		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(_lastSafePosition, 0.2f);
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	static void ResetSingleton()
	{
		Instance = null;
	}

}
