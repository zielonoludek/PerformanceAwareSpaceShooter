using Unity.Entities;
using UnityEngine;

namespace Enemy.EnemySpawner
{
    public class EnemySpawnerAuthoring : MonoBehaviour
    {
        public GameObject EnemyPrefab;
        public float SpawnRate;
        public float SpawnHeight;

        private class EnemySpawnerAuthoringBaker : Baker<EnemySpawnerAuthoring>
        {
            public override void Bake(EnemySpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new EnemyComponents
                {
                    EnemyPrefab = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic),
                    SpawnYPosition = authoring.SpawnHeight,
                    SpawnRate = authoring.SpawnRate,
                });
                
                if (authoring.EnemyPrefab == null)
                {
                    Debug.LogError("Enemy prefab is null");
                }
            }
        }
    }
}