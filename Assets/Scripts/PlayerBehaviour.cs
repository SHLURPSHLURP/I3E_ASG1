using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    // Player's maximum health
    int maxHealth = 100;
    // Player's current health
    int currentHealth = 100;
    // Player's current score
    int currentScore = 0;
    // Flag to check if the player can interact with objects
    bool canInteract = false;
    // Number of items left
    int totalItems = 0;

    // Flag for whether the player has collected the Liberty Crystal
    bool hasLibertyCrystal = false;

    CKeyBehaviour currentKey = null;
    DoorBehaviour currentDoor = null;

    bool doorClosed = true;

    [SerializeField]
    Transform spawnPoint;

    [SerializeField]
    float interactionDistance = 5f;

    void Start()
    {
        totalItems = FindObjectsOfType<CKeyBehaviour>().Length;
        UIManager.Instance.UpdateItemsLeft(totalItems);
        UIManager.Instance.UpdateScore(currentScore);
        UIManager.Instance.UpdateHealth(currentHealth, maxHealth);
    }

    //WHEN PRESSING "E"
    void OnInteract()
    {
        if (canInteract)
        {
            if (currentKey != null) // Crystal key collection
            {
                Debug.Log("Interacting with crystal key");
                currentKey.Collect(this);

                totalItems--; // Decrease item count
                UIManager.Instance.UpdateItemsLeft(totalItems);

                // Crystal-specific logic
                if (currentKey.CompareTag("GuidanceCrystal"))
                {
                    Debug.Log("Guidance Crystal collected!");
                    FindObjectOfType<MapToggle>().UnlockMap();
                    UIManager.Instance.ShowTemporaryMessage("Guidance crystal collected, press G to open map and find other crystal keys");
                }
                else if (currentKey.CompareTag("LibertyCrystal"))
                {
                    Debug.Log("Liberty Crystal collected!");
                    hasLibertyCrystal = true;
                    UIManager.Instance.ShowTemporaryMessage("Liberty crystal collected, you may now open the door");
                }

                // Reset interaction
                canInteract = false;
                currentKey = null;
            }
            else if (currentDoor != null) // Door interaction
            {
                if (!hasLibertyCrystal)
                {
                    Debug.Log("You need the Liberty Crystal to open this door!");
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

    //CHANGING SCORE
    public void ModifyScore(int amt)
    {
        currentScore += amt;
        UIManager.Instance.UpdateScore(currentScore);
    }

    //CHANGING HEALTH
    public void ModifyHealth(int amount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += amount;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            UIManager.Instance.UpdateHealth(currentHealth, maxHealth);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);

        if (other.CompareTag("Crystal") || other.CompareTag("GuidanceCrystal") || other.CompareTag("LibertyCrystal"))
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

    void Update()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hitInfo, interactionDistance))
        {
            if (hitInfo.collider.gameObject.CompareTag("Crystal")
                || hitInfo.collider.gameObject.CompareTag("GuidanceCrystal")
                || hitInfo.collider.gameObject.CompareTag("LibertyCrystal"))
            {
                if (currentKey != null)
                {
                    currentKey.Unhighlight();
                }

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
}
