using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using System.Collections.Generic;

public partial class SpawnerSystem : SystemBase
{
    private List<(Entity prefab, float3 position)> spawnQueue;

    protected override void OnCreate()
    {
        spawnQueue = new List<(Entity, float3)>();
    }

    protected override void OnUpdate()
    {
        spawnQueue.Clear();

        foreach (var spawner in SystemAPI.Query<RefRW<Spawner>>())
        {
            if (spawner.ValueRO.NextSpawnTime < SystemAPI.Time.ElapsedTime)
            {
                float3 position = new float3(spawner.ValueRO.SpawnPosition.x, spawner.ValueRO.SpawnPosition.y, 0);
                spawnQueue.Add((spawner.ValueRO.Prefab, position));
                spawner.ValueRW.NextSpawnTime = (float)(SystemAPI.Time.ElapsedTime + spawner.ValueRO.SpawnRate);

            }
        }
        foreach (var (prefab, position) in spawnQueue)
        {
            Entity newEntity = EntityManager.Instantiate(prefab);
            EntityManager.SetComponentData(newEntity, LocalTransform.FromPosition(position));
        }
    }
}
