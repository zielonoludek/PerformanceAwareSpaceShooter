using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

namespace Enemy.EnemySpawner
{
    public partial struct EnemySpawnSystem : ISystem
    {
        private NativeArray<Unity.Mathematics.Random> RandomArray;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            RandomArray = new NativeArray<Unity.Mathematics.Random>(JobsUtility.MaxJobThreadCount, Allocator.Persistent);
            uint seed = (uint)System.Environment.TickCount;
            for (int i = 0; i < RandomArray.Length; i++)
            {
                RandomArray[i] = new Unity.Mathematics.Random(seed == 0 ? 1 : seed);
                seed++;
            }
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            double elapsedTime = SystemAPI.Time.ElapsedTime;
            float deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (enemyComponent, entity) in SystemAPI.Query<EnemyComponents>().WithEntityAccess())
            {
                if (elapsedTime % enemyComponent.SpawnRate < deltaTime)
                {
                    int threadIndex = JobsUtility.ThreadIndex;
                    var random = RandomArray[threadIndex];
                    float randomX = random.NextFloat(-8f, 8f);
                    float randomScale = random.NextFloat(2f, 10f);

                    RandomArray[threadIndex] = random;

                    Entity newEnemy = ecb.Instantiate(enemyComponent.EnemyPrefab);
                    ecb.SetComponent(newEnemy, LocalTransform.FromPositionRotationScale(
                        new float3(randomX, enemyComponent.SpawnYPosition, 0), 
                        quaternion.identity,
                        randomScale));
                }
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }

        public void OnDestroy(ref SystemState state)
        {
            if (RandomArray.IsCreated)
                RandomArray.Dispose();
        }
    }
}