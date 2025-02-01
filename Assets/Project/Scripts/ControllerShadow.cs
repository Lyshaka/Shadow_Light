using UnityEngine;

public class ControllerShadow : MonoBehaviour
{
	[HideInInspector] public static ControllerShadow Instance { get; private set; }

	// Public properties
	[HideInInspector] public IInteractable interactor;
	
	// Private properties
	[Header("Movement")]
	[SerializeField] float moveSpeed = 10f;

	[Header("Jump")]
	[SerializeField] float jumpForce = 50f;
	[SerializeField] float gravityForce = 50f;

	[Header("Ladders")]
	[SerializeField] float climbSpeed = 6f;

	[Header("Technical")]
	[Header("Ground Check")]
	[SerializeField] Transform groundCheckTr;
	[SerializeField] LayerMask groundLayer;
	[SerializeField] Vector3 groundCheckBoxSize;

	bool _grounded;
	int _inLadder;
	bool _climbing;
	Rigidbody rb;

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

	// Update is called once per frame
	void Update()
	{
		_grounded = IsGrounded();
		//Debug.Log("grounded : " + grounded);

		HandleMove();

		HandleJump();

		HandleClimbing();

		HandleInteract();
	}

	void FixedUpdate()
	{
		HandleGravity();
	}

	void HandleMove()
	{
		Vector3 velocity = rb.linearVelocity;
		velocity.x = InputActionShadow.Instance.Move * moveSpeed;
		rb.linearVelocity = velocity;
	}

	void HandleJump()
	{
		if ((_grounded || _climbing) && InputActionShadow.Instance.Jump)
		{
			_climbing = false;
			InputActionShadow.Instance.Jump = false;
			rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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

	bool IsGrounded()
	{
		return Physics.CheckBox(groundCheckTr.position, groundCheckBoxSize / 2, Quaternion.identity, groundLayer);
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
	}
}
