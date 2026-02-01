# Halves Of Tria

## Libraries Used

- [MonoGame.Extended](https://www.monogameextended.net/)

## Roadmap
This roadmap serves as initial guidelines and progress tracking for this project. As such, it may change over time. Also, each stage will gain more details as the project progresses, outlining the stage's key features and goals.

### Stage Summaries

| #   | Name                                              | Status      | Date Completed |
| --- | ------------------------------------------------- | ----------- | -------------- |
| 1   | Technical Setup                                   | In Progress |                |
| 2   | Primary Player Character (Salt)                   | Not Started |                |
| 3   | Secondary Player Character (Sulphur)              | Not Started |                |
| 4   | Environment                                       | Not Started |                |
| 5   | Capsule Colliders and Procedural Animation Basics | Not Started |                |
| 6   | Items and Interactables                           | Not Started |                |
| 7   | Health, Death, and Respawning                     | Not Started |                |
| 8   | First Creatures                                   | Not Started |                |
| 9   | Graphical User Interface                          | Not Started |                |
| 10  | Room Connectivity and Editor                      | Not Started |                |
| 11  | Simple Environmental Pixel Art                    | Not Started |                |
| 12  | Improved Pixel Art Procedural <br>Animation       | Not Started |                |
| 13  | Container Item with Inventory System              | Not Started |                |
| 14  | Transmutation System                              | Not Started |                |
| 15  | Persistent Data Storage and Save Points           | Not Started |                |
| 16  | Creature Types                                    | Not Started |                |
| 17  | Ecosystem Simulation                              | Not Started |                |
| 18  | Day-Night Cycle                                   | Not Started |                |
| 19  | NPCs, Dialogue, and Dynamic Relationships         | Not Started |                |
| 20  | Regions and The Map                               | Not Started |                |
| 21  | Basic Audio                                       | Not Started |                |
| 22  | Player Character Abilities                        | Not Started |                |
| 23  | First Narrative Spine                             | Not Started |                |
| 24  | Polish                                            | Not Started |                |
| 25  | Fleshed Out Band                                  | Not Started |                |

### Stage 1 - Technical Setup

#### End Goal

A good foundation from which to code the rest of the game, including an Entity Component system; input handling for keyboard and mouse, as well as gamepads; the ability to read and write variables to and from JSON files (for convenience while tweaking values while the game is running); and a way to use unit tests to test methods.

#### To Do
- Start "JSON Reading and Writing to Variables" feature.

#### Key Features

| Name                                  | End Goal                                                                                                                                                                         | Status      | Date Completed |
| ------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ----------- | -------------- |
| Entity Component System               | A well-structured object-oriented code structure, with entities, entity components, and systems that act on all entities with components they govern. Each entity should only contain components, not other code (if other code is needed, make a component instead). Each system should be what runs most of the code.                                                                                                                                       | Completed   | 01/02/2026     |
| Input Handling                        | Input handling for keyboard and mouse and gamepads, with methods for key pressing/clicking,  holding, and releasing, and the ability to map each input to a labelled action.     | Completed   | 29/01/2026     |
| JSON Reading and Writing to Variables | Some abstract class containing global variables, with methods that allow someone to read and write directly to JSON.                                                             | Not Started |                |
| Tweak JSON During Runtime (Debug)     | During runtime, it's possible to, with a keypress, it's possible to search for a variable (in the above global abstract class) and, if it's found, change its value. The input will need to be parsed from a string to the type of the variable.                                                                                                                                                                                                                  | Not Started |                |
| Testing                               | A way for code to be tested while debugging (ideally, tests won't run when outside of debugging).                                                                                | Not Started |                |

### Stage 2 - Primary Player Character (Salt)

#### End Goal

A controllable character which can move around in a basic environment with rudimentary environmental collisions (with a rectangular room boundary). Their physics should use Verlet integration, with movement mechanics based on physics, affected by gravity and air resistance. The rendering and simulation timesteps should be decoupled.

#### Key Features

| Name                                         | End Goal                                                                                                                                                                         | Status      | Date Completed |
| -------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ----------- | -------------- |
| Verlet Integration on Physics Objects        | Physics objects with their velocity, acceleration, and position calculated based on the resultant force and resultant impulse acting upon them. The simulation accounts for delta time, despite the static timestep, to allow for the simulation timestep to be manually changed (changing this could be an option in performance settings).                                                                                                                        | Not Started |                |
| External Forces                              | External forces (such as the forces due to gravity, air resistance, or surface friction) which act on all physics objects with reasonable realism.                               | Not Started |                |
| Labelled Visible Forces (Debug)              | A visual to represent the forces acting on a given physics object. These forces are represented by arrows, with length proportional to the force's magnitude, and pointing in the direction of the force. The arrows have text with the name of each force, as well as a numerical value indicating it's magnitude. This visual should be toggleable during runtime.                                                                                             | Not Started |                |
| Controllable Character Movement              | A player character that can walk left and right, jump, and wall jump, all depending on player inputs. Such movement applies forces or impulses to the character, rather than altering their velocity or position directly.                                                                                                                                                                                                                                           | Not Started |                |
| Basic Environment and Collisions             | A rectangular box which can collide with the player character to keep them on screen at all times. Ideally, it should be impossible for two collideable entities to occupy the same space (i.e. overlap), so "position fudging" might be required.                                                                                                                                                                                                                | Not Started |                |
| Decoupled Rendering and Simulation Timesteps | Entities are rendered at a dynamic timestep, low enough to be smooth, but high enough to not experience any hiccups. They are simulated at a fixed timestep, low enough to be smooth, but high enough to give the game time to load.                                                                                                                                                                                                                             | Not Started |                |









