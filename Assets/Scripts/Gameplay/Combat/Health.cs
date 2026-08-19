using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;


namespace HeroSurvivor.Gameplay.Combat
{
    public class Health : MonoBehaviour
    {
        public static event Action<Vector3> AnyDamaged;
        public static event Action<Vector3> AnyKilled;

        public event Action<Vector3> Damaged;
        public event Action<Vector3> Killed;

        [SerializeField] private CharacterConfig _characterConfig;
        [SerializeField] private float _timeToDestroy = 0.08f;

        private int _currentHealth;

        private void Awake()
        {
            _currentHealth = _characterConfig.maxHealth;
        }

        public void TakeDamage(int amount, Vector3 direction)
        {
            if (_currentHealth <= 0)
                return;

            _currentHealth -= amount;
            Debug.Log($"{_characterConfig.name} took {amount} damage. Current HP: {_currentHealth}");

            if (_currentHealth <= 0)
            {
                Killed?.Invoke(direction);
                AnyKilled?.Invoke(direction);

                DestroyAsync(_timeToDestroy, destroyCancellationToken).Forget();
            }
            else
            {
                Damaged?.Invoke(direction);
                AnyDamaged?.Invoke(direction);
            }
        }

        private async UniTaskVoid DestroyAsync (float seconds, CancellationToken cancellationToken)
        {
            await UniTask.WaitForSeconds(seconds, ignoreTimeScale: true, cancellationToken: cancellationToken);
            Destroy(gameObject);
        }
    }
}
