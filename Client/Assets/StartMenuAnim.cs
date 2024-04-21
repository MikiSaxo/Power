using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuAnim : MonoBehaviour
{
    [Header("----- Setup -----")]
    [SerializeField] private Image _bg;
    [SerializeField] private TMP_Text _textChooseColor;
    [SerializeField] private GameObject _buttonBattle;
    [SerializeField] private GameObject[] _buttonsColors;
    [SerializeField] private Image[] _imgHighlight;
    
    [Header("----- Timings -----")]
    [SerializeField] private float _timeScaleText = .5f;
    [SerializeField] private float _timeScaleBtnColor = .5f;
    [SerializeField] private float _timeBetweenBtnColor = .1f;
    [SerializeField] private float _timeScaleBtnBattle = 1f;
    [SerializeField] private float _timeBgFade = 1f;

    private Image _lastHighlightSelected;

    private void Start()
    {
        OnClickBtnColor(0);
    }

    public void OnClick()
    {
        StartCoroutine(AnimStartMenu());
    }

    private IEnumerator AnimStartMenu()
    {
        _textChooseColor.gameObject.transform.DOScale(0, _timeScaleText).SetEase(Ease.InBounce);;
        
        foreach (var button in _buttonsColors)
        {
            yield return new WaitForSeconds(_timeBetweenBtnColor);
            button.transform.DOScale(0, _timeScaleBtnColor).SetEase(Ease.InBounce);
        }
        
        _buttonBattle.transform.DOScale(0, _timeScaleBtnBattle).SetEase(Ease.InBounce);
        yield return new WaitForSeconds(_timeBgFade - (_timeBetweenBtnColor * _buttonsColors.Length));
        
        _bg.DOFade(0, _timeBgFade);
        yield return new WaitForSeconds(_timeBgFade);
        
        gameObject.SetActive(false);
    }

    public void OnClickBtnColor(int index)
    {
        Manager.Instance.SetMyColor(index);
        
        _lastHighlightSelected.DOFade(0, .1f);
        _lastHighlightSelected = _imgHighlight[index];
        _lastHighlightSelected.DOFade(1, .25f);
    }
}
