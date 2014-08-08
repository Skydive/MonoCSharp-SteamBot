# MonoCSharp-SteamBot
A work in development, to say the least.

Compile using MonoDevelop.
Commands and chat handlers are controlled with IronPython. 
(Because writing them and compiling them in the program doesn't make it very modular).
IronPython also supports C# dlls. (Look at CleverBot)

Setup:
1) Remove #define EXTERNAL_LOGIN in MainClass.cs
2) Create a new steam account without steam guard
3) Use this.

### TODO (Non-exhaustive list):
- SteamBot TODOs:

  - FriendContainer.cs: Line 51: Somehow fix SendTable()

  - PythonChatHandler.cs: Line 7: Create a better method of chat handler creation. (Involve Classes in IronPython?)


### Changelog
- 8-8-14:
  - Prepared for release
  - Released
  
