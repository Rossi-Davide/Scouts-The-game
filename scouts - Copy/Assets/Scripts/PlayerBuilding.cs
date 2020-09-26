using UnityEngine;


[CreateAssetMenu(fileName = "New PlayerBuilding", menuName = "PlayerBuilding")]
public class PlayerBuilding : ScriptableObject
{
    public new string name;
    public int maxLevel; //index, not normal number
    public int[] maxHealth;
    public int[] healthLossInterval;
    public int[] prices; //a price for each level
    public Vector3 clickListenerOffset;
    public int[] pointsGiven; //different points given for each level
}
