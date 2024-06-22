using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ReserveManager : MonoBehaviour
{
    public static ReserveManager Instance;


    [SerializeField] private ShopManager shopManager;
    [SerializeField] private GameObject _powerPrefab;
    [SerializeField] private GameObject[] _reserves;
    [SerializeField] private Color[] _colorsReserves;


    private int _powerCount;


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        // if(Input.GetKeyDown(KeyCode.U))
        //     SpawnPowerToReserve(2);
    }

    public void SetReserveColor()
    {
        var myColorID = Manager.Instance.MyColorID;

        // Set my reserve color
        _reserves[0].GetComponent<Reserve>().InitReserve(myColorID, _colorsReserves[(int)myColorID]);

        List<ColorsID> availableColors = new List<ColorsID> { ColorsID.Blue, ColorsID.Yellow, ColorsID.Red, ColorsID.Green };
        availableColors.Remove(myColorID);

        // Set other reserve colors
        for (int i = 1; i < _reserves.Length; i++)
        {
            var colorIndex = i - 1;
            _reserves[i].GetComponent<Reserve>().InitReserve(availableColors[colorIndex], _colorsReserves[(int)availableColors[colorIndex]]);
        }
    }

    public void AddAllPower(List<Troop> troops)
    {
        StartCoroutine(AddAllPowerTiming(troops));
    }

    IEnumerator AddAllPowerTiming(List<Troop> troops)
    {
        Dictionary<ColorsID, int> colorCounts = new Dictionary<ColorsID, int>();

        // Compter le nombre de "troops" de chaque couleur sur des cases ennemies
        foreach (var troop in troops)
        {
            if (troop.IsCellEnemyColor())
            {
                var colorID = troop.MyColorID;
                if (!colorCounts.TryAdd(colorID, 1))
                {
                    colorCounts[colorID]++;
                }
            }
        }

        Debug.Log("Color Counts: " + string.Join(", ", colorCounts.Select(kv => kv.Key + ": " + kv.Value).ToArray()));

        // Appeler SpawnPowerToReserve pour chaque couleur avec le nombre correspondant de "troops"
        foreach (var colorCount in colorCounts)
        {
            var reserve = FindReserveByColor(colorCount.Key);
            if (reserve != null)
            {
                Debug.Log("Spawning " + colorCount.Value + " power(s) to reserve of color: " + colorCount.Key);
                SpawnPowerToReserve(colorCount.Value, reserve);
                yield return new WaitForSeconds(.5f);
            }
            else
            {
                Debug.Log("No reserve found for color: " + colorCount.Key);
            }
        }
    }

    Reserve FindReserveByColor(ColorsID colorID)
    {
        foreach (var reserveObject in _reserves)
        {
            var reserve = reserveObject.GetComponent<Reserve>();
            if (reserve.MyColorID == colorID)
            {
                return reserve;
            }
        }
        return null;
    }

    public void SpawnPowerToReserve(int powerCount, Reserve reserve)
    {
        _powerCount += powerCount;
        StartCoroutine(TimingSpawnPower(powerCount, reserve));
    }

    IEnumerator TimingSpawnPower(int newPower, Reserve reserve)
    {
        for (int i = 0; i < newPower; i++)
        {
            GameObject go = Instantiate(_powerPrefab, transform);
            var power = go.GetComponent<Power>();

            power.InitAnim((int)reserve.MyColorID, reserve);
            go.transform.DOPunchScale(Vector3.one, power.TimePunchScale);
            yield return new WaitForSeconds(power.TimePunchScale);
            power.JumpToReserve();
            yield return new WaitForSeconds(power.TimeAfterJump);
        }
        
        shopManager.CheckShopUpgradeAvailable(_powerCount);
    }


    public void BuyShopUpgrade(int cost, TroopsType troopType)
    {
        _powerCount -= cost;

        _reserves[0].GetComponent<Reserve>().AddNewUnit(troopType);
        _reserves[0].GetComponent<Reserve>().RemovePower(cost);

        shopManager.CheckShopUpgradeAvailable(_powerCount);
    }

    public void AddNewUnitByFusion(TroopsType troopType)
    {
        var troop = FusionManager.Instance.CurrentTroopFusion;
        TroopsManager.Instance.InstantiateNewTroop((int)troopType, (int)troop.MyColorID, troop.CurrentCell);
        FusionManager.Instance.FusionHasBeenDone();
        
        // _reserves[0].GetComponent<Reserve>().AddNewUnit(troopType);
    }
}