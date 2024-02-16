using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro.EditorUtilities;

public class LineConnector : MonoBehaviour
{
    // [SerializeField] private RawImage linePrefab;
    [SerializeField] private LineRenderer _lineRenderer;

    private List<Vector3> _selectedBalls = new List<Vector3>();

    public void LinkLineRenderer(Vector3 start, Vector3 end)
    {
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, start);
        _lineRenderer.SetPosition(1, end);
    }

    public void ResetLine()
    {
        _lineRenderer.positionCount = 0;
    }

    #region OldRawImage
    // void Update()
    // {
        // if (_selectedBalls.Count > 1)
        // {
        //     DrawConnectionLines();
        // }
    // }
    //
    // public void ResetLine()
    // {
    //     _selectedBalls.Clear();
    // }
    // public void AddBall(Vector3 point)
    // {
    //     if (!_selectedBalls.Contains(point))
    //     {
    //         _selectedBalls.Add(point);
    //     }
    //
    //     
    //     // else
    //     // {
    //     //     selectedBalls.Remove(point);
    //     // }
    // }

    // void DrawConnectionLines()
    // {
    //     for (int i = 0; i < _selectedBalls.Count - 1; i++)
    //     {
    //         Vector3 pointA = _selectedBalls[i];
    //         Vector3 pointB = _selectedBalls[i + 1];
    //
    //         RawImage lineInstance = GetOrCreateLineInstance(i);
    //
    //         // Convertir les coordonnées de l'écran vers l'espace du Canvas
    //         RectTransform canvasRect = GetComponent<RectTransform>();
    //         Vector2 screenPointA = RectTransformUtility.WorldToScreenPoint(Camera.main, pointA);
    //         Vector2 screenPointB = RectTransformUtility.WorldToScreenPoint(Camera.main, pointB);
    //         Vector2 lineCenterScreen = (screenPointA + screenPointB) / 2f;
    //
    //         // Convertir les coordonnées de l'écran vers l'espace du Canvas
    //         Vector2 lineCenterLocal;
    //         RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, lineCenterScreen, Camera.main, out lineCenterLocal);
    //
    //         float lineLength = Vector2.Distance(screenPointA, screenPointB);
    //         float lineAngle = Mathf.Atan2(screenPointB.y - screenPointA.y, screenPointB.x - screenPointA.x) * Mathf.Rad2Deg;
    //
    //         lineInstance.rectTransform.sizeDelta = new Vector2(lineLength, lineInstance.rectTransform.sizeDelta.y);
    //         lineInstance.rectTransform.anchoredPosition = lineCenterLocal;
    //         lineInstance.rectTransform.rotation = Quaternion.Euler(0f, 0f, lineAngle);
    //
    //         lineInstance.enabled = true;
    //     }
    //
    //     DisableExtraLines(_selectedBalls.Count - 1);
    // }
    //
    // RawImage GetOrCreateLineInstance(int index)
    // {
    //     if (index < transform.childCount)
    //     {
    //         return transform.GetChild(index).GetComponent<RawImage>();
    //     }
    //     else
    //     {
    //         RawImage newLine = Instantiate(linePrefab, transform);
    //         newLine.enabled = false;
    //         return newLine;
    //     }
    // }
    //
    // void DisableExtraLines(int activeLineCount)
    // {
    //     for (int i = activeLineCount; i < transform.childCount; i++)
    //     {
    //         transform.GetChild(i).GetComponent<RawImage>().enabled = false;
    //     }
    // }
    
    #endregion
}
