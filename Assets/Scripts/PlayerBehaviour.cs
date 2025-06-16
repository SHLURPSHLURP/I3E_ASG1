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
        if (canInteract) //check if player can interact with objects
        {
            if (currentKey != null) //CRYSTAL KEY COLLECTION
            {
                Debug.Log("Interacting with crystal key");
                currentKey.Collect(this);

                totalItems--; // Decrease item count
                UIManager.Instance.UpdateItemsLeft(totalItems);

                // Check which crystal was collected by tag
                if (currentKey.CompareTag("GuidanceCrystal"))
                {
                    Debug.Log("Guidance Crystal collected!");
                    FindObjectOfType<MapToggle>().UnlockMap();
                }
                else if (currentKey.CompareTag("LibertyCrystal"))
                {
                    Debug.Log("Liberty Crystal collected!");
                    hasLibertyCrystal = true;
                }

                // After collecting, reset interaction flags
                canInteract = false;
                currentKey = null;
            }
            else if (currentDoor != null)
            {
                // Only allow door interaction if Liberty Crystal is collected
                if (!hasLibertyCrystal)
                {
                    Debug.Log("You need the Liberty Crystal to open this door!");
                    return;
                }

                Debug.Log("Interacting with door");
                if (doorClosed)
                {
                    currentDoor.Open();       // Opens the door
                    Debug.Log("Door open!");
                    doorClosed = false;       // Updates flag
                }
                else
                {
                    currentDoor.Close();      // Closes the door
                    Debug.Log("Door closed!");
                    doorClosed = true;        // Updates flag
                }
            }
        }
    }

    //CHANGING SCORE
    public void ModifyScore(int amt)
    {
        // Increase currentScore by the amount passed as an argument
        currentScore += amt;
        UIManager.Instance.UpdateScore(currentScore);
    }

    //CHANGING HEALTH
    public void ModifyHealth(int amount)
    {
        // Check if the current health is less than the maximum health
        // If it is, increase the current health by the amount passed as an argument
        if (currentHealth < maxHealth)
        {
            currentHealth += amount;
            // Check if the current health exceeds the maximum health
            // If it does, set the current health to the maximum health
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
        // Check if the player detects a trigger collider tagged as any crystal or door
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
        // Check if the player has a detected crystal or door and exited it
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
