using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionShadow : MonoBehaviour
{
	[HideInInspector] public static InputActionShadow Instance { get; private set; }

	// Properties
	float _move;
	bool _jump;
	float _climb;
	bool _interact;

	// Attributes
	public float Move => _move;
	public bool Jump { get => _jump;  set => _jump = value; }
	public float Climb => _climb;
	public bool Interact { get => _interact; set => _interact = value; }

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	public void OnMove(InputValue value)
	{
		_move = value.Get<float>();
	}

	public void OnJump(InputValue value)
	{
		_jump = value.Get<float>() > 0f;
	}

	public void OnClimb(InputValue value)
	{
		_climb = value.Get<float>();
	}

	public void OnInteract(InputValue value)
	{
		_interact = value.Get<float>() > 0f;
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	static void ResetSingleton()
	{
		Instance = null;
	}
}
