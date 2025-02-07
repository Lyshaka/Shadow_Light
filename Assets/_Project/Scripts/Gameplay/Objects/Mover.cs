using UnityEngine;

public class Mover : MonoBehaviour
{
	[SerializeField] Movable movable;
	[SerializeField] float moveSpeed;
	[SerializeField] AnimationCurve moveCurve;

	float _elapsedTime;
	float _movementDuration;

	private void Start()
	{
		_movementDuration = movable.DistanceToMove() / moveSpeed;
	}


	private void FixedUpdate()
	{
		movable.Move(moveCurve.Evaluate(_elapsedTime / _movementDuration));

		if (_elapsedTime < _movementDuration)
			_elapsedTime += Time.fixedDeltaTime;
		else
			_elapsedTime = 0f;
	}
}
