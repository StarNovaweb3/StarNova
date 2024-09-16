using UnityEngine;

public class Creation : MonoBehaviour
{
    public UserProfile currentUserProfile;
    public Planet currentPlanet;
    public GameObject planetPrefab; // Reference to the Planet prefab

    void Start()
    {
        // Example player data
        string playerName = "PlayerOne";
        int playerID = 1;

        // Initialize UserProfile with a name and ID
        currentUserProfile = new UserProfile(playerName, playerID);

        // Initialize planet with an ID
        int planetID = GeneratePlanetID();  // Assuming this is a method that generates unique planet IDs

        // Instantiate a planet from prefab and assign its ID
        GameObject planetObject = Instantiate(planetPrefab, Vector3.zero, Quaternion.identity);
        currentPlanet = planetObject.GetComponent<Planet>();
        currentPlanet.SetPlanetID(planetID);

        // Add the planet to the user's profile
        currentUserProfile.AddPlanet(currentPlanet);

        // Initialize UI or other components as needed
        Debug.Log("UserProfile Created: " + currentUserProfile.playerName + ", Planet ID: " + planetID);
    }

    int GeneratePlanetID()
    {
        // Example logic for generating a unique planet ID
        return Random.Range(1, 10000);
    }

    void Update()
    {
        // Example logic for user creation or UI handling
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("User Name: " + currentUserProfile.playerName);
        }
    }
}
