using UnityEngine;
using UnityEngine.UI; // For Slider
using TMPro; // For TMP_Text

public class AttackManager : MonoBehaviour
{
    public Planet playerPlanet; // Reference to the player's planet
    public Planet targetPlanet; // Reference to the target planet for attack

    public Slider shipPercentageSlider; // Slider to choose percentage of ships to send
    public TMP_Text attackInfoText; // Text to display attack info
    public Button attackButton; // Button to initiate the attack

    private float attackTimer = 0f;
    private float cooldownTimer = 0f;
    private bool isAttacking = false;
    private bool isCooldown = false;
    private float attackDuration = 60f; // Attack duration in seconds
    private float cooldownDuration = 30f; // Cooldown duration in seconds
    private int attackShips = 0;
    private AttackResult attackResult;

    private float lootedStardust = 0f; // To store looted stardust from target planet

    void Start()
    {
        if (shipPercentageSlider != null)
        {
            shipPercentageSlider.minValue = 0;
            shipPercentageSlider.maxValue = 1;
            shipPercentageSlider.wholeNumbers = false;
            shipPercentageSlider.value = 0;

            shipPercentageSlider.onValueChanged.AddListener(UpdateAttackInfoOnSliderChange);
        }

        if (attackButton != null)
        {
            attackButton.onClick.AddListener(OnAttackButtonClicked);
        }
    }

    void Update()
    {
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;
            float remainingAttackTime = Mathf.Max(0, attackDuration - attackTimer);

            if (remainingAttackTime <= 0)
            {
                EndAttack();
            }
            else
            {
                UpdateAttackInfo("Attacking... Ships Sent: " + attackShips +
                                  ", Time Remaining: " + Mathf.CeilToInt(remainingAttackTime) + "s");
            }
        }
        else if (isCooldown)
        {
            cooldownTimer += Time.deltaTime;
            float remainingCooldownTime = Mathf.Max(0, cooldownDuration - cooldownTimer);

            if (remainingCooldownTime <= 0)
            {
                EndCooldown();
            }
            else
            {
                UpdateAttackInfo("Cooldown... Time Remaining: " + Mathf.CeilToInt(remainingCooldownTime) + "s" +
                                  ", Result: " + (attackResult == AttackResult.Victory ? "V" : "L"));
            }
        }
    }

    public void OnAttackButtonClicked()
    {
        if (playerPlanet == null || targetPlanet == null)
        {
            Debug.LogWarning("Player or target planet not set.");
            return;
        }

        if (isAttacking || isCooldown)
        {
            Debug.LogWarning("Cannot start a new attack while in progress or during cooldown.");
            return;
        }

        float percentage = shipPercentageSlider.value;
        attackShips = Mathf.RoundToInt(playerPlanet.GetShips() * percentage);

        if (attackShips > 0 && playerPlanet.GetShips() >= attackShips)
        {
            playerPlanet.ships -= attackShips; // Deduct ships before starting the attack
            StartAttack(targetPlanet, attackShips);
        }
        else
        {
            Debug.LogWarning("Not enough ships to start the attack or invalid amount.");
        }
    }

    private void StartAttack(Planet target, int shipsToSend)
    {
        targetPlanet = target;
        isAttacking = true;
        attackTimer = 0f;
        Debug.Log("Attack started. Ships sent: " + shipsToSend);
        UpdateAttackInfo("Attacking... Ships Sent: " + shipsToSend + ", Time Remaining: " + attackDuration + "s");
    }

    private void EndAttack()
    {
        isAttacking = false;
        int survivingShips = CalculateSurvivingShips(targetPlanet.GetDefenseLevel());
        attackResult = survivingShips > 0 ? AttackResult.Victory : AttackResult.Loss;

        if (attackResult == AttackResult.Victory)
        {
            HandleVictory(survivingShips);
        }
        else
        {
            HandleDefeat();
        }

        isCooldown = true;
        cooldownTimer = 0f;
    }

    private void HandleVictory(int survivingShips)
    {
        // Calculate looted stardust (60% of enemy's stardust) and deduct it from the target planet
        lootedStardust = Mathf.RoundToInt(0.6f * targetPlanet.GetStardust());
        targetPlanet.stardust -= lootedStardust;

        // Attacking player does not collect fragments on victory
        playerPlanet.ships += survivingShips; // Add surviving ships back to player

        // Enemy planet loses all ships
        int enemyShipsLost = targetPlanet.GetShips();
        targetPlanet.ships = 0; // Set enemy ships to 0

        // Calculate fragments for the losing planet and add it to its total
        int fragmentsToAdd = Mathf.RoundToInt(0.6f * enemyShipsLost); // 60% of lost ships as fragments
        targetPlanet.CollectFragments(fragmentsToAdd); // Add fragments to enemy planet

        Debug.Log("Victory! Surviving Ships: " + survivingShips + ". Looted Stardust: " + lootedStardust + 
                  ". Fragments added to enemy planet: " + fragmentsToAdd);
        UpdateAttackInfo("Victory! Surviving Ships: " + survivingShips + 
                         ", Looted Stardust: " + lootedStardust +
                         ", Enemy Fragments Collected: " + fragmentsToAdd);
    }

    private void HandleDefeat()
    {
        // Player loses the attack and receives fragments
        int lostShips = attackShips;
        int fragmentsToCollect = Mathf.RoundToInt(lostShips * 0.6f); // 60% of lost ships as fragments
        playerPlanet.CollectFragments(fragmentsToCollect);

        // Enemy loses ships, but not more than the attacking ships sent
        int enemyShipLoss = CalculateEnemyShipLoss(targetPlanet.GetShips(), targetPlanet.GetDefenseLevel(), lostShips);
        targetPlanet.ships = Mathf.Max(0, targetPlanet.GetShips() - enemyShipLoss); // Reduce enemy planet ships

        if (targetPlanet.ships == 0)
        {
            // Enemy collects 60% of their lost ships as fragments
            int fragmentsToAdd = Mathf.RoundToInt(0.6f * enemyShipLoss);
            targetPlanet.CollectFragments(fragmentsToAdd); // Add fragments to enemy planet
        }

        Debug.Log("Loss. Fragments collected from defeat: " + fragmentsToCollect + 
                  ". Enemy Fragments Collected: " + (targetPlanet.ships == 0 ? Mathf.RoundToInt(0.6f * enemyShipLoss) : 0));
        UpdateAttackInfo("Loss. Fragments Collected: " + fragmentsToCollect);
    }

    private int CalculateSurvivingShips(int targetDefense)
    {
        int attackingShips = attackShips;
        int defense = targetDefense;

        if (attackingShips > defense)
        {
            return attackingShips - defense; // Some ships survive the attack
        }
        else
        {
            return 0; // All attacking ships are lost
        }
    }

    private int CalculateEnemyShipLoss(int enemyShips, int enemyDefense, int attackingShips)
    {
        if (attackingShips >= enemyDefense)
        {
            return Mathf.Min(attackingShips, enemyShips); // Cap ship loss by the attacking ship count
        }
        else
        {
            float lossPercentage = (float)attackingShips / enemyDefense; // Percentage of enemy ships lost
            return Mathf.Min(Mathf.RoundToInt(enemyShips * lossPercentage), attackingShips); // Capped by attacking ships
        }
    }

    private void EndCooldown()
    {
        isCooldown = false;

        // Add looted stardust to player planet after cooldown
        if (lootedStardust > 0)
        {
            playerPlanet.stardust += lootedStardust;
            Debug.Log("Looted Stardust Added to Player: " + lootedStardust);
            UpdateAttackInfo("Looted Stardust Added: " + lootedStardust);
            lootedStardust = 0; // Reset looted stardust after it's added
        }

        Debug.Log("Cooldown ended. Ready for a new attack.");
        UpdateAttackInfo("Ready to Attack");
    }

    private void UpdateAttackInfo(string message = "")
    {
        if (attackInfoText != null)
        {
            attackInfoText.text = message;
        }
    }

    private void UpdateAttackInfoOnSliderChange(float value)
    {
        if (playerPlanet != null)
        {
            int shipsToSend = Mathf.RoundToInt(playerPlanet.GetShips() * value);
            UpdateAttackInfo("Ships to Send: " + shipsToSend);
        }
    }

    private enum AttackResult
    {
        Victory,
        Loss
    }
}
