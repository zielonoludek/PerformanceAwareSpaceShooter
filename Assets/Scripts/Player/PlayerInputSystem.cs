using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
public partial class PlayerInputSystem : SystemBase
{
    private Entity Player;

    protected override void OnCreate()
    {
        RequireForUpdate<PlayerTag>();
        RequireForUpdate<PlayerMoveInput>();
    }

    protected override void OnStartRunning()
    {
        Player = SystemAPI.GetSingletonEntity<PlayerTag>();
    }

    protected override void OnUpdate()
    {
        if (!SystemAPI.Exists(Player)) return;
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 moveInput = new Vector2(moveHorizontal, moveVertical);

        SystemAPI.SetSingleton(new PlayerMoveInput { Value = moveInput });

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SystemAPI.SetComponentEnabled<FireProjectileTag>(Player, true);
        }
        else SystemAPI.SetComponentEnabled<FireProjectileTag>(Player, false);
    }

    protected override void OnStopRunning()
    {
        Player = Entity.Null;
    }
}
