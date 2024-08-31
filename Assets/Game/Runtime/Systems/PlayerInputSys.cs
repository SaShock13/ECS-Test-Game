using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Xml.Schema;
using UnityEngine;

namespace Client 
{
    sealed class PlayerInputSys : IEcsInitSystem , IEcsRunSystem
    {
        private EcsWorldInject myWorld;
        private EcsPoolInject<UnitComponent> unitCmpPool;
        private EcsPoolInject<PlayerTag> playerTagPool;

        private EcsCustomInject<Debugger> debugger;
        private EcsCustomInject<SceneService> sceneData;

        private int playerEntity;
        public void Init (IEcsSystems systems) 
        {

            //������������ debugger
            var moveSpeed = sceneData.Value.PlayerMoveSpeed;
            debugger.Value.DebugLog("PlayerInputSys Initialization!!! MoveSpeed = " + moveSpeed );

            //������ �������� ������
            playerEntity = myWorld.Value.NewEntity();

            //��������� PlayerTag � UnitCmp �� �������� ������
            playerTagPool.Value.Add( playerEntity );
            ref var playerCmp = ref unitCmpPool.Value.Add(playerEntity);

            //����������� UnitView � UnitCmp
            playerCmp.View = sceneData.Value.PlayerView;
        }

        public void Run(IEcsSystems systems)
        {
            //������� �������� ������
            var moveSpeed = sceneData.Value.PlayerMoveSpeed;
            var x = Input.GetAxisRaw("Horizontal");
            var y = Input.GetAxisRaw("Vertical");
            var direction = new Vector2 (x, y).normalized;
            var velocity = direction * moveSpeed;

            // ��������� ��� �� �������� ������ ����� ���� UnitCmp
            if (unitCmpPool.Value.Has(playerEntity))
            {
                // ������������� ��������
                ref var playerCmp = ref unitCmpPool.Value.Get(playerEntity);
                playerCmp.Velocity = velocity;
            }

        }
    }
}