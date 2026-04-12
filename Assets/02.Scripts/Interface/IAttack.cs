using UnityEngine;

public interface IAttack
{
    public void Attack(Transform attakTransform, float attackDistance, Vector3 mouseWorldPos, LayerMask targetLayer);
}
