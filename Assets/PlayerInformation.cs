using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class PlayerInformation 
{
  public static Player[] players;
    public static int GetPlayerNum(this Player player)
    {
        return Array.FindIndex(players, (X) => X == player) + 1;
    }
}
