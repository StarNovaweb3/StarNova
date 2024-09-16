using UnityEngine;
using TMPro;

public class Planet : MonoBehaviour
{
    public int level = 1;
    public float ships = 100f;
    public float stardust = 100f;
    public int defenseLevel = 100;
    public int fragmentsCollected = 0;

    public float initialShipProductionRate = 100f;  // Ships produced per hour
    public float initialStardustProductionRate = 100f; // Stardust produced per hour
    private float shipProductionRatePerSecond;
    private float stardustProductionRatePerSecond;
    private float upgradeCost = 100f;

    private float productionInterval = 1.0f; // Time interval in seconds
    private float productionTimer = 0.0f;

    public TMP_Text stardustText;
    public TMP_Text shipsText;
    public TMP_Text fragmentsText;
    public TMP_Text defenseText;
    public TMP_Text planetIDText;

    private float lastStardust;
    private float lastShips;
    private int lastFragments;
    private int lastDefenseLevel;

    public int planetID; // Unique ID for the planet
    public string playerID; // Player ID that owns this planet

    private bool isProducingStardust = true;

    private UserProfile owner; // Reference to the owner

    void Start()
    {
        // Initialize production rates in per-second terms
        shipProductionRatePerSecond = initialShipProductionRate / 3600f;
        stardustProductionRatePerSecond = initialStardustProductionRate / 3600f;

        lastStardust = stardust;
        lastShips = ships;
        lastFragments = fragmentsCollected;
        lastDefenseLevel = CalculateDefenseLevel();

        UpdateUI();

        Debug.Log($"[{gameObject.name}] Planet Initialized: Ships = {ships}, Stardust = {stardust}, Defense Level = {defenseLevel}");
        Debug.Log($"[{gameObject.name}] Ship Production Rate per Second: {shipProductionRatePerSecond}");
        Debug.Log($"[{gameObject.name}] Stardust Production Rate per Second: {stardustProductionRatePerSecond}");
    }

    void Update()
    {
        productionTimer += Time.deltaTime;
        if (productionTimer >= productionInterval)
        {
            ProduceResources();
            productionTimer = 0.0f;
        }
        CheckAndUpdateUI();
    }

    public void ProduceResources()
    {
        if (isProducingStardust)
        {
            float stardustToAdd = stardustProductionRatePerSecond * productionInterval; // Calculate based on interval
            float shipsToAdd = shipProductionRatePerSecond * productionInterval;

            stardust += stardustToAdd;
            ships += shipsToAdd;

            Debug.Log($"[{gameObject.name}] Produced Resources: Added {stardustToAdd} stardust, {shipsToAdd} ships.");
        }
    }

    public void CollectFragments(int fragments)
    {
        fragmentsCollected += fragments;
        Debug.Log($"[{gameObject.name}] Collected Fragments: {fragments}. Total Fragments: {fragmentsCollected}");
    }

    public void UpdatePlanetAfterAttack(int lostShips, float stardustLossPercentage)
    {
        ships -= lostShips;
        stardust = Mathf.Max(0, stardust - (stardust * stardustLossPercentage));

        Debug.Log($"[{gameObject.name}] After Attack: Lost {lostShips} ships. Stardust loss: {(int)(stardust * stardustLossPercentage)}");
        Debug.Log($"[{gameObject.name}] Remaining: {ships} ships, {stardust} stardust.");

        CheckAndUpdateUI();
    }

    public void UpgradePlanet()
    {
        if (stardust >= upgradeCost)
        {
            stardust -= upgradeCost;
            level++;

            shipProductionRatePerSecond *= 1.13f; // Increase production rate by 13%
            stardustProductionRatePerSecond *= 1.1f; // Increase production rate by 10%
            defenseLevel = (int)(defenseLevel * 1.06f); // Increase defense level by 6%
            upgradeCost *= 1.5f; // Increase upgrade cost by 50%

            Debug.Log($"[{gameObject.name}] Planet Upgraded: New Level: {level}, New Stardust Production Rate: {stardustProductionRatePerSecond * 3600f}, New Ship Production Rate: {shipProductionRatePerSecond * 3600f}, New Defense Level: {defenseLevel}, New Upgrade Cost: {upgradeCost}");

            CheckAndUpdateUI(); // Update the UI after upgrade
        }
        else
        {
            Debug.LogWarning($"[{gameObject.name}] Not enough stardust to upgrade the planet.");
        }
    }

    public float GetUpgradeCost() { return upgradeCost; }
    public int GetLevel() { return level; }
    public float GetShipProductionRate() { return shipProductionRatePerSecond * 3600f; } // Convert back to per hour
    public float GetStardustProductionRate() { return stardustProductionRatePerSecond * 3600f; } // Convert back to per hour
    public int GetDefenseLevel() { return defenseLevel; }
    public int GetFragmentsCollected() { return fragmentsCollected; }
    public int GetShips() { return (int)ships; }
    public int GetStardust() { return (int)stardust; }

    // New Methods
    public string GetPlanetName()
    {
        return "Planet " + planetID; // Placeholder for actual name retrieval
    }

    public float GetProductionRate()
    {
        return GetShipProductionRate(); // Or return a combined rate if needed
    }

    public void CheckAndUpdateUI()
    {
        int currentDefenseLevel = CalculateDefenseLevel();

        if (stardust != lastStardust || ships != lastShips || fragmentsCollected != lastFragments || defenseLevel != lastDefenseLevel)
        {
            UpdateUI();
            lastStardust = stardust;
            lastShips = ships;
            lastFragments = fragmentsCollected;
            lastDefenseLevel = currentDefenseLevel;
        }
    }

    private void UpdateUI()
    {
        if (stardustText != null)
        {
            stardustText.text = "Stardust: " + Mathf.FloorToInt(stardust);
        }
        if (shipsText != null)
        {
            shipsText.text = "Ships: " + Mathf.FloorToInt(ships);
        }
        if (fragmentsText != null)
        {
            fragmentsText.text = "Fragments: " + fragmentsCollected;
        }
        if (defenseText != null)
        {
            defenseText.text = "Defense Level: " + CalculateDefenseLevel();
        }
        if (planetIDText != null)
        {
            planetIDText.text = "Planet ID: " + planetID; // Display Planet ID
        }

        Debug.Log($"[{gameObject.name}] UI Updated: Stardust: {stardust}, Ships: {ships}, Fragments: {fragmentsCollected}, Defense Level: {CalculateDefenseLevel()}");
    }

    private int CalculateDefenseLevel()
    {
        // Correct defense calculation
        return defenseLevel + Mathf.FloorToInt(ships / 2f);
    }

    public void StopStardustProduction()
    {
        isProducingStardust = false;
    }

    public void SetPlanetID(int id)
    {
        planetID = id;
    }

    public void SetPlayerID(string id)
    {
        playerID = id;
    }

    public void AssignOwner(UserProfile newOwner)
    {
        owner = newOwner;
    }

    public void AddPlanet(int newPlanetID)
    {
        // Logic to add a new planet
        Planet newPlanet = Instantiate(this); // Creates a new instance of the current planet
        newPlanet.SetPlanetID(newPlanetID);
        newPlanet.SetPlayerID(playerID);

        Debug.Log($"Added new planet with ID: {newPlanetID} for Player: {playerID}");
    }
}
