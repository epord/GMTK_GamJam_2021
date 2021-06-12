using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstellationManager: MonoBehaviour
{
    public GameObject linePrefab;

    private Star selectedStar;
    private LineRenderer currentLine;
    private List<LineRenderer> linesCreated = new List<LineRenderer>();

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        if (this.selectedStar != null && this.currentLine != null)
        {
            this.currentLine.SetPosition(1, mousePos2D);
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
                    if (this.selectedStar == null)
                    {
                        this.selectedStar = star;
                        this.CreateLine(star.transform.position, mousePos2D);
                    }
                    else
                    {
                        ConsolidateLine(star.transform.position);
                        this.selectedStar = null;
                    }
                }
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

    }
}
