using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Power : MonoBehaviour
{
    [SerializeField] private Image _powerImg;
    [FormerlySerializedAs("_myColor")] [SerializeField] private ColorsID myColorID;
    [Header("--- Timing ---")]
    [SerializeField] private float _jumpPower = 3f;
    [SerializeField] private float _duration = 3f;
    [field:SerializeField] public float TimePunchScale { get; private set; }
    [field:SerializeField] public float TimeAfterJump { get; private set; }
    
    
    private Reserve _myReserve;


    public void InitAnim(int colorIndex, Reserve myReserve)
    {
        _powerImg.color = Manager.Instance.PawnColors[colorIndex];
        myColorID = (ColorsID)colorIndex;
        _myReserve = myReserve;
    }

    public void InitReserve(int colorIndex)
    {
        _powerImg.color = Manager.Instance.PawnColors[colorIndex];
        myColorID = (ColorsID)colorIndex;
    }

    public void JumpToReserve()
    {
        gameObject.transform.DOJump(_myReserve.gameObject.transform.position, _jumpPower, 1, _duration).SetEase(Ease.OutSine).OnComplete(
            () =>
            {
                _myReserve.AddNewPower((int)myColorID);
                Destroy(gameObject);
            });
    }
}
