using System.Collections.Generic;
using UnityEngine;

public class UserProfile : MonoBehaviour
{
    public string playerName;
    public int playerID;
    public float stardust;
    public float moongems;
    public float ships;

    private List<Planet> ownedPlanets; // List of planets owned by the player

    // Constructor with parameters
    public UserProfile(string name, int id)
    {
        playerName = name;
        playerID = id;
        stardust = 1000f; // Initial stardust
        moongems = 50f;   // Initial moongems
        ships = 100f;     // Initial ships
        ownedPlanets = new List<Planet>(); // Initialize empty planet list
    }

    void Start()
    {
        // Any additional initialization can be done here
        // Create initial planet
        CreateInitialPlanet();
    }

    // Getters for player's resources
    public float GetStardust() { return stardust; }
    public float GetMoongems() { return moongems; }
    public float GetShips() { return ships; }

    // Get the number of planets the player owns
    public int GetPlanetCount() { return ownedPlanets.Count; }

    // Spend stardust and ensure it doesn't go below zero
    public void SpendStardust(float amount)
    {
        stardust = Mathf.Max(0, stardust - amount);
    }

    // Spend moongems and ensure it doesn't go below zero
    public void SpendMoongems(float amount)
    {
        moongems = Mathf.Max(0, moongems - amount);
    }

    // Add ships to the player's total
    public void AddShips(float amount)
    {
        ships += amount;
    }

    // Add moongems to the player's total
    public void AddMoongems(float amount)
    {
        moongems += amount;
    }

    // Add a new planet to the player's list of owned planets
    public void AddPlanet(Planet newPlanet)
    {
        ownedPlanets.Add(newPlanet);
        newPlanet.AssignOwner(this); // Link planet to this player
    }

    // Get the planet at the specified index
    public Planet GetPlanetAt(int index)
    {
        if (index >= 0 && index < ownedPlanets.Count)
        {
            return ownedPlanets[index];
        }
        return null; // Handle case where index is out of bounds
    }

    // Helper method to create an initial Planet (placeholder code, needs real implementation)
    private void CreateInitialPlanet()
    {
        Planet newPlanet = Instantiate(Resources.Load<Planet>("PlanetPrefab")); // Load from Resources or another method
        if (newPlanet != null)
        {
            AddPlanet(newPlanet);
        }
        else
        {
            Debug.LogError("Failed to load initial planet.");
        }
    }
}
