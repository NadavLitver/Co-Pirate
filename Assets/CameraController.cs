using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  internal Transform myfollow;
  private void LateUpdate()
  {
        SetZaxis(); 
  }
  void SetZaxis()
  {
        this.transform.position = new Vector3(transform.position.x, transform.position.y,myfollow.position.z);
  }
}
