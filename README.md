Translation of [Kaiakairos](https://github.com/kaiakairos)' [PNGTuber+](https://kaiakairos.itch.io/pngtuber-plus) Godot application from Godot's native GDScript to C#.

## TODOs for this project:
- Translate all GDScript files within the project to C#.  This will include, but is not limited to:
    - Explicitly typing all properties and variables within the code (no use of `var` for any property declaration)
    - Using [Microsoft's C# naming rules](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names)
    - Implementing any dependencies (through Nuget packages) that could be beneficial to the application 
- Rework some of the methods to better flow within C#
- Extend functionality through the use of WebSocket
    - Receive simple "commands" that change application parameters or states.  For example, receiving a command to change costume and change gravity value
    - This will allow direct interaction through stream applications such as [Streamer.bot](https://streamer.bot/) or [SAMMI](https://sammi.solutions/)

20240311: Project archived due to starting another project similar in nature.
