using UnityEngine;

public class EnemyHP : CharacterHP
{
    [SerializeField] EnemyLevelScriptable levelScriptable;

    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        if (baseMaxHP <= 0) baseMaxHP = 1;
        currentHP = baseMaxHP * levelScriptable.enemyData.hpMultiplier;
    }

    public void Initialize(EnemyLevelScriptable enemySO)
    {
        levelScriptable = enemySO;
        currentHP = baseMaxHP * enemySO.enemyData.hpMultiplier;
    }

    protected override void Died()
    {
        base.Died();//액션 이벤트 호출

    }
}
