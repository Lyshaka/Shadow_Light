using System;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
	[HideInInspector] public static UserInterface Instance { get; private set; }

	[SerializeField] public GameObject mainCanvas;

	[SerializeField] public GameObject keyToShowShadow;
	[SerializeField] public Image bubbleImage;

	[Header("Controllers")]
	[SerializeField] ControllerType controllerType = ControllerType.Keyboard;
	[SerializeField] ActionSprites keyboard;


	ActionSprites _activeSprites;

	public enum ControllerType
	{
		None,
		Keyboard,
		Playstation,
		Xbox,
	}

	public enum Actions
	{
		None,
		Interact,
		Jump,
		Move,
		Climb
	}

	[Serializable]
	public class ActionSprites
	{
		public Sprite interact;
		public Sprite jump;
		public Sprite move;
		public Sprite climb;
	}

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	private void Start()
	{
		mainCanvas.SetActive(true);

		switch (controllerType)
		{
			case ControllerType.None:
				break;
			case ControllerType.Keyboard:
				_activeSprites = keyboard;
				break;
			case ControllerType.Playstation:
				break;
			case ControllerType.Xbox:
				break;
		}
	}

	public void ShowKeyShadow(Actions action)
	{
		if (action == Actions.None)
		{
			keyToShowShadow.SetActive(false);
			return;
		}

		keyToShowShadow.SetActive(true);
		keyToShowShadow.transform.position = Camera.main.WorldToScreenPoint(ControllerShadow.Instance.transform.position);

		switch (action)
		{
			case Actions.None:
				break;
			case Actions.Interact:
				bubbleImage.sprite = _activeSprites.interact;
				break;
			case Actions.Jump:
				break;
			case Actions.Move:
				break;
			case Actions.Climb:
				break;
			default:
				break;
		}
	}
}
