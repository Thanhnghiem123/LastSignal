using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool useEvents;
    [SerializeField]
    public string promptMessage;

    public virtual string OnLook()
    {
        return promptMessage;
    }

    public void BaseInteract()
    {
        if (useEvents)
        {
            GetComponent<InteractionEvent>().OnInteract.Invoke();
        }
        Interact();
        Debug.Log("Thuc hien thanh cong goi Interact11111111111111");
    }

    // Allow overriding in child classes
    protected virtual void Interact() {
        //Debug.Log("Thuc hien thanh cong goi Interact22222222");
    }

}
