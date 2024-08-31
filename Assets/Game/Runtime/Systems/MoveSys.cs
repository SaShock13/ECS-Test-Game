using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class MoveSys : IEcsRunSystem {
        private EcsPoolInject<UnitComponent> unitCmpPool;
        private EcsFilterInject<Inc<UnitComponent>> unitCmpFilter;

        public void Run(IEcsSystems systems)
        {
            //Бежим по всем сущностям с UnitCmp
            foreach (var entity in unitCmpFilter.Value)
            {
                //Получаем текущую скорость и отображение
                var unitCmp = unitCmpPool.Value.Get(entity);
                var velocity = unitCmp.Velocity;
                var view = unitCmp.View;

                view.UpdateAnimationState(velocity);

                if (velocity == Vector3.zero)
                {
                    continue;
                }
                // Двигаем отображение 
                view.SetDirection(velocity);
                view.Move(velocity * Time.deltaTime);
            }
                
            
        }
    }
}