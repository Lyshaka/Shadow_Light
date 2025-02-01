using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionLight : MonoBehaviour
{
	[HideInInspector] public static InputActionLight Instance {  get; private set; }

	// Properties
	Vector2 move;

	// Attributes
	public Vector2 Move => move;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	public void OnMove(InputValue value)
	{
		move = value.Get<Vector2>();
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	static void ResetSingleton()
	{
		Instance = null;
	}

}
