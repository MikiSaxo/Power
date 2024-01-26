using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TroopInfo", menuName = "TroopInfo")]
public class TroopInfos : ScriptableObject
{
    [field: SerializeField] public TroopsType TroopsType { get; set; }
    
    [field: SerializeField] public int MovementRange { get; set; }
    [field: SerializeField] public int Power { get; set; }
    
    [field: SerializeField] public bool CanCrossSea { get; set; }
}
