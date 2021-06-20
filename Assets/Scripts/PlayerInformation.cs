using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System;
using UnityEngine;

public static class PlayerInformation
{
    public static PlayerData[] players;
    public static int GetPlayerNum(this Player player) => player.GetPlayerData().number;
    public static GameObject GetGameObject(this Player player) => player.GetPlayerData().playerinstance;
    public static PlayerData GetPlayerData(this Player player) => Array.Find(players, (X) => X.player == player);
    //public static bool GetPlayerTeam(this Player player) => player.GetPlayerNum() % 2 == 1;
    public static bool GetPlayerTeam(this Player player) => player.GetPlayerData().isTeam1;


}
public class PlayerData
{
    public Player player;
    public int number;
    public GameObject playerinstance;
    public SpawnPoint spawnPoint;
    public bool isTeam1;

    public PlayerData(Player player, int number, GameObject playerinstance = null)
    {
        this.player = player;
        this.number = number;
        this.playerinstance = playerinstance; 
    }
}
