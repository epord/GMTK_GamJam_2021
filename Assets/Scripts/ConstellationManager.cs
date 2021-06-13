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
    public float constellationMovementXRatio = 0.002f;
    public float constellationMovementYRatio = 0.002f;
    public float cameraMouseMovementRatio = 1.4f;
    public GameObject cameraObject;
    public AudioClip constellationCreatedClip;
    public AudioClip lineCreationClip;
    public AudioClip eraseClip;

    private RadioText radioText;
    private Star selectedStar;
    private Star selectedStarOnClick;
    private LineRenderer currentLine;
    // Each pair of stars represents a line (similar to what is done in 3D models with polygons)
    private List<Star> linesCreated = new List<Star>();

    private Vector3? starFieldPositionStart;
    private Vector3? clickedPosition;

    private void Start()
    {
        radioText = FindObjectOfType<RadioText>();
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        Vector3 mousePosRelative = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        
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
            // Selected the Sky.
            starFieldPositionStart = cameraObject.transform.position;
            float height = 2f * Camera.main.orthographicSize;
            float width = height * Camera.main.aspect;
            clickedPosition = new Vector3(mousePosRelative.x * width / 2, mousePosRelative.y * height / 2, 0);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                GameObject collider = hit.collider.gameObject;
                Star star = collider.GetComponent(typeof(Star)) as Star;
                if (star != null)
                {
                    selectedStarOnClick = star;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            starFieldPositionStart = null;
            clickedPosition = null;
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                GameObject collider = hit.collider.gameObject;
                Star star = collider.GetComponent(typeof(Star)) as Star;
                if (star != null && selectedStarOnClick == star)
                {
                    SelectStar(star);
                }
            }
            selectedStarOnClick = null;
        }

        if (starFieldPositionStart != null && clickedPosition != null)
        {
            float height = 2f * Camera.main.orthographicSize;
            float width = height * Camera.main.aspect;
            Vector3 positionDifference = new Vector3(mousePosRelative.x * width / 2, mousePosRelative.y * height / 2, 0) - clickedPosition.Value;
            Vector3 positionDifferenceAdjusted = new Vector3(positionDifference.x * cameraMouseMovementRatio, positionDifference.y * cameraMouseMovementRatio, 0);
            cameraObject.transform.position = starFieldPositionStart.Value - positionDifferenceAdjusted;
        }
        if (selectedStar != null && this.currentLine != null)
        {
            currentLine.SetPosition(1, mousePos2D);
        }
    }

    private void MoveConstellation(int x, int y)
    {
        cameraObject.transform.position += new Vector3(constellationMovementXRatio * x, constellationMovementYRatio * y, 0);
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
            StartCoroutine(this.CreateConstellation(constellation));
        }
        else if (lineCreationClip)
        {
            SoundManager.instance.PlaySingle(lineCreationClip);
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
        if (linesCreated.Count > 0) this.linesCreated.RemoveAt(this.linesCreated.Count - 1);
        if (this.currentLine != null)
        {
            Destroy(this.currentLine.gameObject);
        }
        this.currentLine = null;
        this.selectedStar = null;
        if (eraseClip)
        {
            SoundManager.instance.PlaySingle(eraseClip);
        }
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

    private IEnumerator CreateConstellation(Constellation constellation)
    {
        if (this.transform.childCount == 0)
        {
            yield return new WaitForSeconds(0); // can't return nothing
        }

            GameObject instantiatedConstellation = Instantiate(contellationPrefab);
        while (this.transform.childCount > 0)
        {
            Transform child = this.transform.GetChild(0);
            child.transform.parent = instantiatedConstellation.transform;
            LineRenderer lineRenderer = child.gameObject.GetComponent<LineRenderer>();
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
        }
        this.linesCreated.Clear();

        if (constellationCreatedClip)
        {
            SoundManager.instance.PlaySingle(constellationCreatedClip);
        }
        yield return new WaitForSeconds(2);
        radioText.WriteText(constellation.constellationName);
    }
}
