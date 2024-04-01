using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class OrdersManager : MonoBehaviour
{
    public static OrdersManager Instance;

    [Header("----- Orders -----")]
    [SerializeField] private TMP_Text _textOrderLeft;

    const int MaxOrdersDone = 5;

    
    public int OrdersDone { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public bool CanAddNewOrder()
    {
        if (OrdersDone >= MaxOrdersDone)
        {
            FeedbackCantDoMoreOrders();
            return false;
        }
        
        return true;
    }

    public void UpdateOrdersLeft(bool isAdding)
    {
        if (isAdding)
            OrdersDone++;
        else
            OrdersDone--;

        if (OrdersDone < 0)
            OrdersDone = 0;
        
        UpdateOrdersText();
    }

    private void UpdateOrdersText()
    {
        _textOrderLeft.text = $"Orders Left : {OrdersDone}/5";
    }

    private void FeedbackCantDoMoreOrders()
    {
        gameObject.transform.DOComplete();
        _textOrderLeft.DOComplete();
        
        gameObject.transform.DOPunchPosition(Vector3.one * 5, 1);
        gameObject.transform.DOPunchRotation(Vector3.one, 1);
        _textOrderLeft.DOColor(Color.red, .25f).SetLoops(2, LoopType.Yoyo);
    }

    public void ResetOrders()
    {
        OrdersDone = 0;
        UpdateOrdersText();
    }
}
