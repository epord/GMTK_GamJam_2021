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
    public float keyboardMovementRatio = 0.002f;
    public float cameraMouseMovementRatio = 1.4f;
    public float cameraDragThreshold = 0.2f;

    public float cameraBorderLeftX;
    public float cameraBorderRightX;
    public float cameraBorderTopY;
    public float cameraBorderBottomY;
    
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
    private AudioSource audioSource;

    private void Start()
    {
        radioText = FindObjectOfType<RadioText>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        Vector3 mousePosRelative = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        float keyboardMovementX = 0f;
        float keyboardMovementY = 0f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            keyboardMovementY += 1;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            keyboardMovementX += 1;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            keyboardMovementY -= 1;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            keyboardMovementX -= 1;
        }
        MoveConstellation(keyboardMovementX, keyboardMovementY);

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

        if (Input.GetMouseButton(0))
        {
            if (starFieldPositionStart != null && clickedPosition != null)
            {
                float height = 2f * Camera.main.orthographicSize;
                float width = height * Camera.main.aspect;
                Vector3 positionDifference = new Vector3(mousePosRelative.x * width / 2, mousePosRelative.y * height / 2, 0) - clickedPosition.Value;
                Vector3 positionDifferenceAdjusted = new Vector3(positionDifference.x * cameraMouseMovementRatio, positionDifference.y * cameraMouseMovementRatio, 0);
                if (Math.Abs(positionDifferenceAdjusted.x) + Math.Abs(positionDifferenceAdjusted.y) > cameraDragThreshold)
                {
                    cameraObject.transform.position = starFieldPositionStart.Value - positionDifferenceAdjusted;
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
        
        if (selectedStar != null && this.currentLine != null)
        {
            currentLine.SetPosition(1, mousePos2D);
        }

        if (cameraObject.transform.position.x < cameraBorderLeftX)
        {
            cameraObject.transform.position = new Vector3(cameraBorderLeftX, cameraObject.transform.position.y,
                cameraObject.transform.position.z);
        }
        
        if (cameraObject.transform.position.x > cameraBorderRightX)
        {
            cameraObject.transform.position = new Vector3(cameraBorderRightX, cameraObject.transform.position.y,
                cameraObject.transform.position.z);
        }
        
        if (cameraObject.transform.position.y < cameraBorderBottomY)
        {
            cameraObject.transform.position = new Vector3(cameraObject.transform.position.x, cameraBorderBottomY,
                cameraObject.transform.position.z);
        }
        
        if (cameraObject.transform.position.y > cameraBorderTopY)
        {
            cameraObject.transform.position = new Vector3(cameraObject.transform.position.x, cameraBorderTopY,
                cameraObject.transform.position.z);
        }
    }

    private void MoveConstellation(float x, float y)
    {
        Vector3 cameraMovement = new Vector3(keyboardMovementRatio * x, keyboardMovementRatio * y, 0);
        Vector3 normalizeMovement = cameraMovement.normalized;
        cameraObject.transform.position += new Vector3(keyboardMovementRatio * normalizeMovement.x, keyboardMovementRatio * normalizeMovement.y, 0);
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
            audioSource.clip = lineCreationClip;
            audioSource.Play();
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
            audioSource.clip = eraseClip;
            audioSource.Play();
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
            lineRenderer.startColor = new Color(0f, 50f, 160f);
            lineRenderer.endColor = new Color(0f, 50f, 160f);
        }
        this.linesCreated.Clear();

        if (constellationCreatedClip)
        {
            audioSource.clip = constellationCreatedClip;
            audioSource.Play();
        }
        yield return new WaitForSeconds(0.2f);
        radioText.WriteText(constellation.messages);
    }
}
