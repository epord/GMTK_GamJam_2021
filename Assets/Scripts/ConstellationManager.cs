using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstellationManager: MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject contellationPrefab;

    private Star selectedStar;
    private LineRenderer currentLine;
    private List<LineRenderer> linesCreated = new List<LineRenderer>();

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        
        if (selectedStar != null && this.currentLine != null)
        {
            currentLine.SetPosition(1, mousePos2D);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetAllLines();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CreateConstellation();
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            if (selectedStar != null && this.currentLine != null)
            {
                ResetLine();
            }
            else
            {
                ResetAllLines();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                GameObject collider = hit.collider.gameObject;
                Star star = collider.GetComponent(typeof(Star)) as Star;
                if (star != null)
                {
                    SelectStar(star);
                }
            }
        }
    }

    private void SelectStar(Star star)
    {
        if (star is null) Debug.LogException(new Exception("Passed null as Star"));
        if (this.selectedStar == null)
        {
            this.selectedStar = star;
            this.CreateLine(star.transform.position, star.transform.position);
        }
        else
        {
            if (GameObject.Equals(star, selectedStar))
            {
                ResetLine();
            }
            else
            {
                ConsolidateLine(star.transform.position);
            }
        }
    }

    private void CreateLine(Vector3 from, Vector3 to)
    {
        GameObject lineObject = Instantiate(linePrefab);
        lineObject.transform.parent = this.transform;
        LineRenderer lineRenderer = lineObject.GetComponent<LineRenderer>();
        List<Vector3> pos = new List<Vector3> { from, to };
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.SetPositions(pos.ToArray());
        lineRenderer.useWorldSpace = true;
        this.currentLine = lineRenderer;
    }

    private void ConsolidateLine(Vector3 finalPoint)
    {
        this.currentLine.SetPosition(1, finalPoint);
        this.linesCreated.Add(this.currentLine);
        this.currentLine = null;
        this.selectedStar = null;
    }

    private void ResetLine()
    {
        if (this.currentLine != null)
        {
            Destroy(this.currentLine.gameObject);
        }
        this.currentLine = null;
        this.selectedStar = null;
    }

    private void ResetAllLines()
    {
        ResetLine();
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateConstellation()
    {
        if (this.transform.childCount == 0) return;

        GameObject constellation = Instantiate(contellationPrefab);
        while (this.transform.childCount > 0)
        {
            Transform child = this.transform.GetChild(0);
            child.transform.parent = constellation.transform;
            LineRenderer lineRenderer = child.gameObject.GetComponent<LineRenderer>();
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
        }
    }
}
