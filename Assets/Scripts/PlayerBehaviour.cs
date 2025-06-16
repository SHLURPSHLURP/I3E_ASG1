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
    CKeyBehaviour currentKey = null;
    DoorBehaviour currentDoor = null;

    bool doorClosed = true;

    [SerializeField]
    Transform spawnPoint;

    [SerializeField]
    float interactionDistance = 5f;

    void Start()
    {
        UIManager.Instance.UpdateScore(currentScore);
        UIManager.Instance.UpdateHealth(currentHealth, maxHealth);
        UIManager.Instance.UpdateItemsLeft(FindObjectsOfType<CKeyBehaviour>().Length);
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
            }

            else if (currentDoor != null)
            {
                Debug.Log("Interacting with door");
                if (doorClosed == true)
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
                UIManager.Instance.UpdateHealth(currentHealth, maxHealth);
            }
        }
    }


    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        // Check if the player detects a trigger collider tagged as "Collectible" or "Door"
        if (other.CompareTag("Crystal"))
        {
            // Set the canInteract flag to true
            // Get the CoinBehaviour component from the detected object
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
        // Check if the player has a detected coin or door
        if (currentKey != null)
        {
            // If the object that exited the trigger is the same as the current coin
            if (other.gameObject == currentKey.gameObject)
            {
                // Set the canInteract flag to false
                // Set the current coin to null
                // This prevents the player from interacting with the coin
                canInteract = false;
                currentKey = null;
            }
        }
    }

    void Update()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hitInfo, interactionDistance))
        {
            //Debug.Log("Raycast hit: " + hitInfo.collider.gameObject.name);
            if (hitInfo.collider.gameObject.CompareTag("Crystal"))
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