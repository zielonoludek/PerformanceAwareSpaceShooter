# Unity DOTS Project - Performance Aware Space Shooter

This project demonstrates a performant system built with Unity DOTS (Data-Oriented Technology Stack) in Unity 2022.3.40f1. The system includes an efficient spawner system for enemies and projectiles, optimized for parallel processing and ECS (Entity Component System) principles.

## Features

- **Enemy and Projectile Spawner**: The project includes a spawning system that manages enemies and projectiles using DOTS, leveraging `ISystem`, `SystemBase`, and `EntityCommandBuffer` to manage entities efficiently.
- **Burst-Optimized Systems**: Systems like `EnemyMovementSystem`, `ProjectileMoveSystem`, and `PlayerMovementSystem` are burst-compiled, improving performance by compiling code to machine-optimized instructions.
- **Parallel Processing**: Spawning, movement, and input handling are designed to operate in parallel across multiple threads, reducing the CPU load.
- **Event-Driven Spawning**: Enemy and projectile spawning are controlled by timed events, making use of DOTS timers and elapsed time to trigger new entity instantiation.

## Code Overview

### Structure and Components

The project follows the Entity Component System (ECS) pattern, with core systems for spawning, movement, and input. Below is an outline of key scripts and their roles:

- **Spawner**: Defines spawning logic for enemies and projectiles.
- **Enemy Components**: Stores data like position, velocity, and prefab references.
- **ProjectileMoveSystem**: Moves projectiles with burst-compiled, thread-safe logic.
- **PlayerMovementSystem**: Controls player movement within screen bounds, clamping positions to keep entities on-screen.

### Performance Considerations

1. **Burst Compilation**: Critical systems (`EnemyMovementSystem`, `ProjectileMoveSystem`) are burst-compiled to ensure they execute at optimal speed.
2. **EntityCommandBuffer**: Spawning and instantiation use `EntityCommandBuffer` for safe parallel entity creation, reducing overhead from structural changes during gameplay.
3. **Native Collections**: `NativeArray` is used to store random values for enemy spawn locations, ensuring efficient, low-latency randomization in a thread-safe manner.
4. **Parallel ForEach**: Systems use `Entities.ForEach` with `.ScheduleParallel()` for processing entities across multiple threads, which leverages Unity’s Job System to minimize main-thread usage.
