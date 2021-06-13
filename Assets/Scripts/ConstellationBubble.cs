using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstellationBubble : MonoBehaviour
{
    public Sprite[] constellations;
    public Sprite[] constellationsBlue;
    public SpriteRenderer currentConstelationRenderer;
    public GameObject prev;
    public GameObject next;

    private int currentConstellationIndex;

    // Start is called before the first frame update
    void Start()
    {
        this.currentConstelationRenderer.sprite = constellations[0];
        this.currentConstellationIndex = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                GameObject collider = hit.collider.gameObject;
                if (collider.Equals(prev))
                {
                    this.PrevConstellation();
                }
                else if (collider.Equals(next))
                {
                    this.NextConstellation();
                }
            }
        }
    }

    public void ConstellationCompleted(int constellationNumber)
    {
        constellations[constellationNumber] = constellationsBlue[constellationNumber];
        this.currentConstelationRenderer.sprite = constellations[this.currentConstellationIndex];
    }

    void PrevConstellation()
    {
        this.currentConstellationIndex = (this.currentConstellationIndex - 1 + constellations.Length) % constellations.Length;
        this.currentConstelationRenderer.sprite = constellations[this.currentConstellationIndex];
    }

    void NextConstellation()
    {
        this.currentConstellationIndex = (this.currentConstellationIndex + 1) % constellations.Length;
        this.currentConstelationRenderer.sprite = constellations[this.currentConstellationIndex];
    }
}
