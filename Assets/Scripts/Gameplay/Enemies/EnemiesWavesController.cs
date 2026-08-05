namespace HeroSurvivor.Gameplay.Enemies
{
    using UnityEngine;
    using System;
    using System.Collections;

    public class EnemiesWavesController : IDisposable
    {
        private readonly EnemySpawner _spawner;
        private readonly MonoBehaviour _coroutineRunner;
        private readonly float _timeBetweenWaves;

        private Coroutine _timerCoroutine;
        private bool _isDisposed;

        public int CurrentWave {  get; private set; }
        public bool isRunning { get; private set; }

       public EnemiesWavesController(EnemySpawner spawner, MonoBehaviour coroutineRunner, float timeBetweenWaves)
        { 
            _spawner = spawner ?? throw new ArgumentNullException(nameof(spawner));
            _coroutineRunner = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));
            _timeBetweenWaves = timeBetweenWaves;
        }

        public void StartWaves()
        {
            if (isRunning || _isDisposed) return;

            isRunning = true;
            _timerCoroutine = _coroutineRunner.StartCoroutine(WavesLoopRoutine());
        }

        public void StopWaves()
        {
            if (!isRunning) return;

            if (_timerCoroutine != null)
            {
                _coroutineRunner.StopCoroutine(_timerCoroutine);
                _timerCoroutine = null;
            }

            isRunning = false;
        }

        private IEnumerator WavesLoopRoutine() 
        {
            var WaveInterval = new WaitForSeconds(_timeBetweenWaves);

            while (isRunning)
            {
                CurrentWave++;
                Debug.Log($"--- Starting Wave {CurrentWave} ---");
                yield return _spawner.SpawnWave();
                Debug.Log($"Wave {CurrentWave} finished spawning. Next wave in {_timeBetweenWaves} seconds...");
                yield return WaveInterval;
            }
        }

        public void Dispose()
        {
            if (_isDisposed) return;

            StopWaves();
            _isDisposed = true;
            Debug.Log("EnemiesWavesController disposed.");
        }
    }
}
