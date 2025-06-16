using UnityEngine;

/// Handles player health, score, interaction, and victory/death states.
public class PlayerBehaviour : MonoBehaviour
{
    /// Player's maximum health.
    int maxHealth = 100;

    /// Player's current health.
    int currentHealth = 100;

    /// Player's current score.
    int currentScore = 0;

    /// Number of collectible crystal keys left.
    int totalItems = 0;

    /// Whether the player can currently interact with something.
    bool canInteract = false;

    /// Whether the player has collected the Liberty Crystal.
    bool hasLibertyCrystal = false;

    /// Whether the player has collected the Energy Crystal.
    bool hasEnergyCrystal = false;

    /// Timer used for Energy Crystal damage over time.
    float energyTimer = 0f;

    /// How often Energy Crystal inflicts damage.
    float energyInterval = 10f;

    /// Reference to the current key the player is interacting with.
    CKeyBehaviour currentKey = null;

    /// Reference to the current door the player is near.
    DoorBehaviour currentDoor = null;

    /// Reference to the Mana Crystal in range.
    ManaCrystalBehaviour currentManaCrystal = null;

    /// Tracks if the door is currently closed.
    bool doorClosed = true;

    /// Tracks if the player is dead.
    bool isDead = false;

    /// Tracks if the player has completed the game.
    bool isVictorious = false;

    /// Transform used to determine interaction direction (usually the camera or player head).
    [SerializeField] Transform spawnPoint;

    /// Maximum distance for raycast interaction.
    [SerializeField] float interactionDistance = 5f;

    /// UI canvas shown when the player dies.
    [SerializeField] GameObject deathScreenCanvas;

    /// UI canvas shown on successful game completion.
    [SerializeField] GameObject successScreenCanvas;

    /// Initializes game values and UI elements.
    void Start()
    {
        totalItems = FindObjectsOfType<CKeyBehaviour>().Length;
        UIManager.Instance.UpdateItemsLeft(totalItems);
        UIManager.Instance.UpdateScore(currentScore);
        UIManager.Instance.UpdateHealth(currentHealth, maxHealth);
    }

    /// Handles interaction input, energy crystal damage over time, and raycast detection.
    void Update()
    {
        // ENERGY CRYSTAL DAMAGE TIMER
        if (hasEnergyCrystal)
        {
            energyTimer += Time.deltaTime;
            if (energyTimer >= energyInterval)
            {
                ModifyHealth(-10);
                energyTimer = 0f;
            }
        }

        // Restart game on death or success
        if (isDead && Input.GetKeyDown(KeyCode.E))
        {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            return;
        }

        if (isVictorious && Input.GetKeyDown(KeyCode.E))
        {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            return;
        }

        // RAYCAST DETECTION
        RaycastHit hitInfo;
        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hitInfo, interactionDistance))
        {
            if (hitInfo.collider.gameObject.CompareTag("Crystal")
                || hitInfo.collider.gameObject.CompareTag("GuidanceCrystal")
                || hitInfo.collider.gameObject.CompareTag("LibertyCrystal")
                || hitInfo.collider.gameObject.CompareTag("EnergyCrystal"))
            {
                if (currentKey != null)
                    currentKey.Unhighlight();

                canInteract = true;
                currentKey = hitInfo.collider.gameObject.GetComponent<CKeyBehaviour>();
                currentKey.Highlight();
            }
        }
        else if (currentKey != null)
        {
            currentKey.Unhighlight();
            currentKey = null;
        }
    }

    /// Called when the interact input is triggered (e.g., pressing E).
    void OnInteract()
    {
        AudioController.Instance.PlaySFX(AudioController.Instance.interactSFX);

        if (isDead || isVictorious)
            return;

        if (canInteract)
        {
            if (currentKey != null)
            {
                Debug.Log("Interacting with crystal key");
                currentKey.Collect(this);

                totalItems--;
                UIManager.Instance.UpdateItemsLeft(totalItems);

                if (totalItems == 0)
                {
                    UIManager.Instance.ShowFinalObjectiveTemporary("All sacred Crystal Keys are in your possession. Return them to the mana crystal at once.");
                }

                if (currentKey.CompareTag("GuidanceCrystal"))
                {
                    Debug.Log("Guidance Crystal collected!");
                    FindObjectOfType<MapToggle>().UnlockMap();
                    UIManager.Instance.ShowTemporaryMessage("Guidance crystal collected, press G for hints to other keys...");
                }
                else if (currentKey.CompareTag("LibertyCrystal"))
                {
                    Debug.Log("Liberty Crystal collected!");
                    hasLibertyCrystal = true;
                    UIManager.Instance.ShowTemporaryMessage("Liberty crystal collected, you may now open the door...");
                }
                else if (currentKey.CompareTag("EnergyCrystal"))
                {
                    Debug.Log("Energy Crystal collected!");
                    hasEnergyCrystal = true;
                    UIManager.Instance.ShowTemporaryMessage("Energy crystal collected. You feel a draining force... (Health will minus by -10 every 10s)");
                }
                else
                {
                    UIManager.Instance.ShowTemporaryMessage("Crystal Key collected!");
                }

                canInteract = false;
                currentKey = null;
            }
            else if (currentDoor != null)
            {
                if (!hasLibertyCrystal)
                {
                    Debug.Log("You need the Liberty Crystal to open this door...");
                    UIManager.Instance.ShowTemporaryMessage("You need the liberty crystal to open the door");
                    return;
                }

                Debug.Log("Interacting with door");
                if (doorClosed)
                {
                    currentDoor.Open();
                    Debug.Log("Door open!");
                    doorClosed = false;
                }
                else
                {
                    currentDoor.Close();
                    Debug.Log("Door closed!");
                    doorClosed = true;
                }
            }
            else if (currentManaCrystal != null)
            {
                currentManaCrystal.TryActivate(this);
            }
        }
    }

    /// Adds to the player's score and updates the UI.
    public void ModifyScore(int amt)
    {
        currentScore += amt;
        UIManager.Instance.UpdateScore(currentScore);
    }

    /// Modifies the player's health and handles death logic if it reaches zero.
    public void ModifyHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        if (currentHealth < 0) currentHealth = 0;

        UIManager.Instance.UpdateHealth(currentHealth, maxHealth);

        if (amount < 0 && amount != -100)
        {
            AudioController.Instance.PlaySFX(AudioController.Instance.damageSFX);
            UIManager.Instance.ShowTemporaryMessage($"You took {-amount} damage!");
        }

        if (currentHealth == 0 && !isDead)
        {
            isDead = true;
            Time.timeScale = 0f;
            AudioController.Instance.PlayDeath();
            if (deathScreenCanvas != null)
            {
                deathScreenCanvas.SetActive(true);
            }
        }
    }

    /// Returns true if the player has collected all crystal keys.
    public bool HasAllCrystals()
    {
        return totalItems == 0;
    }

    /// Returns the player's current health.
    public int CurrentHealth()
    {
        return currentHealth;
    }

    /// Fully heals the player and updates the health UI.
    public void SetHealthToMax()
    {
        currentHealth = maxHealth;
        UIManager.Instance.UpdateHealth(currentHealth, maxHealth);
    }

    /// Called when the player enters a trigger zone.
    /// Detects crystal keys, doors, or the mana crystal.
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);

        if (other.CompareTag("Crystal")
            || other.CompareTag("GuidanceCrystal")
            || other.CompareTag("LibertyCrystal")
            || other.CompareTag("EnergyCrystal"))
        {
            canInteract = true;
            currentKey = other.GetComponent<CKeyBehaviour>();
        }
        else if (other.CompareTag("Door"))
        {
            canInteract = true;
            currentDoor = other.GetComponent<DoorBehaviour>();
        }
        else if (other.CompareTag("ManaCrystal"))
        {
            canInteract = true;
            currentManaCrystal = other.GetComponent<ManaCrystalBehaviour>();
        }
    }

    /// Called when the player exits a trigger zone.
    /// Clears the reference to any interactable objects.
    void OnTriggerExit(Collider other)
    {
        if (currentKey != null && other.gameObject == currentKey.gameObject)
        {
            canInteract = false;
            currentKey = null;
        }
        if (currentDoor != null && other.gameObject == currentDoor.gameObject)
        {
            canInteract = false;
            currentDoor = null;
        }
        if (currentManaCrystal != null && other.gameObject == currentManaCrystal.gameObject)
        {
            canInteract = false;
            currentManaCrystal = null;
        }
    }
}
