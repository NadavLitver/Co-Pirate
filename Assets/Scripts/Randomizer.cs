using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Randomizer 
{
  public static int RandomNum(int range)
    {
        int num = Random.Range(0, range);
        return num;
    }
    public static float NormalizedFloat()//return 1 or -1 or in between
    {
        float num = Random.Range(-1f, 1f);
        return num;
    }
}
