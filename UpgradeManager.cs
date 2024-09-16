using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public Planet planet; // Reference to the Planet instance

    void Start()
    {
        if (planet == null)
        {
            Debug.LogError("Planet reference not assigned in UpgradeManager.");
        }
    }

    public void UpgradePlanet()
    {
        if (planet != null)
        {
            // Calculate upgrade cost
            float upgradeCost = planet.GetUpgradeCost();
            int currentStardust = planet.GetStardust();

            if (currentStardust >= upgradeCost)
            {
                planet.UpgradePlanet(); // Call the method to upgrade the planet

                Debug.Log("Planet Upgraded: New Level: " + planet.GetLevel() + ", New Stardust Production Rate: " + planet.GetStardustProductionRate() +", New Ship Production Rate: " + planet.GetShipProductionRate() +", New Defense Level: " + planet.GetDefenseLevel() + ", New Upgrade Cost: " + planet.GetUpgradeCost());
                
                // Update the UI to reflect the new values
                planet.CheckAndUpdateUI();
            }
            else
            {
                Debug.LogWarning("Not enough stardust to upgrade the planet."); } } } }