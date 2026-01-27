# Halves Of Tria

## Roadmap

### Stage 1 - Technical Setup

#### End Goal

A good foundation from which to code the rest of the game, including an Entity Component system; decoupled rendering and simulation time steps; input handling for keyboard and mouse, as well as gamepads; the ability for variables to read and write their values to and from JSON files (for convenience while tweaking values while the game is running).

#### Key Features

| Name                                  | End Goal                                                                                                                                                                                                                                                                                                                | Status      | Date Completed |
| ------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ----------- | -------------- |
| Entity Component System               | A well-structured object-oriented code structure, with entities, entity components, and systems that act on all entities with components they govern. Each entity should only contain components, not other code (if other code is needed, make a component instead). Each system should be what runs most of the code. | Not Started |                |
| Dynamic Rendering Timestep            | Entities are rendered at a fixed timestep, low enough to be smooth, but high enough to not experience any hiccups.                                                                                                                                                                                                      | Not Started |                |
| Fixed Simulation Timestep             | Entities are simulated at a fixed timestep, low enough to be smooth, but high enough to give the game time to load.                                                                                                                                                                                                     | Not Started |                |
| Input Handling                        | Input handling for keyboard and mouse and gamepads, with methods for key pressing/clicking, holding, and releasing, and the ability to map each input to a labelled action.                                                                                                                                             | Not Started |                |
| JSON Reading and Writing to Variables | Some abstract class containing global variables, with methods that allow someone to read and write directly to JSON.                                                                                                                                                                                                               | Not Started |                |
| Tweak JSON During Runtime (Debug)     | During runtime, it's possible to, with a keypress, it's possible to search for a variable (in the above global abstract class) and, if it's found, change its value. The input will need to be parsed from a string to the type of the variable                                                                         | Not Started |                |
