using UnityEngine;

[CreateAssetMenu(fileName = "SActor", menuName = "Scriptable Objects/SActor")]
public class SActor : ScriptableObject
{
    private string characterName;
    private int health;
    private int maxHealth;
    private int damage;
    private int movementRange;
    private bool isRanged;
    private bool isPlayerControlled;
}   
