using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RadioText : MonoBehaviour
{
    public float letterPause = 0.2f;
    public AudioClip typeSound1;
    public AudioClip typeSound2;
    public AudioClip typeSound3;

    private string message;
    private Text textComp;
    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        textComp = GetComponent<Text>();
        message = textComp.text;
        textComp.text = "";
        StartCoroutine(TypeText());
    }

    public void WriteText(string text)
    {
        textComp.text = "";
        message = text;
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
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
    }
}