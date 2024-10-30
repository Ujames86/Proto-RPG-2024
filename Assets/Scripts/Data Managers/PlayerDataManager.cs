using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager playerDataManager;

    private void Awake()
    {
        if (playerDataManager == null)
        {
            playerDataManager = this;  // Assign the instance
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);  // Destroy duplicate
        }
    }
}

