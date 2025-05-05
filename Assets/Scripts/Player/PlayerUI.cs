using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI promptText; // Text that appears when player looks at an interactable object
   

    // Update is called when the player looks at an interactable object
    public void UpdateText(string promptMessage)
    {
        this.promptText.text = promptMessage;
    }
}
