# Additive Scene Helper
Switch/Open quickly additive scenes (levels, prefabs, UI scenes)

## 1. INTRODUCTION
We work on it different scenes most of time when create games. Same time, We want to get quickly specific prefabs, gameObject etc. when we needed.
At this point, this asset will make our job much easer. <br> By adding this asset to project. You can get...

•	Switch quickly between scenes in the same folder <br>
•	Easy access to specific scenes (prefabs, UI scenes) <br>
•	In runtime, Jumping between levels in a continuous loop (Actually, this asset is editor tool. But use to jump between levels simply) <br>


## 2. HOW TO USE

You wish, You can watch how to use tool on youtube : https://www.youtube.com/watch?v=AdGRB_8aJrQ

<img src="Assets/Additive Scene Helper/Images/Screenshot 00.jpg" >

<img src="Assets/Additive Scene Helper/Images/Screenshot 01.png" >

It is not enough to collect our scenes under certain folder to use this tool. In addition, it is necessary to make assignments to the Editor Build Setting and our LevelManager asset. In this way, we can intervene in the order of our scenes through our LevelManager asset.

<img src="Assets/Additive Scene Helper/Images/Screenshot 02.png">

#### 2.1. Remove All Additive Scene Button: 
Remove all additive scene in hierarchy
#### 2.2. "R" Button: 
Remove additive scene (scene which belongs to the slider) in hierarchy
#### 2.3. SceneInfo -> Name
For information on which slider belogs to which scene. It is also used it for the Playerprefs key.
#### 2.4. SceneInfo -> FolderPath
The folder path where the scenes are located.
#### 2.5. Shortcuts (Runtime)
"P: Pause | N: Jump next level | R: Restart"
#### 2.6. Shortcuts (Editor)
Alt + A	=> Quickly open/close LevelManager <br>
Ctrl + Shift + B =>	Build Settings <br>
Alt+Ctrl+C => Copy folder path <br>

