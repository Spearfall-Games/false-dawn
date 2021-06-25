case ReservedCodes.RequestJoinGame:
   // Just pass it on to the game owner
   id = ClientInfo.G
         etInt(b.GetParameter(ref pi).content, 0, 4);
   int reqcode = ClientInfo.GetInt(
     b.GetParameter(ref pi).content, 0, 4);
   String pwd =
     Encoding.UTF8.GetString(b.GetParameter(ref pi).content);
   gi = (GameInfo)games[id];
   if(gi == null){
    caller.SendMessage(ReservedCodes.Error,
      Encoding.UTF8.GetBytes(
          String.Format(Strings.UnknownGame, id)),
          ParameterType.String);
    break;
   }
   // Make sure they're not already in this game!
   bool found = false;
   for(int i = 0; i < gi.Players.Length; i++)
    if(gi.Players[i] == mem.ID){
    caller.SendMessage(ReservedCodes.Error,
           Encoding.UTF8.GetBytes(Strings.AlreadyJoined),
           ParameterType.String);
    found = true;
    break;
   }
   if(found) break;
   if(gi.Players.Length >= gi.MaxPlayers){
    // Automatically send a rejection if the server is full
    caller.SendMessage(ReservedCodes.PlayerResponse,
          PrepareResponse(gi.CreatorID, reqcode, 0,
                          Strings.GameFull), 0);
    break;
   }
   if((gi.Flags & GameFlags.Closed) != 0){
    // Automatically send a rejection if the server is closed
    caller.SendMessage(ReservedCodes.PlayerResponse,
        PrepareResponse(gi.CreatorID, reqcode, 0,
                        Strings.GameClosed), 0);
    break;
   }
   if((gi.Flags & GameFlags.Locked) != 0){
    // Automatically send a rejection
    // if the server is locked and the
    // password was wrong
    if(pwd != gi.Password){
     caller.SendMessage(ReservedCodes.PlayerResponse,
            PrepareResponse(gi.CreatorID, reqcode, 0,
                            Strings.GameLocked), 0);
     break;
    }
   }

   if(gi.Serverside){
    // Server-hosted game. Allow the plugin to decide
    String cjmsg;
    if(gi.Game.CanJoin(mem.ID, reqcode, pwd, out cjmsg)){
     caller.SendMessage(ReservedCodes.PlayerResponse,
        PrepareResponse(gi.CreatorID, reqcode, 1, cjmsg), 0);
     AddToGame(gi, caller, mem.ID, PlayerGameFlags.Ready);
     gi.Game.Joined(mem.ID);
    } else
     caller.SendMessage(ReservedCodes.PlayerResponse,
        PrepareResponse(gi.CreatorID, reqcode, 0, cjmsg), 0);
    break;
   }

   cito = server[gi.CreatorID];
   if(cito != null){
    output.AddParameter(ClientInfo.IntToBytes(reqcode),
                        ParameterType.Int);
    output.AddParameter(ClientInfo.IntToBytes(mem.ID),
                        ParameterType.Int);
    output.AddParameter(ClientInfo.IntToBytes(gi.ID),
                        ParameterType.Int);
    cito.SendMessage(ReservedCodes.RequestJoinGame,
         output.Read(0, output.Length), 0);
   }
   break;
