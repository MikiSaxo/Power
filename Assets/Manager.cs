using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
   public static Manager Instance;
   
   [SerializeField] private List<Cell> _allCells = new List<Cell>();

   private void Awake()
   {
      Instance = this;
   }

   public void UpdateAllCells(bool state)
   {
      foreach (var cell in _allCells)
      {
         cell.ForceUpdateViewCell(state);
      }
   }
}
