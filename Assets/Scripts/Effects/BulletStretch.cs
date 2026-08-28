using UnityEngine;
using PrimeTween;

namespace HeroSurvivor.GameFeel
{
    public class BulletStretch : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private Transform _visual;

        [Header("Stretch")]
        [SerializeField] private bool _enabled;
        [SerializeField] private Vector3 _spwanScale = new Vector3(1f, 1f, 2f);
        [SerializeField] private float _returnDuration = 0.1f;
        
        void OnEnable()
        {
            Vector3 _currentScale = transform.localScale;
            if (_enabled == false) 
                return;

            _visual.localScale = _spwanScale;
            Tween.LocalScale(_visual, _currentScale, _returnDuration);

        }
    }
}
