using Assets.Game.Runtime.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using NUnit;
using UnityEngine;

namespace Client {
    sealed class EnemiesSys : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorldInject _world;
        private EcsCustomInject<SceneService> _sceneService;
        private EcsPoolInject<UnitComponent> _unitCmpPool;

        private EcsPoolInject<EnemyLifetimeCmp> lifetimePool;
        private EcsFilterInject<Inc<EnemyLifetimeCmp>> lifeTimeFilter;

        private EcsCustomInject<Debugger> debug;


        private float _spawnInterval;
        private Camera _camera;

        public void Init(IEcsSystems systems)
        {
            _spawnInterval = _sceneService.Value.EnemySpawnInterval;
            _camera = _sceneService.Value.Camera;
            debug.Value.DebugLog("EnemiesSys Init");
        }

        public void Run(IEcsSystems systems)
        {
            CreateEnemy();
            CheckLifetime();
        }

        private void CheckLifetime()
        {
            //debug.Value.DebugLog("CheckLifetime ...");
            foreach (var ent in lifeTimeFilter.Value)
            {
                ref var lifetimeCmp = ref lifetimePool.Value.Get(ent);
                lifetimeCmp.value -= Time.deltaTime;
                if (lifetimeCmp.value>0)
                {
                    continue;
                }
                //debug.Value.DebugLog("LifetimeCmp.value <0 on " + ent);
                var unitCmp = _unitCmpPool.Value.Get(ent);
                _sceneService.Value.ReleaseEnemy(unitCmp.View);
                _world.Value.DelEntity(ent);
            }
        }

        private void CreateEnemy()
        {
            //Заходим в тело метода по таймеру
            if ((_spawnInterval -= Time.deltaTime) > 0)
                return;

            _spawnInterval = _sceneService.Value.EnemySpawnInterval;

            //Создаём View 
            var enemyView = _sceneService.Value.GetEnemy();
            var enemyPosition = GetOutOfScreenPosition();
            enemyView.SetPosition(enemyPosition);
            enemyView.RotateTo(_sceneService.Value.PlayerView.transform.position);

            //Создаём сущность и прокидываем данные
            var enemyEntity = _world.Value.NewEntity();
            ref var unitCmp = ref _unitCmpPool.Value.Add(enemyEntity);
            unitCmp.View = Random.Range(0,2) >0? enemyView: enemyView;
            unitCmp.Velocity = Vector3.up * _sceneService.Value.EnemyMoveSpeed;

            //Добавляем компонент EnemyLifetime и задаем value
            ref var lifetimeCmp = ref lifetimePool.Value.Add(enemyEntity);
            lifetimeCmp.value = 3f;
        }

        private Vector3 GetOutOfScreenPosition()
        {
            var randomX = Random.Range(-1000, 1000);
            var randomY = Random.Range(-1000, 1000);
            var randomPosition = new Vector3(randomX, randomY);
            var randomDirection = (_camera.transform.position - randomPosition).normalized;
            var cameraHeight = _camera.orthographicSize * 2;
            var cameraWith = cameraHeight * _camera.aspect;
            return new Vector3(randomDirection.x * cameraHeight, randomDirection.y * cameraWith);
        }
    }
}