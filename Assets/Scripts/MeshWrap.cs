using CustomAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshWrap : MonoBehaviour
{
    [SerializeField, LocalComponent]
    private MeshRenderer _mesh;

    public void SetColor(Color color) => _mesh.material.SetColor("_MainColor", color);
    public void SetColorToRed() => SetColor(Color.red);
    public void SetColorToBlue() => SetColor(Color.blue);
}
