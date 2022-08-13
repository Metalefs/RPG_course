using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    CanvasGroup canvasGroup;
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public IEnumerator FadeOut(float time)
    {
        while (canvasGroup.alpha < 1)
        {
            Debug.Log(canvasGroup.alpha);
            canvasGroup.alpha += Time.deltaTime / time;
            yield return null;
        }
    }
    public IEnumerator FadeIn(float time)
    {
        while (canvasGroup.alpha > 0)
        {
            Debug.Log(canvasGroup.alpha);
            canvasGroup.alpha -= Time.deltaTime / time;
            yield return null;
        }
    }
}
