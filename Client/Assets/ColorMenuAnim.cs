using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorMenuAnim : MonoBehaviour
{
    public static ColorMenuAnim Instance;
    
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
    
    [Header("----- Players -----")]
    [SerializeField] private GameObject _prefabPlayerName;
    [SerializeField] private GameObject[] _parentPlayerName;

    private GameObject _currentPlayerName;
    private Image _lastHighlightSelected;
    private List<GameObject> _currentOtherPlayerName = new List<GameObject>();

    public string MyPlayerName { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // OnClickBtnColor(0);
    }

    public void OnClick()
    {
        StartCoroutine(AnimColorMenu());
    }

    private IEnumerator AnimColorMenu()
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

        if (_currentPlayerName == null)
        {
            InstantiateMyPlayerName(index);
        }
        else
        {
            Destroy(_currentPlayerName);
            InstantiateMyPlayerName(index);
        }
    }
    
    private void InstantiateMyPlayerName(int index)
    {
        _currentPlayerName = Instantiate(_prefabPlayerName, _parentPlayerName[index].transform);
        _currentPlayerName.GetComponent<TMP_Text>().text = MyPlayerName;
        PlayerIOScript.Instance.Pioconnection.Send("ChooseColorPlayerName", MyPlayerName, index);
    }
    
    public void AddOtherPlayerName(string playerName, int index)
    {
        if (playerName == MyPlayerName)
            return;
        
        if(_currentOtherPlayerName.Count == 0)
        {
            InstantiateOtherPlayerName(playerName, index);
            return;
        }

        bool hasFound = false;
        foreach (var player in _currentOtherPlayerName)
        {
            if(player.GetComponent<TMP_Text>().text == playerName)
            {
                var playerToDestroy = player;
                _currentOtherPlayerName.Remove(player);
                Destroy(playerToDestroy);
                
                InstantiateOtherPlayerName(playerName, index);

                hasFound = true;
                break;
            }
        }
        
        if(!hasFound)
            InstantiateOtherPlayerName(playerName, index);
    }

    private void InstantiateOtherPlayerName(string playerName, int index)
    {
        var playerNameGo = Instantiate(_prefabPlayerName, _parentPlayerName[index].transform);
        playerNameGo.GetComponent<TMP_Text>().text = playerName;
        _currentOtherPlayerName.Add(playerNameGo);
    }
}
