using UnityEngine;

public abstract class Movable : MonoBehaviour
{
	public virtual void Move(float t) { }
	public virtual float DistanceToMove() { return 0f; }
}
