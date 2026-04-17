using UnityEngine;

public class EnemyContainer : MonoBehaviour
{
    [SerializeField] GameObject[] enemy;

    public GameObject[] Enemy => enemy;
}
