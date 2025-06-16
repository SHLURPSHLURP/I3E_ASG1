using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    int maxHealth = 100;
    int currentHealth = 100;
    int currentScore = 0;
    int totalItems = 0;

    bool canInteract = false;
    bool hasLibertyCrystal = false;
    bool hasEnergyCrystal = false;

    float energyTimer = 0f;
    float energyInterval = 10f;

    CKeyBehaviour currentKey = null;
    DoorBehaviour currentDoor = null;
    bool doorClosed = true;
    ManaCrystalBehaviour currentManaCrystal = null;

    [SerializeField] Transform spawnPoint;
    [SerializeField] float interactionDistance = 5f;
    [SerializeField] GameObject deathScreenCanvas;
    bool isDead = false;

    void Start()
    {
        totalItems = FindObjectsOfType<CKeyBehaviour>().Length;
        UIManager.Instance.UpdateItemsLeft(totalItems);
        UIManager.Instance.UpdateScore(currentScore);
        UIManager.Instance.UpdateHealth(currentHealth, maxHealth);
    }

    //END OF START()

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

        Debug.Log(currentHealth);

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
    //END OF UPDATE()


    void OnInteract()
    {
        if (isDead)
        {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene(0); // Restart scene
            return;
        }

        if (canInteract)
        {
            if (currentKey != null)
            {
                Debug.Log("Interacting with crystal key");
                currentKey.Collect(this);

                totalItems--;
                UIManager.Instance.UpdateItemsLeft(totalItems);

                if (totalItems == 0) //once there are no more crystals
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

    //END OF ONINTERACT()

    public void ModifyScore(int amt)
    {
        currentScore += amt;
        UIManager.Instance.UpdateScore(currentScore);
    }

    //END OF MODIFYSCORE()

    public void ModifyHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        if (currentHealth < 0) currentHealth = 0;

        UIManager.Instance.UpdateHealth(currentHealth, maxHealth);

        if (amount < 0 && amount != -100)
        {
            UIManager.Instance.ShowTemporaryMessage($"You took {-amount} damage!");
        }

        // Death handling 
        if (currentHealth == 0 && !isDead)
        {
            isDead = true;
            Time.timeScale = 0f;
            if (deathScreenCanvas != null)
            {
                deathScreenCanvas.SetActive(true);
            }
        }
    }

    void Die()
    {
        isDead = true;
        Time.timeScale = 0f; // Optional: freezes time
        deathScreenCanvas.SetActive(true);
    }

    public bool HasAllCrystals()
    {
        return totalItems == 0;
    }

    //END OF MODIFYHEALTH()

    public int CurrentHealth()
    {
        return currentHealth;
    }

    public void SetHealthToMax()
    {
        currentHealth = maxHealth;
        UIManager.Instance.UpdateHealth(currentHealth, maxHealth);
    }



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

    //END OF ONTRIGGERENTER()

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

//END OF ONTRIGGEREXIT()
