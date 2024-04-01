using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Button _btn;

    private int _cost;
    private int _index;
    
    public void Init(Sprite newIcon, int cost, int index)
    {
        _icon.sprite = newIcon;
        _cost = cost;
        _index = index;
        UpdateBtn(false);
    }

    public void UpdateBtn(bool state)
    {
        _btn.interactable = state;
    }

    public void GoUpdate()
    {
        ReserveManager.Instance.BuyUpgrade(_cost, _index);
    }
}
