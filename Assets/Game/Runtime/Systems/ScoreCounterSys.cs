using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class ScoreCounterSys : IEcsInitSystem , IEcsRunSystem
    {
        private EcsCustomInject<SceneService> sceneService;
        private CounterView counterView;
        private float timer;
        private int count;

        public void Init (IEcsSystems systems) 
        {
            counterView = sceneService.Value.counterView;
            counterView.SetText("0");
        }

        public void Run(IEcsSystems systems)
        {
            if ((timer+=Time.deltaTime)<1) { return; }
            timer = 0;
            count++;
            counterView.SetText(count.ToString());
        }
    }
}