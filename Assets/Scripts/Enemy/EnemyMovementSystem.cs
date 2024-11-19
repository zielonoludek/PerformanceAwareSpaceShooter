using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Enemy
{
    [BurstCompile]
    public partial class EnemyMovementSystem : SystemBase
    {
        [BurstCompile]
        protected override void OnUpdate()
        {
            float deltaTime = UnityEngine.Time.deltaTime;

            Entities.ForEach((ref LocalTransform transform, in EnemyComponents movement) =>
            {
                transform.Position += movement.Velocity * deltaTime;
            }).ScheduleParallel();
        }
    }
}
