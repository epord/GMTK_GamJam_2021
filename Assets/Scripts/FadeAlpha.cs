using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeAlpha : MonoBehaviour
{
    public float fadePerSecond = 1.5f;
    private bool isVisible = false;

    private void Start()
    {
        Material material = GetComponent<Renderer>().material;
        Color color = material.color;
        material.color = new Color(color.r, color.g, color.b, 0);

        material = GetComponentInChildren<Text>().material;
        color = material.color;
        material.color = new Color(color.r, color.g, color.b, 0);
    }

    private void Update()
    {
        if (isVisible)
        {
            // Fade in
            Material material = GetComponent<Renderer>().material;
            Color color = material.color;
            float alpha = Mathf.Min(color.a + (fadePerSecond * Time.deltaTime), 1);
            material.color = new Color(color.r, color.g, color.b, alpha);

            material = GetComponentInChildren<Text>().material;
            color = material.color;
            material.color = new Color(color.r, color.g, color.b, alpha);
        }
        else
        {
            // Fade out
            Material material = GetComponent<Renderer>().material;
            Color color = material.color;
            float alpha = Mathf.Max(color.a - (fadePerSecond * Time.deltaTime), 0);
            material.color = new Color(color.r, color.g, color.b, alpha);

            material = GetComponentInChildren<Text>().material;
            color = material.color;
            material.color = new Color(color.r, color.g, color.b, alpha);
        }
}

    public void FadeOut()
    {
        isVisible = false;
    }

    public void FadeIn()
    {
        isVisible = true;
    }
}