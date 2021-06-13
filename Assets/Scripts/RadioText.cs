using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RadioText : MonoBehaviour
{
    public float letterPause = 0.2f;
    public float messagePause = 20;
    public AudioClip typeSound1;
    public AudioClip typeSound2;
    public AudioClip typeSound3;
    public bool isShowingMessage = false;

    private string[] messages;
    private string message;
    private Text textComp;
    private AudioSource audioSource;
    private FadeAlpha fadeAlpha;

    // Use this for initialization
    void Start()
    {
        fadeAlpha = GetComponentInParent<FadeAlpha>();
        textComp = GetComponent<Text>();
    }

    public void WriteText(string[] messages)
    {
        StopAllCoroutines();
        StartCoroutine(TypeText(messages));
    }
    public void WriteText(string message)
    {
        WriteText(new string[] { message });
    }

    IEnumerator TypeText(string[] messages)
    {
        isShowingMessage = true;
        textComp.text = "";
        fadeAlpha.FadeIn();
        yield return new WaitForSeconds(0.8f);
        foreach (string message in messages)
        {
            textComp.text = "";
            foreach (char letter in message.ToCharArray())
            {
                textComp.text += letter;
                if (typeSound1 && typeSound2 && typeSound3)
                {
                    SoundManager.instance.RandomizeSfx(typeSound1, typeSound2, typeSound3);
                }
                yield return 0;
                yield return new WaitForSeconds(letterPause);
            }
            yield return new WaitForSeconds(messagePause);
        }
        fadeAlpha.FadeOut();
        yield return new WaitForSeconds(1);
        isShowingMessage = false;
    }
}