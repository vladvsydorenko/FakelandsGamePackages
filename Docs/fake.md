# Xyz.Vasd.FakeFramework

Fake Framework
Set of packages forming a framework for simple game loop.

The idea is to treat game objects hierarchy like context.

- Context
  - Data (injected)
  - Component (using injected data from parent context, set data for children context)
    - Context
      - Data (injected)
      - Component (using data)
        - ...

In this way, game structure may look like:

- Context
  - Systems
  - Preloader
    - Context
      - Systems
  - Loader
    - Context
      - Systems
  - Game
    - Context
      - Systems
      - Menu World
        - Context
          - Systems
          - Menu View
          - ...

# Set/Get Data
To set or get data first parent's context will be used.
As context is just a monobeahaviour it could be referenced as any property.