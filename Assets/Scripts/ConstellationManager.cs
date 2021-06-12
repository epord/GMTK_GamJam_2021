using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstellationManager: MonoBehaviour
{
    // Each constellation is defined by
    public List<Constellation> constellations;
    public GameObject linePrefab;
    public GameObject contellationPrefab;
    public float constellationMovementXRatio = 1.0f;
    public float constellationMovementYRatio = 1.0f;
    public GameObject starField;
    
    private Star selectedStar;
    private LineRenderer currentLine;
    // Each pair of stars represents a line (similar to what is done in 3D models with polygons)
    private List<Star> linesCreated = new List<Star>();



    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            MoveConstellation(0, 1);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            MoveConstellation(1, 0);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            MoveConstellation(0, -1);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            MoveConstellation(-1, 0);
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

        if (selectedStar != null && this.currentLine != null)
        {
            currentLine.SetPosition(1, mousePos2D);
        }
    }

    private void MoveConstellation(int x, int y)
    {
        starField.transform.position -= new Vector3(constellationMovementXRatio * x, constellationMovementYRatio * y, 0);
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
                ConsolidateLine(star);
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

    private void ConsolidateLine(Star endStar)
    {
        this.currentLine.SetPosition(1, endStar.transform.position);
        this.linesCreated.Add(this.selectedStar);
        this.linesCreated.Add(endStar);
        this.currentLine = null;
        this.selectedStar = null;

        if (this.IsAnExistingConstellation(out Constellation constellation))
        {
            this.CreateConstellation();
        }
    }

    private bool IsAnExistingConstellation(out Constellation constellation)
    {
        constellation = null;
        foreach (Constellation c in this.constellations)
        {
            if (c.IsSameConstellation(this.linesCreated))
            {
                constellation = c;
                return true;
            }
        }
        return false;
    }

    private void ResetLine()
    {
        this.linesCreated.RemoveAt(this.linesCreated.Count - 1);
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
        this.linesCreated.Clear();
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
        this.linesCreated.Clear();
    }
}
