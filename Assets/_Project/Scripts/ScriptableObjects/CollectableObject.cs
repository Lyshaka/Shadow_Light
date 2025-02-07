using UnityEngine;

[CreateAssetMenu(fileName = "CollectableObject", menuName = "Scriptable Objects/CollectableObject")]
public class CollectableObject : ScriptableObject
{
	public Mesh mesh;
	public Material material;
}
