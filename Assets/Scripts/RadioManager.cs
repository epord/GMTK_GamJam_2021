using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioManager : MonoBehaviour
{
    public float timeBetweenMessages = 90;

    private string[] messages;
    private RadioText radioText;

    // Start is called before the first frame update
    void Start()
    {
        radioText = FindObjectOfType<RadioText>();
        messages = new string[]
        {
            "It’s exactly midnight and this is K.T. speaking to you from Radiostar aficionado, studying the universe one star at a time.",
            "I hope you are taking advantage of this clear night for stargazing. Today's topic: the zodiac.",
            "Although it is not a science, we can talk about mythology and take the opportunity to learn astronomy thanks to the constellations.",
            "For tonight’s program we have a quote from Marcus Aurelius: “Dwell on the beauty of life.Watch the stars, and see yourself running with them.”",
            "As always, we will be playing music until the stars are no longer visible, so make the most of the night! Carpe noctem!",
            "For those of you who are alone looking at the stars, did you remember to check in with those you love ? They may be looking at the same sky as you",
            "It's going to be a long night, stay tuned to Radiostar aficionado for the best ambience and astronomy facts.",
            "If you like the program, remember to share it with your friends, relatives, acquaintances and strangers.Share the love!",
            "“I want to take this opportunity to send love to my children who are sleeping on this peaceful night.” says Caytlyn from the Lake.",
            "Do you share your love for the stars with your partner ? Make sure to tell them they are your Sun and stars!",
            "We received many messages from our listeners tonight, but we can't forget to send a shout out to Robin who sent us a quote for what would be her father's birthday.",
            "“Though my soul may set in darkness, it will rise in perfect light; I have loved the stars too fondly to be fearful of the night.” Sarah Williams"
        };
        StartCoroutine(ShowMessages());
    }

    private IEnumerator ShowMessages()
    {
        yield return new WaitForSeconds(15);
        foreach (string message in messages)
        {
            while (radioText.isShowingMessage)
            {
                yield return new WaitForSeconds(5);
            }
            radioText.WriteText(message);
            yield return new WaitForSeconds(timeBetweenMessages);
        }
    }
}
