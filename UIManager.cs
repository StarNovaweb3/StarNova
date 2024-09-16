using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public TMP_Text stardustText;
    public TMP_Text shipsText;
    public TMP_Text fragmentsText;
    public TMP_Text planetIDText;
    public TMP_Text playerNameText;
    public TMP_Text moongemsText; // New Text field to display Moongems

    private Planet[] playerPlanets; // Array to hold all of the player's planets
    private int currentPlanetIndex = 0; // Track the currently displayed planet

    public ShopManager shopManager; // Reference to ShopManager

    private UserProfile playerProfile; // Reference to UserProfile for getting Moongems

    void Start()
    {
        // Initialize UserProfile
        playerProfile = FindObjectOfType<UserProfile>();
        if (playerProfile == null)
        {
            Debug.LogError("UserProfile not found in the scene.");
            return;
        }

        // Find all player-owned planets in the scene
        playerPlanets = FindObjectsOfType<Planet>(); 
        if (playerPlanets.Length > 0)
        {
            UpdateUI(); // Update the UI for the first planet
        }
        else
        {
            Debug.LogError("No planets found in UIManager.");
        }
    }

    public void UpdateUI()
    {
        if (playerPlanets == null || playerPlanets.Length == 0)
        {
            Debug.LogError("Player planets array is null or empty.");
            return;
        }

        if (currentPlanetIndex < 0 || currentPlanetIndex >= playerPlanets.Length)
        {
            Debug.LogError($"Invalid planet index: {currentPlanetIndex}. Resetting to 0.");
            currentPlanetIndex = 0; // Reset to a valid index
        }

        Planet currentPlanet = playerPlanets[currentPlanetIndex];
        if (currentPlanet == null)
        {
            Debug.LogError($"Current planet at index {currentPlanetIndex} is null.");
            return;
        }

        // Update UI fields
        if (stardustText != null)
        {
            stardustText.text = "Stardust: " + currentPlanet.GetStardust();
        }
        else
        {
            Debug.LogError("Stardust Text is not assigned.");
        }

        if (shipsText != null)
        {
            shipsText.text = "Ships: " + currentPlanet.GetShips();
        }
        else
        {
            Debug.LogError("Ships Text is not assigned.");
        }

        if (fragmentsText != null)
        {
            fragmentsText.text = "Fragments: " + currentPlanet.GetFragmentsCollected();
        }
        else
        {
            Debug.LogError("Fragments Text is not assigned.");
        }

        if (planetIDText != null)
        {
            planetIDText.text = "Planet ID: " + currentPlanet.planetID;
        }
        else
        {
            Debug.LogError("Planet ID Text is not assigned.");
        }

        // Update playerNameText
        if (playerNameText != null)
        {
            int playerID;
            if (int.TryParse(currentPlanet.playerID, out playerID))
            {
                var userProfile = UserManager.instance.GetUserByID(playerID);
                if (userProfile != null)
                {
                    playerNameText.text = "Player Name: " + userProfile.playerName;
                }
                else
                {
                    Debug.LogWarning($"UserProfile not found for playerID: {playerID}");
                }
            }
            else
            {
                Debug.LogWarning($"Invalid playerID format: {currentPlanet.playerID}");
            }
        }
        else
        {
            Debug.LogError("Player Name Text is not assigned.");
        }

        // Update moongemsText
        if (moongemsText != null && playerProfile != null)
        {
            moongemsText.text = "Moongems: " + playerProfile.GetMoongems().ToString("F2");
        }
        else
        {
            Debug.LogError("Moongems Text is not assigned or UserProfile is null.");
        }

        Debug.Log($"UI Updated for Planet {currentPlanet.planetID}: Stardust: {currentPlanet.GetStardust()}, Ships: {currentPlanet.GetShips()}, Fragments: {currentPlanet.GetFragmentsCollected()}, Moongems: {playerProfile?.GetMoongems()}");
    }

    // Method to switch to the next planet in the player's list
    public void NextPlanet()
    {
        if (playerPlanets != null && playerPlanets.Length > 0)
        {
            currentPlanetIndex = (currentPlanetIndex + 1) % playerPlanets.Length; // Loop back to the first planet if we reach the end
            UpdateUI();
        }
    }

    // Method to switch to the previous planet in the player's list
    public void PreviousPlanet()
    {
        if (playerPlanets != null && playerPlanets.Length > 0)
        {
            currentPlanetIndex = (currentPlanetIndex - 1 + playerPlanets.Length) % playerPlanets.Length; // Loop to the last planet if we go below 0
            UpdateUI();
        }
    }

    // Method to handle purchasing a new planet
    public void BuyNewPlanet()
    {
        if (shopManager != null)
        {
            shopManager.BuyPlanet(); // Call BuyPlanet without expecting a return value

            // Re-fetch all planets and update the UI
            playerPlanets = FindObjectsOfType<Planet>(); 
            if (playerPlanets.Length > 0)
            {
                currentPlanetIndex = playerPlanets.Length - 1; // Switch to the new planet
                UpdateUI(); // Update UI to reflect the new planet
            }
            else
            {
                Debug.LogWarning("Failed to update planets after purchasing a new planet.");
            }
        }
        else
        {
            Debug.LogError("ShopManager reference is missing.");
        }
    }
}
