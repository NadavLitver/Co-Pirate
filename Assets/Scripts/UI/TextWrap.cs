using CustomAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TextWrap : MonoBehaviour
{
    [SerializeField, LocalComponent]
    private TextMeshProUGUI _text;
    [SerializeField, LocalComponent]
    private DOTweenAnimation _tweenAnimation;

    [SerializeField]
    private string _prefix;
    [SerializeField]
    private string _suffix;


    public void SetText(string text) => SetText<string>(text);
    public void SetText(int text) => SetText<int>(text);
    public void SetText<T>(T text)
    {
        _text.text = _prefix + text + _suffix;
        if (_tweenAnimation != null)
            _tweenAnimation.DORestart();
    }
}
