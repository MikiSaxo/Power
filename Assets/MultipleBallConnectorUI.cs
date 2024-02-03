using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MultipleBallConnectorUI : MonoBehaviour
{
    public static MultipleBallConnectorUI Instance;
    
    public RawImage linePrefab; // Préfabriqué de ligne (UI Raw Image)
    public LayerMask ballLayer;

    private List<Vector3> selectedBalls = new List<Vector3>();

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectBall();
        }

        if (selectedBalls.Count > 1)
        {
            DrawConnectionLines();
        }
    }

    void SelectBall()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, ballLayer);

        if (hit.collider != null)
        {
            Vector3 selectedBall = hit.collider.transform.position;

            // Ajouter ou supprimer la balle de la sélection
            if (!selectedBalls.Contains(selectedBall))
            {
                selectedBalls.Add(selectedBall);
            }
            else
            {
                selectedBalls.Remove(selectedBall);
            }
        }
    }

    public void ResetLine()
    {
        selectedBalls.Clear();
    }
    public void AddBall(Vector3 point)
    {
        if (!selectedBalls.Contains(point))
        {
            selectedBalls.Add(point);
        }

        // else
        // {
        //     selectedBalls.Remove(point);
        // }
    }

    void DrawConnectionLines()
    {
        for (int i = 0; i < selectedBalls.Count - 1; i++)
        {
            Vector3 pointA = selectedBalls[i];
            Vector3 pointB = selectedBalls[i + 1];

            RawImage lineInstance = GetOrCreateLineInstance(i);

            // Convertir les coordonnées de l'écran vers l'espace du Canvas
            RectTransform canvasRect = GetComponent<RectTransform>();
            Vector2 screenPointA = RectTransformUtility.WorldToScreenPoint(Camera.main, pointA);
            Vector2 screenPointB = RectTransformUtility.WorldToScreenPoint(Camera.main, pointB);
            Vector2 lineCenterScreen = (screenPointA + screenPointB) / 2f;

            // Convertir les coordonnées de l'écran vers l'espace du Canvas
            Vector2 lineCenterLocal;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, lineCenterScreen, Camera.main, out lineCenterLocal);

            float lineLength = Vector2.Distance(screenPointA, screenPointB);
            float lineAngle = Mathf.Atan2(screenPointB.y - screenPointA.y, screenPointB.x - screenPointA.x) * Mathf.Rad2Deg;

            lineInstance.rectTransform.sizeDelta = new Vector2(lineLength, lineInstance.rectTransform.sizeDelta.y);
            lineInstance.rectTransform.anchoredPosition = lineCenterLocal;
            lineInstance.rectTransform.rotation = Quaternion.Euler(0f, 0f, lineAngle);

            lineInstance.enabled = true;
        }

        DisableExtraLines(selectedBalls.Count - 1);
    }

    RawImage GetOrCreateLineInstance(int index)
    {
        if (index < transform.childCount)
        {
            return transform.GetChild(index).GetComponent<RawImage>();
        }
        else
        {
            RawImage newLine = Instantiate(linePrefab, transform);
            newLine.enabled = false;
            return newLine;
        }
    }

    
    void DisableExtraLines(int activeLineCount)
    {
        for (int i = activeLineCount; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<RawImage>().enabled = false;
        }
    }
}
