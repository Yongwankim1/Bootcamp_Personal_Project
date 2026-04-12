using UnityEngine;

public class EnemyHP : CharacterHP
{
    [SerializeField] EnemyLevelScriptable levelScriptable;

    private void Awake()
    {
        Initialize();
    }
    private void Initialize()
    {
        if (maxHP <= 0) maxHP = 1;
        currentHP = maxHP * levelScriptable.enemyData.hpMultiplier;
    }

    public void Initialize(EnemyLevelScriptable enemySO)
    {
        levelScriptable = enemySO;
        currentHP = maxHP * enemySO.enemyData.hpMultiplier;
    }

    protected override void Died()
    {
        base.Died();//액션 이벤트 호출

    }
}
