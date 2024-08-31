using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class EcsStartup : MonoBehaviour {
        EcsWorld _world;        
        IEcsSystems _systems;
        [SerializeField] private SceneService sceneData;
        [SerializeField] private Debugger debugger;

        void Start () {
            _world = new EcsWorld ();
            _systems = new EcsSystems (_world);
            _systems
                // register your systems here, for example:
                // .Add (new TestSystem1 ())
                // .Add (new TestSystem2 ())

                // register additional worlds here, for example:
                // .AddWorld (new EcsWorld (), "events")
                .Add(new PlayerInputSys())
                .Add(new EnemiesSys())
                .Add(new MoveSys())
                .Add(new ScoreCounterSys())
                
#if UNITY_EDITOR
                // add debug systems for custom worlds here, for example:
                // .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif

                .Inject(sceneData)
                .Inject(debugger)
                .Init ();
        }

        void Update () {
            // process systems here.
            _systems?.Run ();
        }

        void OnDestroy () {
            if (_systems != null) {
                // list of custom worlds will be cleared
                // during IEcsSystems.Destroy(). so, you
                // need to save it here if you need.
                _systems.Destroy ();
                _systems = null;
            }
            
            // cleanup custom worlds here.
            
            // cleanup default world.
            if (_world != null) {
                _world.Destroy ();
                _world = null;
            }
        }
    }
}