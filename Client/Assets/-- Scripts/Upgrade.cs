using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Button _btn;

    private int _cost;
    private TroopsType _troopType;
    
    public void Init(Sprite newIcon, int cost, TroopsType troopsType)
    {
        _icon.sprite = newIcon;
        _cost = cost;
        _troopType = troopsType;
        UpdateBtn(false);
    }

    public void UpdateBtn(bool state)
    {
        _btn.interactable = state;
    }

    public void GoUpdate()
    {
        ReserveManager.Instance.BuyShopUpgrade(_cost, _troopType);
    }

    public void GoFusion()
    {
        ReserveManager.Instance.AddNewUnit(_troopType);
    }
}
