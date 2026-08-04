using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyConfig", menuName = "Configs/Enemy Config")]

public class EnemyConfig : ScriptableObject
{
    public int maxHealth;
    public int attackEnemyDamage;
    public float attackInterval;
    public int enemyMovSpeed;
}
