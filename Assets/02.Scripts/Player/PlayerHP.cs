using UnityEngine;

public class PlayerHP : CharacterHP
{
    [SerializeField] float equipHP;
    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (maxHP <= 0) maxHP = 1;
        currentHP = maxHP + equipHP;
    }

    protected override void Died()
    {
        base.Died();
    }
}
