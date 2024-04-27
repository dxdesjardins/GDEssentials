GDEssentials
=================
Addon to improve handling of events, actions, and memory management for the Godot Game Engine.

Overview
----
The original purpose of this project was to design a system to replace the use of Godot Signals in the Editor Interface. Signals seem to suffer from scalability and performance issues partially due to their high reliance on string references. In the end, this addon has become a collection of tools that I import to all Godot Projects.

Features
----
1. A GameEvent & GameAction System designed to completely replace the need to use Signal Connections through the Editor Interface.

2. A Resource Event System implementation.

3. An Object Pooling System. See me separate GDPool repository for documentation.

4. An improved implementation of the Singleton Pattern.

5. Various Extension Methods and Listeners.

6. And much more. This repository is developing quickly and documentation is currently lacking.

Known Issues
----
This project currently suffers from an infamous engine bug. It has been fixed for the upcoming 4.3 release; I am currently using an unofficial engine patch to mitigate. I recommend first reading the issue report at:
https://github.com/godotengine/godot/issues/79519

Licence
---
MIT
