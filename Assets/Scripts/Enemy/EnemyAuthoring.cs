using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Enemy
{
    public class EnemyAuthoring : MonoBehaviour
    {
        public float3 Position;
        public float3 Velocity;
        public GameObject EnemyPrefab;
        
        private class EnemyAuthoringBaker : Baker<EnemyAuthoring>
        {
            public override void Bake(EnemyAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new EnemyComponents
                {
                    Position = authoring.Position,
                    Velocity = authoring.Velocity,
                    EnemyPrefab = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
    public struct EnemyComponents : IComponentData
    {
        public float3 Position;
        public float3 Velocity;
        public float SpawnYPosition;
        public float SpawnRate;
        public Entity EnemyPrefab;
    }

}