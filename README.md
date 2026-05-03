# Asteroids

Personal remake built to validate my architecture and automated workflows in a controlled environment.

![Screenshot](https://github.com/alejandrodeveloper7/UnityAsteroids/blob/main/Assets/Asteroids/Art/Screens/Screen1.png)

# Core Architecture & Scalability

**- Decoupled Systems**: Interface-driven design, **Zenject** for DI and a custom **EventBus**.

**- Data-Driven Gameplay**: Ship, stats, asteroids, and balance managed via **ScriptableObjects** for instant iteration.

**- MVC UI System**: Robust framework split into additive scenes, ensuring a clean separation between logic and view.
​​

# Engineering & Workflow

**- Automated Tooling**: Custom Unity editors to generate Managers and MVC layers, focusing on **development velocity**.

**- Scalable Codebase**: Designed to support growing complexity without technical debt, mirroring professional production standards.
​

# Performance Optimization

**- Memory Management**: Full **Object Pooling** for VFX, audio, and gameplay entities to eliminate GC spikes.

**- Rendering**: Optimized draw calls through **Sprite Atlases** and efficient culling.
