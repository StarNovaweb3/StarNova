using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlanetSwipingManager : MonoBehaviour
{
    public TMP_Text planetIDText; // UI to display the selected planet's ID
    public Button previousButton; // Reference to the Previous Button
    public Button nextButton; // Reference to the Next Button

    private UserProfile playerProfile; // Reference to UserProfile
    private int currentPlanetIndex = 0;

    void Start()
    {
        // Initialize the UserProfile reference
        playerProfile = FindObjectOfType<UserProfile>();
        if (playerProfile == null)
        {
            Debug.LogError("UserProfile not found in the scene.");
            return;
        }

        // Initialize UI and button states
        UpdatePlanetUI();
        UpdateButtonVisibility();
    }

    // Move to the next planet in the list
    public void NextPlanet()
    {
        if (playerProfile != null && playerProfile.GetPlanetCount() > 0)
        {
            currentPlanetIndex = (currentPlanetIndex + 1) % playerProfile.GetPlanetCount(); // Infinite swiping
            
            UpdatePlanetUI();
            UpdateButtonVisibility();
        }
    }

    // Move to the previous planet in the list
    public void PreviousPlanet()
    {
        if (playerProfile != null && playerProfile.GetPlanetCount() > 0)
        {
            currentPlanetIndex = (currentPlanetIndex - 1 + playerProfile.GetPlanetCount()) % playerProfile.GetPlanetCount(); // Infinite swiping
            
            UpdatePlanetUI();
            UpdateButtonVisibility();
        }
    }

    // Updates the UI elements with the current planet's ID
    private void UpdatePlanetUI()
    {
        if (planetIDText == null)
        {
            Debug.LogError("Planet ID Text is not assigned.");
            return;
        }

        if (playerProfile == null || playerProfile.GetPlanetCount() <= 0)
        {
            planetIDText.text = "No planets available.";
            return;
        }

        Planet currentPlanet = playerProfile.GetPlanetAt(currentPlanetIndex);
        if (currentPlanet == null)
        {
            Debug.LogError($"Planet at index {currentPlanetIndex} is null.");
            return;
        }

        planetIDText.text = "Planet ID: " + currentPlanet.planetID;
    }

    // Updates the visibility of the Previous and Next buttons
    private void UpdateButtonVisibility()
    {
        if (previousButton == null || nextButton == null)
        {
            Debug.LogError("Previous or Next Button is not assigned.");
            return;
        }

        int planetCount = playerProfile != null ? playerProfile.GetPlanetCount() : 0;

        if (planetCount <= 1)
        {
            // Hide both buttons if there is only one planet
            previousButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);
        }
        else
        {
            // Show buttons and ensure they are enabled
            previousButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(true);

            // Optionally, disable the Previous button if on the first planet
            previousButton.interactable = currentPlanetIndex > 0;
            nextButton.interactable = currentPlanetIndex < planetCount - 1;
        }
    }
}
