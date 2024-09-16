using UnityEngine;
using System.Collections.Generic;

public class PlanetManager : MonoBehaviour
{
    public static PlanetManager instance;
    
    private Dictionary<int, Planet> planets = new Dictionary<int, Planet>();
    private int nextPlanetID = 1; // Start with 1 for planet IDs

    void Awake()
    {
        // Ensure that there's only one instance of PlanetManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Create a new planet with a unique ID and add it to the manager
    public Planet CreatePlanet()
    {
        Planet newPlanet = new GameObject("Planet_" + nextPlanetID).AddComponent<Planet>();
        newPlanet.planetID = nextPlanetID++;
        planets[newPlanet.planetID] = newPlanet;
        return newPlanet;
    }

    // Get a planet by its ID
    public Planet GetPlanetByID(int id)
    {
        if (planets.TryGetValue(id, out Planet planet))
        {
            return planet;
        }
        return null;
    }

    // Remove a planet by its ID
    public void RemovePlanet(int id)
    {
        if (planets.ContainsKey(id))
        {
            Destroy(planets[id].gameObject);
            planets.Remove(id);
        }
    }
}
