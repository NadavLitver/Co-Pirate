using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Tip_Randomaizer : MonoBehaviour
{
    [SerializeField]
    private List<string> Tips;
    [SerializeField]
    private TextMeshProUGUI text;

    private Queue<string> _tipsQueue = new Queue<string>();
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("set_text", 0f, 10f);
    }

    void set_text()
    {
        if (_tipsQueue.Count == 0)
            ReshuffleQueue();

        string tip = _tipsQueue.Dequeue();

        text.text = "Tip:  " + tip;
    }
    private void ReshuffleQueue()
    {
        var tempTips = new List<string>(Tips);

        while (tempTips.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, tempTips.Count);

            _tipsQueue.Enqueue(tempTips[index]);
            tempTips.RemoveAt(index);
        }
    }
}
