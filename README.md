# Halves Of Tria

## Libraries Used

- [MonoGame.Extended](https://www.monogameextended.net/)

## Roadmap
(Note: This roadmap will expand as the project progresses.)

### Stage Summaries

| Name | End Goal                                          | Status      | Date Completed |
| ---- | ------------------------------------------------- | ----------- | -------------- |
| 1    | Technical Setup                                   | In Progress |                |
| 2    | Primary Player Character (Salt)                   | Not Started |                |
| 3    | Secondary Player Character (Sulphur)              | Not Started |                |
| 4    | Environment                                       | Not Started |                |
| 5    | Capsule Colliders and Procedural Animation Basics | Not Started |                |
| 6    | Items and Interactables                           | Not Started |                |
| 7    | Health, Death, and Respawning                     | Not Started |                |
| 8    | First Creatures                                   | Not Started |                |
| 9    | Graphical User Interface                          | Not Started |                |
| 10   | Room Connectivity and Editor                      | Not Started |                |
| 11   | Simple Environmental Pixel Art                    | Not Started |                |
| 12   | Improved Pixel Art Procedural <br>Animation       | Not Started |                |
| 13   | Container Item with Inventory System              | Not Started |                |
| 14   | Transmutation System                              | Not Started |                |
| 15   | Persistent Data Storage and Save Points           | Not Started |                |
| 16   | Creature Types                                    | Not Started |                |
| 17   | Ecosystem Simulation                              | Not Started |                |
| 18   | Day-Night Cycle                                   | Not Started |                |
| 19   | NPCs, Dialogue, and Dynamic Relationships         | Not Started |                |
| 20   | Regions and The Map                               | Not Started |                |
| 21   | Basic Audio                                       | Not Started |                |
| 22   | Player Character Abilities                        | Not Started |                |
| 23   | First Narrative Spine                             | Not Started |                |
| 24   | Polish                                            | Not Started |                |
| 25   | Fleshed Out Band                                  | Not Started |                |

### Stage 1 - Technical Setup

#### End Goal

A good foundation from which to code the rest of the game, including an Entity Component system; input handling for keyboard and mouse, as well as gamepads; and the ability for variables to read and write their values to and from JSON files (for convenience while tweaking values while the game is running).

#### To Do
- Add The following functions:
  - WasActionHeld
  - WasActionJustPressed
  - WasActionJustReleased

#### Key Features

| Name                                  | End Goal                                                                                                                                                                         | Status      | Date Completed |
| ------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ----------- | -------------- |
| Entity Component System               | A well-structured object-oriented code structure, with entities, entity components, and systems that act on all entities with components they govern. Each entity should only contain components, not other code (if other code is needed, make a component instead). Each system should be what runs most of the code.                                                                                                                                               | Completed   | 27/01/2025     |
| Input Handling                        | Input handling for keyboard and mouse and gamepads, with methods for key pressing/clicking,  holding, and releasing, and the ability to map each input to a labelled action.     | In Progress |                |
| JSON Reading and Writing to Variables | Some abstract class containing global variables, with methods that allow someone to read and write directly to JSON.                                                             | Not Started |                |
| Tweak JSON During Runtime (Debug)     | During runtime, it's possible to, with a keypress, it's possible to search for a variable (in the above global abstract class) and, if it's found, change its value. The input will need to be parsed from a string to the type of the variable.                                                                                                                                                                                                                  | Not Started |                |
| Testing                               | A way for code to be tested while debugging (ideally, tests won't run when outside of debugging).                                                                                | Not Started |                |
