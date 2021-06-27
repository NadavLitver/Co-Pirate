using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cannon_LiveLobby : MonoBehaviour
{
    [SerializeField]
    UnityEvent Shot_Repeat;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("shot", 0f,3f);
    }
    void shot() 
    {
        Shot_Repeat.Invoke();
    }
  
}
