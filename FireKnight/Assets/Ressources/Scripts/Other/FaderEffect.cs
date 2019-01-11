using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaderEffect : MonoBehaviour {

    [SerializeField] Image image;

    float time;
    Color invColor;
    Color fullColor;

    bool alreadyFadeIn = false;

    private void SetupColor(Color c)
    {
        invColor = new Color(c.r, c.g, c.b, 0);
        fullColor = new Color(c.r, c.g, c.b, 1);
    }


    public void FadeIn(Color c, float _time)
    {
        if (alreadyFadeIn)
        {
            return;
        }
        else
        {
            alreadyFadeIn = true;
        }

        SetupColor(c);
        time = _time;
        StartCoroutine(Fade(true));
    }


    public void FadeOut(Color c, float _time)
    {
        SetupColor(c);
        time = _time;
        StartCoroutine(Fade(false));
    }

    private IEnumerator Fade(bool isIn)
    {

        Color i, e;

        if (isIn)
        {
            i = fullColor;
            e = invColor;
        }
        else
        {
            i = invColor;
            e = fullColor;
        }


        float counter = 0;

        while (counter < time)
        {
            image.color = Color.Lerp(i, e, counter / time);
            yield return null;
            counter += Time.deltaTime;
        }
        image.color = e;
    }
}
