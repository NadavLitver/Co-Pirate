using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Tip_Randomaizer : MonoBehaviour
{
    [SerializeField]
    private List<string> Tips;
    [SerializeField]
    private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("set_text", 0f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void set_text()
    {
        text.text = "Tip:  " + Tips[Random.Range(0, Tips.Count - 1)];
    }
}
