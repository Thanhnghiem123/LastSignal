using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles player interactions with the environment, including raycasting for interactable objects,
/// toggling the in-game menu, and managing related UI and gameplay states.
/// </summary>
public class PlayerInteract : MonoBehaviour
{
    // === Fields ===
    [Header("Interaction Settings")]
    [SerializeField] private Camera camera; // The player's camera used for raycasting.
    [SerializeField] private float distance = 3f; // Maximum distance for interaction raycasting.
    [SerializeField] private LayerMask mask; // Layer mask to filter interactable objects.

    private PlayerUI playerUI; // Manages the player's UI (e.g., interaction prompts).
    private InputManager inputManager; // Handles player input.
    private GameObject menu; // Reference to the in-game menu UI.
    private bool isMenuActive = false; // Tracks whether the menu is currently active.
    private Canvas[] allCanvases; // Cached list of all Canvas objects in the scene.

    // === Unity Methods ===

    /// <summary>
    /// Initializes components and caches necessary references at the start of the game.
    /// </summary>
    private void Start()
    {
        InitializeComponents();
        CacheAllCanvases(); // Cache all Canvas objects in the scene.
    }

    /// <summary>
    /// Handles interaction logic and menu toggling every frame.
    /// </summary>
    private void Update()
    {
        HandleInteraction();
        HandleMenuToggle();
     
    }

    // === Initialization ===

    /// <summary>
    /// Initializes required components and references.
    /// </summary>
    private void InitializeComponents()
    {
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();
        menu = GameObject.FindWithTag("Menu");

        if (menu != null)
        {
            menu.SetActive(false); // Ensure the menu is hidden at the start.
            Debug.LogWarning("Menu GameObject with tag 'Menu' has been disabled.");
        }
    }

    /// <summary>
    /// Caches all Canvas objects in the scene for efficient toggling.
    /// </summary>
    private void CacheAllCanvases()
    {
        allCanvases = FindObjectsOfType<Canvas>();
    }

    // === Interaction Logic ===

    /// <summary>
    /// Handles player interaction with objects in the environment using raycasting.
    /// </summary>
    private void HandleInteraction()
    {
        // Kiểm tra playerUI
        if (playerUI == null)
        {
            Debug.LogError("playerUI is null! Check if PlayerUI component is attached and initialized.");
            return;
        }

        playerUI.UpdateText(string.Empty); // Reset text
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red); // Đổi màu để dễ thấy

        if (Physics.Raycast(ray, out RaycastHit raycastHit, distance, mask))
        {

            Interactable interactable = raycastHit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                Debug.Log("Found Interactable with prompt: " + interactable.promptMessage);
                playerUI.UpdateText(interactable.promptMessage);

                if (inputManager.onFootActions.Interact.triggered)
                {
                    Debug.Log("Thuc hien interacttttttttttttttttt");
                    interactable.BaseInteract();
                    Debug.Log("Thuc hien thanh conggggggggggggggggg");
                }
            }
            else
            {
                Debug.LogWarning("Hit object has no Interactable component.");
            }
        }
        else
        {
            Debug.LogWarning("Raycast did not hit anything within " + distance + " units.");
        }
    }

    // === Menu Logic ===

    /// <summary>
    /// Toggles the in-game menu when the Escape key is pressed.
    /// </summary>
    private void HandleMenuToggle()
    {
        // Check if the cutscene is finished before allowing menu toggle
        IntroCutscene introCutscene = FindObjectOfType<IntroCutscene>();
        if (introCutscene != null && !introCutscene.cutsceneFinished)
        {
            Debug.LogWarning("Cannot open menu: Cutscene is still playing.");
            return; // Prevent menu toggle if cutscene is not finished
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    // === Enable/Disable Menu ===

    /// <summary>
    /// Toggles the in-game menu and updates related gameplay states.
    /// </summary>
    public void ToggleMenu()
    {
        isMenuActive = !isMenuActive; // Toggle the menu state.

        // Update cursor visibility and lock state.
        if (isMenuActive)
        {
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor.
            Cursor.visible = true; // Make the cursor visible.
        }
        else
        {
                Cursor.lockState = CursorLockMode.Locked;
        }

        if (menu != null)
        {

            menu.SetActive(isMenuActive); // Show or hide the menu.
            Debug.Log($"Menu is nowwwwwwwwwwwwwwwwwwwwwwwwww {(menu.activeSelf ? "enabled" : "disabled")}.");

            // Pause or resume the game.
            Time.timeScale = isMenuActive ? 0 : 1;

            // Toggle visibility of all other Canvas objects.
            foreach (Canvas canvas in allCanvases)
            {
                if (canvas.gameObject != menu)
                {
                    canvas.gameObject.SetActive(!isMenuActive); // Hide if the menu is active.
                }
            }

            // Enable or disable the GunScript based on the menu state.
            ToggleGunScript(isMenuActive);
        }
        else
        {
            Debug.LogWarning("Menu GameObject not found!");
        }
    }



    // === Enable/Disable GunScript ===

    /// <summary>
    /// Toggles the GunScript on all child objects of the player based on the menu state.
    /// </summary>
    /// <param name="isMenuActive">True if the menu is active, false otherwise.</param>
    public void ToggleGunScript(bool isMenuActive)
    {
        // Iterate through all child objects of the player.
        foreach (Transform child in transform)
        {
            // Check if the child object has a GunScript component.
            GunScript gunScript = child.GetComponent<GunScript>();
            if (gunScript != null)
            {
                // Enable or disable the GameObject containing the GunScript based on the menu state.
                child.gameObject.SetActive(!isMenuActive);
                Debug.Log($"GameObject '{child.name}' containing GunScript has been {(isMenuActive ? "disabled" : "enabled")}.");
            }
        }
    }

    /// <summary>
    /// Ensures the menu is closed and switches to scene 0.
    /// </summary>
    public void SwitchToScene0()
    {
        if (isMenuActive) // Check if the menu is active.
        {
            ToggleMenu(); // Close the menu.
        }

        Debug.Log("Switching to scene 0...");
        SceneManager.LoadScene(0); // Load scene with index 0.
    }

}
