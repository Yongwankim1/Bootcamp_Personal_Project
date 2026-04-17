using UnityEngine;
[System.Serializable]
public struct Stage
{
    public bool IsWaterRepair;
    public bool IsPowerRepair;
    public bool IsBossDie;

    public bool IsClear()
    {
        if (IsWaterRepair && IsPowerRepair) return true;
        return false;
    }
}

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    public Stage[] stages;
    public EnemyLevelScriptable[] enemyLevelScriptables;

    public int CurrentStage = 0;
    public EnemyLevelScriptable currentEnemyLevel;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetEnemyLevel(int index)
    {
        currentEnemyLevel = enemyLevelScriptables[index];
    }
}
