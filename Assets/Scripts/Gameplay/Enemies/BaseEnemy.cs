namespace HeroSurvivor.Gameplay.Enemies 
{
    using UnityEngine;
    using UnityEngine.AI;
    using System;
    using HeroSurvivor.Gameplay.Player;

    public class BaseEnemy : MonoBehaviour
    {
        protected Transform playerTransform;
        protected NavMeshAgent agent;
        protected HeroController heroController;

        [SerializeField] private int maxHealth;
        [SerializeField] private int attackEnemyDamage;
        [SerializeField] private float attackInterval;

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
            agent = GetComponent<NavMeshAgent>();
            currentHealth = maxHealth;
        }

        private void OnEnable()
        {
            currentHealth = maxHealth;
            hitEffect.transform.SetParent(transform);
            hitEffect.transform.localPosition = Vector3.zero;
        }

        public void Init(HeroController player)
        {
            heroController = player;
            playerTransform = player.transform;
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
}