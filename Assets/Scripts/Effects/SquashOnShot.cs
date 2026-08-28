using UnityEngine;
using PrimeTween;
using HeroSurvivor.Gameplay.Combat;

namespace HeroSurvivor.GameFeel
{
    public class SquashOnShot : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private Shooter _shooter;
        [SerializeField] private Transform _target;

        [Header("Squash")]
        [SerializeField] private bool _enabled;
        [SerializeField] private Vector3 _squashScale = new Vector3(1f, 1.3f, 1f);
        [SerializeField] private float _returnDuration = 0.15f;

        private void OnEnable() => _shooter.Shot += OnShot;

        private void OnDisable() => _shooter.Shot -= OnShot;

        private void OnShot()
        {
            if (_enabled == false)
                return;

            Tween.StopAll(onTarget:  _target);
            _target.localScale = _squashScale;
            Tween.LocalScale(_target, Vector3.one, _returnDuration);
        }

    }
}
