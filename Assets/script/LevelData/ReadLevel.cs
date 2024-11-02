using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/LevelData")]
public class LevelData : ScriptableObject
{
    public string levelName;
    public int maxStars;
    public int earnedStars;
    public Transform portalPosition;
}
