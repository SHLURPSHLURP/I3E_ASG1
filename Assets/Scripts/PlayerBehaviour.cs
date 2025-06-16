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

    [SerializeField] Transform spawnPoint;
    [SerializeField] float interactionDistance = 5f;

    void Start()
    {
        totalItems = FindObjectsOfType<CKeyBehaviour>().Length;
        UIManager.Instance.UpdateItemsLeft(totalItems);
        UIManager.Instance.UpdateScore(currentScore);
        UIManager.Instance.UpdateHealth(currentHealth, maxHealth);
    }

    void Update()
    {
        // ENERGY CRYSTAL DAMAGE TIMER
        if (hasEnergyCrystal)
        {
            energyTimer += Time.deltaTime;
            if (energyTimer >= energyInterval)
            {
                ModifyHealth(-5);
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

    void OnInteract()
    {
        if (canInteract)
        {
            if (currentKey != null)
            {
                Debug.Log("Interacting with crystal key");
                currentKey.Collect(this);
                totalItems--;
                UIManager.Instance.UpdateItemsLeft(totalItems);

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
                    UIManager.Instance.ShowTemporaryMessage("Energy crystal collected. You feel a draining force... (Health will minus by -5 every 10s)");
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
        }
    }

    public void ModifyScore(int amt)
    {
        currentScore += amt;
        UIManager.Instance.UpdateScore(currentScore);
    }

    public void ModifyHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        if (currentHealth < 0) currentHealth = 0;

        UIManager.Instance.UpdateHealth(currentHealth, maxHealth);

        if (amount < 0)
        {
            UIManager.Instance.ShowTemporaryMessage($"You took {-amount} damage, be careful...");
        }

        // Death handling will come in later step
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
    }

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
    }
}
