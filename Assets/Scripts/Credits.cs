using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    public bool creditsStarted = false;
    public float fadePerSecond = 1.5f;
    private SpriteRenderer spriteRenderer;
    private string[] messages;
    private RadioText radioText;
    private RadioManager radioManager;

    private void Start()
    {
        radioText = FindObjectOfType<RadioText>();
        radioManager = FindObjectOfType<RadioManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.messages = new string[]
        {
            "We received many messages from our listeners tonight, but we can't forget to send a shout out to Robin who sent us a quote for what would be her father's birthday.",
            "\"Though my soul may set in darkness, it will rise in perfect light; I have loved the stars too fondly to be fearful of the night.\" Sarah Williams"
        };


        Material material = spriteRenderer.material;
        Color color = material.color;
        material.color = new Color(color.r, color.g, color.b, 0);
    }

    public void EnableCredits()
    {
        this.creditsStarted = true;
        StartCoroutine(RunEnding());
    }

    private IEnumerator RunEnding()
    {
        string[] credits= new string[]
           {
               "And now, for our closing message, lets give a big thanks to the ones that made this show possible.",
               "Technical team (coding): \n Diego de Rochebouët \n Pedro Balaguer",
               "AV club (arts): \n Federico Jiménez Poza\n Ezequiel Inverni\n Carlos Chaves",
               "And your host (writing): \n Fidel Chaves",
               "Thanks for playing!"
           };

        radioText.CloseBubble();
        radioManager.StopMessages();
        yield return new WaitForSeconds(2f);
        radioText.WriteTextNow(messages);
        yield return new WaitForSeconds(45f);
        radioText.CloseBubble();
        yield return new WaitForSeconds(10f);
        radioText.WriteTextNow(credits);
    }

    void Update()
    {
        if (creditsStarted)
        {
            Material material = spriteRenderer.material;
            Color color = material.color;
            float alpha = Mathf.Min(color.a + (fadePerSecond * Time.deltaTime), 1);
            material.color = new Color(color.r, color.g, color.b, alpha);
        }
    }
}
