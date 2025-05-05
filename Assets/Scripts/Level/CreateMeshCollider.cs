using UnityEngine;

public class AddMeshColliders : MonoBehaviour
{
    void Start()
    {
        AddMeshCollidersRecursive(transform);
    }

    void AddMeshCollidersRecursive(Transform obj)
    {
        if (obj.GetComponent<MeshRenderer>() != null)
        {
            if (obj.GetComponent<MeshCollider>() == null)
            {
                MeshCollider meshCollider = obj.gameObject.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = obj.GetComponent<MeshFilter>().sharedMesh;
            }
        }

        foreach (Transform child in obj)
        {
            AddMeshCollidersRecursive(child);
        }
    }
}
