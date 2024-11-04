using UnityEngine;

[CreateAssetMenu(fileName = "ArrowData", menuName = "Game/ArrowData")]
public class ArrowData : ScriptableObject
{
    public Transform destination; // 目的地物體
    public bool showArrow = false;
}
