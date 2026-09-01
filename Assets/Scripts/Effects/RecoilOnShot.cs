using UnityEngine;
using HeroSurvivor.Gameplay.Combat;
using PrimeTween;

namespace HeroSurvivor.Effects
{
    public class RecoilOnShot : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private Shooter _shooter;
        [SerializeField] private Transform _weapon;

        [Header("Recoil")]
        [SerializeField] private bool _enabled;
        [SerializeField] private Vector3 _kickOffset = new Vector3 (0, 0, -0.3f);
        [SerializeField] private float _returnDuration = 0.15f;

        private Vector3 _basePosition;

        private void Awake()
        {
            _basePosition = _weapon.localPosition;
        }

        private void OnEnable() => _shooter.Shot += OnShot;

        private void OnDisable() => _shooter.Shot -= OnShot;

        private void OnShot()
        {
            if (_enabled == false)
                return;

            Tween.StopAll(onTarget: _weapon);
            _weapon.localPosition = _basePosition + _kickOffset;
            Tween.LocalPosition(_weapon, _basePosition, _returnDuration);
        }

    }
}
