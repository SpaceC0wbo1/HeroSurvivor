using UnityEngine;

namespace HeroSurvivor.Gameplay.Animation
{
    public class HeroAnimatorView : MonoBehaviour, IMovementAnimator
    {
        [SerializeField] private Animator _animator;

        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private static readonly int ShootHash = Animator.StringToHash("Shoot");
        private static readonly int DieHash = Animator.StringToHash("Die");

        private void Awake()
        {
            if (_animator == null)
            {
                _animator = GetComponentInChildren<Animator>(true);
            }
        }

        public void SetMovementSpeed(float speed)
        {
            _animator.SetFloat(SpeedHash, speed);
        }

        public void PlayShoot()
        {
            _animator.SetTrigger(ShootHash);
        }

        public float PlayDeath()
        {
            _animator.SetTrigger(DieHash);

            AnimatorClipInfo[] clipInfo = _animator.GetCurrentAnimatorClipInfo(0);
            return clipInfo.Length > 0 ? clipInfo[0].clip.length : 2.0f;
        }
    }
}
