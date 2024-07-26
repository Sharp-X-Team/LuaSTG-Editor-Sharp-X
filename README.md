<p align="center" width="50%">
    <img width="8%" src="https://cdn.discordapp.com/emojis/871436538087624805.png?v=1">
</p><h1 align="center">LuaSTG Editor Sharp X</h1>
<h4 align="center">

(Current release: [0.76.0](https://github.com/RyannThi/LuaSTG-Editor-Sharp-X/releases))

</h4>

<div align="center">
    
[![](https://dcbadge.limes.pink/api/server/bhM599npvd)](https://discord.gg/bhM599npvd)

</div>

**LuaSTG Editor Sharp X** is a *code generator* for the LuaSTG engine, based on the *THlib* library. It is based on and forked from [**czh098tom**](https://github.com/czh098tom)'s original [**Editor Sharp**](https://github.com/czh098tom/LuaSTG-Editor-Sharp).

<h3>Usage</h3>
Games can be created for LuaSTG without the use of direct lua coding by using the editor. There are multiple nodes that equate to functions and code groups that can be placed within a hierarchy to generate compatible lua output code. This is the main way of producing content within the editor, but direct coding is permitted as well.
<br><br>
<p align="center" width="50%">
    <img width="100%" src="https://github.com/Sharp-X-Team/LuaSTG-Editor-Sharp-X/blob/main/CodeGenerationExample.png">
</p>

<h3>Compatibility</h3>
The editor is designed to fit many variants of the LuaSTG engine. New versions can be added within the source via a plugin system, which can change runtime arguments between versions.
<br>
<br>
<div align="center">
  
ex+ (ExPlus) | -x | aex+ (Sub) | Evo | Others
:---: | :---: | :---: | :---: | :---:
:heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :hourglass: (next release) | As long as THlib is present

</div>

Depending on the THlib you're using, some of the node functions may not be available. Some of these cases are handled by the editor, displaying a warning about the node not working unless your codebase supports it. This warning can be disabled in the settings.

<h3>Features</h3>

As a code generator, the editor can virtually handle any type of code you'd likely do in the usual IDE but with a friendly GUI, **allowing direct coding via code nodes** as well. Here are some creations that can be done natively without extra coding needed, as well as editor features.

<div align="center">
  
Creation | System
:---: | :---: 
Boss fights | Light/Dark editor themes
Stage sections | Automatic engine version detection
Players | Folder/Zip packing
3D Backgrounds | Logging/debug window (Sub only)
Advanced dialog handling | Custom node creation (experimental)
Post-effect and particles | Node marketplace (in development)
Advanced object manipulation | Automatic updates
Game data handling | Discord Rich Presence
Audio control | Auto-saves
Advanced rendering | Project inclusion

</div>

Of course, that's just a fraction of all the things the editor is capable of.

<h3>Interface</h3>
<p align="center" width="50%">
    <img width="100%" src="https://github.com/Sharp-X-Team/LuaSTG-Editor-Sharp-X/blob/main/EditorPreview.png">
</p>

<h3>Importing from Editor Sharp</h3>
Projects are mostly compatible when bringing them from Sharp to Sharp X. There are exceptions, though, which can all be fixed. Some nodes had their parameter list changed, so you'll need to replace those in your project. These errors will be exhibited in the error list at the bottom, of which you can analyze and see what nodes need to be adjusted.
<br>
<br>
For further information:

* Click [HERE](https://github.com/RyannThi/LuaSTG-Editor-Sharp-X/blob/main/LuaSTGEditorSharp/Update%20Log.txt) to see the update log.
