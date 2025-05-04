# Toolkit
The files I dump into every new personal project I make for free infrastructure points
This isn't clean enough for random people to use yet really but I made it public anyway because why not I guess?
Most of this was originally written for an ongoing personal project with https://logicallychaotic.itch.io/

# How to use
1. Put it all in your Assets folder
2. Move all the contents of the `MOVE_INTO_YOUR_SCRIPTS_FOLDER` directory into wherever you keep your other scripts
3. Rename the newly moved `InputManager_YOURGAMENAME` to include your game name
4. Create a GameConfig asset through `Create/Game/Game Config`
5. Import the Unity Input system, and create a new basic configuration with whatever basic inputs you need
6. Make a new GameObject, add to it the components
   - `GameManager`
   - `SceneLoadManager`
   - `InputManager_YOURGAMENAME`
   - `FlowManager`
7. Link up the `SceneLoadManager`, `InputManager_YOURGAMENAME`, `FlowManager`, and `GameConfig` to the `GameManager`'s parameters
8. Save the `GameManager` as a prefab in a new folder `Assets/Config`
