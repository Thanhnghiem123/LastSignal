using UnityEngine;

public class AddTagToChildren : MonoBehaviour
{
    void Start()
    {
        AddTagToChildrenRecursive(transform);
    }

    void AddTagToChildrenRecursive(Transform obj)
    {
        obj.tag = "LevelPart";

        foreach (Transform child in obj)
        {
            AddTagToChildrenRecursive(child);
        }
    }
}
