# Asteroids

Personal remake of the classic arcade game **“Asteroids”** for Windows. The project recreates the gameplay with enhanced visuals and several new features, while preserving the essence and feel of the original title.

![Screenshot](https://github.com/alejandrodeveloper7/UnityAsteroids/blob/main/Assets/Art/Screens/Screen1.png)

This version includes the following additions and changes:

 - **Ship and bullet selection**, each with its own visuals and **stats** to adapt to different playstyles.
 - Asteroids have **health points** that vary based on their size.
 - Bullets apply **force and torque** to asteroids, allowing you to alter their trajectory when shooting them.
 - Instead of traditional lives, each ship has a limited **number of hits it can take** before being destroyed.
 - The ships have a **shield** that blocks one hit and **regenerates after a cooldown** once it’s gone.
 - **Online Leaderboard** to record each player’s score.
 - **Pause** menu with several configurable **settings**.

# ​​​​​​General Architecture
 - The architecture is fully **modular**, **decoupled**, and **scalable**, with a strong **data-driven** approach, strong **encapsulation of responsibilities**, **contract-based** communication, **component-based** structure, **additive scenes** and **event-based communications**.
 - Gameplay and other systems communicate internally using **native C# events**, while Services and Managers dispatch events to notify important occurrences. Communication between systems is handled through my **custom EventBus system**, based in delegates and the interface IEvent.
 - **Zenject** is used for dependency injection, which required slight adaptations of my tools and scripts, as well as the creation of multi-instance Managers and Services versions, allowing Zenject to manage singletons without conflicts instead using the singleton versions.
​​

# Project Structure
 - This project showcases several of my **reusable scripts and tools** applied in a real environment. Only the components actually in use were kept to keep the project clean and focused.
 - All Managers, Services, ScriptableObjects, and API Callers were generated with my **custom editor tools**, ensuring a consistent structure, **interface-based** design, and **contract-driven** functionality. This approach guarantees clarity, scalability, and flexible communication between systems.
 - The ScriptableObjects used for configuration, settings, collections, and data share a common inheritance structure and are integrated through interfaces that allow them to interact with various systems (such as selectors or stats displayers), maintaining a fully **data-driven** workflow.
​

# User Interface (UI)
 - The UI follows an **MVC pattern**, generated with my** MVC Module Creator tool**.
 - Each module includes its own **ScriptableObject for configuration** and is loaded as an **additive scene**.
 - Both the Controller and View are based on interfaces, allowing for a **contract-based**, decoupled design that’s easily mockable for testing.
 - The UI is built around a **prefabs design system** that speeds up development and iteration while ensuring visual consistency across layouts and components.
 - It also uses **adaptive layouts** that automatically resize when content changes and is configured for respond to **different resolutions and aspect ratios**.
 - **DOTween** is used for UI animations, transitions, feedback, and other motion effects, not only in the UI.
 - The project also includes **UI particles**.


# Gameplay and Systems
 - Gameplay elements such as asteroids, the player, and bulets follow a **component-based** architecture, separating functionality into distinct scripts.
 - Gameplay scripts implement **interfaces** like IPushable or IDamageable, along with modular components such as PushOnContact and DamageOnContact, maintaining **modularity and scalability**.
 - The project is fully **data-driven** through ScriptableObjects, allowing you to modify controls, gameplay elements, stage configurations, backgrounds, UI, and more, all directly within the editor.
​

# Art and Optimization
 - Audio and gameplay GameObjects are pooled using my **pooling tool**, optimizing performance and minimizing runtime object creation and destruction.
 - Art assets are organized into **Sprite Atlases** according to their usage, optimizing draw calls and rendering efficiency.
 - The codebase is organized with **regions, headers, and clearly defined sections**, including explanatory comments to make navigation and returning to the project after some time quick and intuitive.
