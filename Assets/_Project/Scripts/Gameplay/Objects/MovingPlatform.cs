using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
	[SerializeField] Transform pointA;
	[SerializeField] Transform pointB;
	[SerializeField] float movementDuration = 10f;
	[SerializeField] AnimationCurve movingCurve;

	float _elapsedTime;
	Rigidbody rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		//Debug.Log("Velocity : " + rb.linearVelocity);
	}

	private void FixedUpdate()
	{
		Vector3 targetPosition = Vector3.Lerp(pointA.position, pointB.position, movingCurve.Evaluate(_elapsedTime / movementDuration));

		if (_elapsedTime < movementDuration)
			_elapsedTime += Time.fixedDeltaTime;
		else
			_elapsedTime = 0f;

		rb.MovePosition(targetPosition);
	}
}
