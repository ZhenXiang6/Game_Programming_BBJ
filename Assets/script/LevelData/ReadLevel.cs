using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/LevelData")]
public class LevelData : ScriptableObject
{
    public string levelName;
    public int maxStars;
    public int earnedStars;
    public Transform portalPosition;
    public int weightLimit;

    public int get_one_star;
    public int get_two_stars;
    public int get_three_stars;
}
