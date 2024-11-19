using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct PlayerMovementSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        Camera mainCamera = Camera.main;
        float deltaTime = SystemAPI.Time.DeltaTime;

        if (mainCamera != null)
        {
            float3 minBounds = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0));
            float3 maxBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

            foreach (var (transform, input, speed) in SystemAPI.Query<RefRW<LocalTransform>, PlayerMoveInput, PlayerMoveSpeed>())
            {
                float2 newPosition = transform.ValueRW.Position.xy + input.Value * speed.Value * deltaTime;
                newPosition = math.clamp(newPosition, minBounds.xy, maxBounds.xy);
                transform.ValueRW.Position.xy = newPosition;
            }
        }
    }
}
