using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using System;

public class EnemyController : MonoBehaviour
{
    private Transform playerTransform;
    private NavMeshAgent agent;
    private HeroController heroController;

    public int maxHealth;
    public int attackEnemyDamage;
    public float attackInterval;
    public AudioSource audioSourceRef;
    public AudioClip damageHitSound;
    public static event Action<int> OnEnemyDied;

    private float nextAttackTime;
    private int currentHealth;

    
   
    void Start()
    {
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
    }

    public void TakeDamage(int damageAmount)
    {
        if (audioSourceRef != null && damageHitSound != null)
        {
            audioSourceRef.PlayOneShot(damageHitSound);
        }

        currentHealth -= damageAmount;
        Debug.Log($"Enemy took {damageAmount} damage. Current HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            OnEnemyDied?.Invoke(1);
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
