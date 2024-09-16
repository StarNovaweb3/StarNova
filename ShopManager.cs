using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    // UI Elements
    public GameObject shopPanel;
    public TMP_Text stardustText;
    public TMP_Text moongemsText;
    public TMP_Text planetPriceText;
    public TMP_Text planetCountText;
    public TMP_Text shipsText;

    public Button buyPlanetButton;
    public Button buy500ShipsButton;
    public Button buyProductionBoostButton;
    public Button buyMoongemsButton;
    public Button buyMoongemsWithStardustButton; // New button for buying Moongems with Stardust

    // Planet purchase variables
    public float planetBaseCost = 100f;
    public float priceIncreaseFactor = 1.8f;
    private float currentPlanetPrice;

    // Other shop variables
    public float shipsCost = 200f;
    public float productionBoostCost = 150f;
    public float moongemsCost = 10f; // Cost of 1 Moongem in Stardust
    public float moongemsToBuy = 50f; // Amount of Moongems to buy with Stardust

    private UserProfile playerProfile; // Adjusted to UserProfile
    public GameObject planetPrefab; // Planet prefab for instantiation

    void Start()
    {
        playerProfile = FindObjectOfType<UserProfile>(); // Find player profile in the scene
        shopPanel.SetActive(false); // Start with shop panel closed

        // Hook up button listeners
        buyPlanetButton.onClick.AddListener(BuyPlanet);
        buy500ShipsButton.onClick.AddListener(Buy500Ships);
        buyProductionBoostButton.onClick.AddListener(BuyProductionBoost);
        buyMoongemsButton.onClick.AddListener(BuyMoongems);
        buyMoongemsWithStardustButton.onClick.AddListener(BuyMoongemsWithStardust); // Hook up the new button

        // Initialize UI and planet price
        currentPlanetPrice = planetBaseCost;
        UpdateCurrencyUI();
        UpdatePlanetPriceUI();
    }

    // Updates the currency UI for stardust, moongems, ships, and planets
    void UpdateCurrencyUI()
    {
        if (playerProfile != null)
        {
            stardustText.text = "Stardust: " + playerProfile.GetStardust().ToString("F2");
            moongemsText.text = "Moongems: " + playerProfile.GetMoongems().ToString("F2");
            shipsText.text = "Ships: " + playerProfile.GetShips().ToString("F2");
            planetCountText.text = "Planets: " + playerProfile.GetPlanetCount(); // Number of planets player owns
        }
    }

    // Updates the UI for the current price of the next planet
    void UpdatePlanetPriceUI()
    {
        planetPriceText.text = "Planet Price: " + currentPlanetPrice.ToString("F2") + " Moongems";
    }

    // Opens the shop and updates the UI
    public void OpenShop()
    {
        shopPanel.SetActive(true);
        UpdateCurrencyUI();
    }

    // Closes the shop
    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }

    // Method to buy a new planet
    public void BuyPlanet()
    {
        if (playerProfile != null && playerProfile.GetMoongems() >= currentPlanetPrice)
        {
            // Deduct the current planet price in Moongems
            playerProfile.SpendMoongems(currentPlanetPrice);

            // Instantiate a new planet using the planetPrefab
            GameObject newPlanetGO = Instantiate(planetPrefab, Vector3.zero, Quaternion.identity); // Adjust position and rotation as needed
            Planet newPlanet = newPlanetGO.GetComponent<Planet>();

            if (newPlanet != null)
            {
                // Add the new planet to the player's profile
                playerProfile.AddPlanet(newPlanet);

                Debug.Log("Bought a new planet for " + currentPlanetPrice + " Moongems.");

                // Increase the price for the next planet purchase
                currentPlanetPrice *= priceIncreaseFactor;

                // Update UI to reflect changes
                UpdateCurrencyUI();
                UpdatePlanetPriceUI();
            }
            else
            {
                Debug.LogWarning("Failed to instantiate a new planet.");
            }
        }
        else
        {
            Debug.LogWarning("Not enough Moongems to buy a new planet.");
        }
    }

    // Method to buy 500 ships
    public void Buy500Ships()
    {
        if (playerProfile != null && playerProfile.GetStardust() >= shipsCost)
        {
            // Deduct the cost from stardust and add 500 ships
            playerProfile.SpendStardust(shipsCost);
            playerProfile.AddShips(500);

            Debug.Log("Bought 500 ships for " + shipsCost + " Stardust.");

            // Update UI to reflect changes
            UpdateCurrencyUI();
        }
        else
        {
            Debug.LogWarning("Not enough Stardust to buy 500 ships.");
        }
    }

    // Method to buy a production boost
    public void BuyProductionBoost()
    {
        if (playerProfile != null && playerProfile.GetStardust() >= productionBoostCost)
        {
            // Deduct the cost from stardust and apply the production boost
            playerProfile.SpendStardust(productionBoostCost);
            // Assuming ApplyProductionBoost method exists or implement accordingly
            // playerProfile.ApplyProductionBoost();

            Debug.Log("Bought a production boost for " + productionBoostCost + " Stardust.");

            // Update UI to reflect changes
            UpdateCurrencyUI();
        }
        else
        {
            Debug.LogWarning("Not enough Stardust to buy production boost.");
        }
    }

    // Method to buy Moongems with Stardust
    public void BuyMoongemsWithStardust()
    {
        if (playerProfile != null && playerProfile.GetStardust() >= moongemsCost * moongemsToBuy)
        {
            // Deduct the cost from stardust and add Moongems
            playerProfile.SpendStardust(moongemsCost * moongemsToBuy);
            playerProfile.AddMoongems(moongemsToBuy);

            Debug.Log("Bought " + moongemsToBuy + " Moongems for " + (moongemsCost * moongemsToBuy) + " Stardust.");

            // Update UI to reflect changes
            UpdateCurrencyUI();
        }
        else
        {
            Debug.LogWarning("Not enough Stardust to buy " + moongemsToBuy + " Moongems.");
        }
    }

    // Method to buy Moongems (could be in-app purchases or currency exchange)
    public void BuyMoongems()
    {
        // This method may be replaced or removed if using the new Moongems purchase method
        // Implement according to your in-app purchase logic if needed
    }
}
