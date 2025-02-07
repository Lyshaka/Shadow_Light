using UnityEngine;

public class MovingPlatform : Movable
{
	[SerializeField] Transform pointA;
	[SerializeField] Transform pointB;

	Rigidbody rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	public override float DistanceToMove()
	{
		return Vector3.Distance(pointA.position, pointB.position);
	}

	public override void Move(float t)
	{
		rb.MovePosition(Vector3.Lerp(pointA.position, pointB.position, t));
	}
}
