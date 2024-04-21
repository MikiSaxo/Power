using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleMenuManager : MonoBehaviour
{
    [Header("----- Setup -----")]
    [SerializeField] private ColorMenuAnim _colorMenuAnim;
    [SerializeField] private TMP_Text _inputText;
    [SerializeField] private Image _bg;
    [SerializeField] private Image _title;
    [SerializeField] private GameObject _inputField;

    [Header("----- Timings -----")]
    [SerializeField] private float _timeScaleText = .5f;
    [SerializeField] private float _timeTitleFade = 1f;
    [SerializeField] private float _timeBgFade = 1f;
    
    public void OnPlayButton()
    {
        var name = _inputText.text;
        
        if (string.IsNullOrEmpty(name) || name.Length < 2)
            return;
        
        _colorMenuAnim.MyPlayerName = name;

        StartCoroutine(AnimTitleMenu());
    }

    IEnumerator AnimTitleMenu()
    {
        _title.DOFade(0, _timeTitleFade);
        _inputField.transform.DOScale(0, _timeScaleText).SetEase(Ease.InBounce);
        
        yield return new WaitForSeconds(_timeScaleText);
        
        _bg.DOFade(0, _timeBgFade);
        
        yield return new WaitForSeconds(_timeBgFade);
        
        gameObject.SetActive(false);
    }
}
