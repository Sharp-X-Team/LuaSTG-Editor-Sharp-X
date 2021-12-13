---
title: Folders
has_children: false
nav_order: 2
parent: General tab
---

# Folders

### ![](https://raw.githubusercontent.com/zinoLath/LuaSTG-Editor-Sharp-X/main/LuaSTGEditorSharp.Core/images/16x16/folder.png) Folder node

Folders are used to organize content inside the editor.

### ![](https://raw.githubusercontent.com/zinoLath/LuaSTG-Editor-Sharp-X/main/LuaSTGEditorSharp.Core/images/16x16/folderred.png) ![](https://raw.githubusercontent.com/zinoLath/LuaSTG-Editor-Sharp-X/main/LuaSTGEditorSharp.Core/images/16x16/foldergreen.png) ![](https://raw.githubusercontent.com/zinoLath/LuaSTG-Editor-Sharp-X/main/LuaSTGEditorSharp.Core/images/16x16/folderblue.png) ![](https://raw.githubusercontent.com/zinoLath/LuaSTG-Editor-Sharp-X/main/LuaSTGEditorSharp.Core/images/16x16/folderyellow.png) Colored folders nodes

| Icon | Folder | Note |
| - | :-: | - |
| <img src="https://raw.githubusercontent.com/RyannThi/LuaSTG-Editor-Sharp-X/main/LuaSTGEditorSharp.Core/images/folderred.png"> | Red folder | Used in the same way as default folder |
| <img src="https://raw.githubusercontent.com/RyannThi/LuaSTG-Editor-Sharp-X/main/LuaSTGEditorSharp.Core/images/foldergreen.png"> | Green folder | Used in the same way as default folder |
| <img src="https://raw.githubusercontent.com/RyannThi/LuaSTG-Editor-Sharp-X/main/LuaSTGEditorSharp.Core/images/folderblue.png"> | Blue folder | Used in the same way as default folder |
| <img src="https://raw.githubusercontent.com/RyannThi/LuaSTG-Editor-Sharp-X/main/LuaSTGEditorSharp.Core/images/folderyellow.png"> | Yellow folder | Used in the same way as default folder |

Colored folders have the same attributes as normal, "uncolored", folders. They're present solely for the purpose of organization.

## Usage and in-game result

![](https://github.com/RyannThi/LuaSTG-Editor-Sharp-X/blob/site/img/Folder_example.png?raw=true)

There is no visible behavior inside the game that directly relates to the node function itself.

## Lua Output

![](https://github.com/RyannThi/LuaSTG-Editor-Sharp-X/blob/site/img/Folder_output.png?raw=true)

The folder node doesn't have any output by default, however, if it has nodes inside it that do have an output, they get aggrouped and the folder's output becomes the combination of all the outputs of the nodes inside the folder. Example:

![](https://github.com/RyannThi/LuaSTG-Editor-Sharp-X/blob/site/img/Folder_output_2.png?raw=true)

## Parents

The folder node doesn't have any strict parenting rules.
