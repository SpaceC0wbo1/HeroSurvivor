using UnityEngine;
using UnityEngine.AI;
using System;

public class BaseEnemy : MonoBehaviour
{
    protected Transform playerTransform;
    protected NavMeshAgent agent;
    protected HeroController heroController;

    public int maxHealth;
    public int attackEnemyDamage;
    public float attackInterval;
    public AudioSource audioSourceRef;
    public AudioClip damageHitSound;
    public GameObject hitEffect;

    public static event Action<int> OnEnemyDied;

    protected string enemyName = "Base Enemy";

    protected float nextAttackTime;
    protected int currentHealth;

    public virtual void Start()
    {
        Debug.Log($"Enemy type {enemyName} was created and spawned!");
        heroController = FindAnyObjectByType<HeroController>();
        agent = GetComponent<NavMeshAgent>();

        currentHealth = maxHealth;

        GameObject target = GameObject.Find("Hero");

        if (target != null)
        {
            playerTransform = target.transform;
            Debug.Log("Hero was successfully founded!");
        }
        else
        {
            Debug.LogError("ERROR: An GameObject with name 'Hero' was not found on the scene!");
        }
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;
        hitEffect.transform.SetParent(transform);
        hitEffect.transform.localPosition = Vector3.zero;
    }

    public virtual void TakeDamage(int damageAmount)
    {
        hitEffect.SetActive(false);
        if (audioSourceRef != null && damageHitSound != null)
        {
            audioSourceRef.PlayOneShot(damageHitSound);
        }

        hitEffect.SetActive(true);

        currentHealth -= damageAmount;
        Debug.Log($"Enemy took {damageAmount} damage. Current HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            OnEnemyDied?.Invoke(1);

            if (hitEffect != null)
            {
                hitEffect.transform.SetParent(null);
                hitEffect.SetActive(true);
            }

            gameObject.SetActive(false);
            
        }
    }
    void Update()
    {
        if (playerTransform == null) return;

        if (agent != null && agent.isOnNavMesh)
        {
            agent.SetDestination(playerTransform.position);
        }

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance <= 1.5f && Time.time >= nextAttackTime)
        {
            heroController.TakeDamage(attackEnemyDamage);

            nextAttackTime = Time.time + attackInterval;
        }
    }
}
