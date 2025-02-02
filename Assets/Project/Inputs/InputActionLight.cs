using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionLight : MonoBehaviour
{
	[HideInInspector] public static InputActionLight Instance {  get; private set; }

	// Properties
	Vector2 _move;
	float _interact;

	// Attributes
	public Vector2 Move => _move;
	public float Interact => _interact;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	public void OnMove(InputValue value)
	{
		_move = value.Get<Vector2>();
	}

	public void OnInteract(InputValue value)
	{
		_interact = value.Get<float>();
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	static void ResetSingleton()
	{
		Instance = null;
	}

}
