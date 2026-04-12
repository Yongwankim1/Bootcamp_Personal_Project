using UnityEngine;

[System.Serializable]
public struct EnemyData
{
    public float hpMultiplier;
}
[CreateAssetMenu(fileName = "EnemyData",menuName = "EnemySO/EnemyData")]
public class EnemyLevelScriptable : ScriptableObject
{
    public EnemyData enemyData;
}
