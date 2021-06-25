public class MemberInfo {
  public int ID; // the ID assigned to this used (by the server)
  public uint Flags;
  public string Username, DisplayName; // must be provided to enter
  public object Data; // app-specific stuff about this member
  
  public object InternalData;
  // stuff used by lobby classes
 }
 
 public struct MemberFlags {
  // Client flags: low word
  
  // Server flags: high word
  public const uint ServerControlled= 0xFFFF0000;
 }
 
 public class GameInfo {
  public int ID, CreatorID, MaxPlayers;
  public string Name;
  public String GameType, Version;
  // Reserved flags: 1 locked, 2 closed, 4 in progress
  public uint Flags;
  public int[] Players;
  public uint[] PlayerFlags;
  public String Password;
  public object Data;
  public bool Serverside;
  public IServersideGame Game;
 }
 
 public struct GameFlags {
  // Requires a password to enter
  public const uint Locked    = 0x00000001;
  // No-one can enter
  public const uint Closed    = 0x00000002;
  public const uint InProgress= 0x00000004;
 }
 
 public struct PlayerGameFlags {
  public const uint Ready            = 0x00000001; 
       // Player is content to have the game start
}

