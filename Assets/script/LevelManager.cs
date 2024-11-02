using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelData levelData; // Drag your LevelData asset here in the Inspector
    public GameObject portalPrefab; // Reference to your portal prefab (optional)

    void Start()
    {
        // Assign the Transform of an existing GameObject
         // Replace "Portal" with the name of your GameObject
        if (portalPrefab != null)
        {
            levelData.portalPosition = portalPrefab.transform;
        }
        
        // Alternatively, if you want to instantiate a prefab and assign its Transform:
        // GameObject portalInstance = Instantiate(portalPrefab, somePosition, Quaternion.identity);
        // levelData.portalPosition = portalInstance.transform;
    }
}
