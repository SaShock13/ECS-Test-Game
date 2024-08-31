using UnityEngine;
using UnityEngine.Pool;

public class SceneService : MonoBehaviour
{
    [field: SerializeField] public UnitView PlayerView { get; private set; }
    [field: SerializeField] public float PlayerMoveSpeed { get; private set; } = 10;
    [field: SerializeField] public UnitView EnemyViewPrefab { get; private set; }
    [field: SerializeField] public UnitView SmallEnemyViewPrefab { get; private set; }
    [field: SerializeField] public Camera Camera { get; private set; }
    [field: SerializeField] public float EnemyMoveSpeed { get; private set; } = 13;
    [field: SerializeField] public float EnemySpawnInterval { get; private set; } = 0.5f;

    [field: SerializeField] public CounterView counterView { get; private set; }



    private ObjectPool<UnitView> _unitsPool;

    private void Awake()
    {
        _unitsPool = new ObjectPool<UnitView>(() => Instantiate(Random.Range(0,2)>0? EnemyViewPrefab: SmallEnemyViewPrefab));
    }

    public UnitView GetEnemy()
    {
        var view = _unitsPool.Get();
        view.SetActive(true);
        return view;
    }

    public void ReleaseEnemy(UnitView view)
    {
        view.SetActive(false);
        _unitsPool.Release(view);
    }
}
