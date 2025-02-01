using UnityEngine;

public class GameManager : MonoBehaviour
{
	[HideInInspector] public static GameManager Instance { get; private set; }

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(this);
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	static void ResetSingleton()
	{
		Instance = null;
	}
}
