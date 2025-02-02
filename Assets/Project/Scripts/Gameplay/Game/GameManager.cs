using UnityEngine;

public class GameManager : MonoBehaviour
{
	[HideInInspector] public static GameManager Instance { get; private set; }

	[Header("Materials")]
	[SerializeField] Material activatedMaterialTrue;
	[SerializeField] Material activatedMaterialFalse;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(this);
	}

	public Material GetActivatedMaterial(bool state)
	{
		return state ? activatedMaterialTrue : activatedMaterialFalse;
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	static void ResetSingleton()
	{
		Instance = null;
	}
}
