﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeText : MonoBehaviour
{

    public Image background;
    public TextMeshProUGUI textMeshPro;

    public string startText;
    public string endText;

    public bool Busy = false;

    private void Start()
    {
    }

    public IEnumerator DoShowAndHideIntro(float fadeDuration)
    {
        Busy = true;
        textMeshPro.text = startText;

        Color backgroundFixedColor = background.color;
        backgroundFixedColor.a = 1;
        background.color = backgroundFixedColor;
        background.CrossFadeAlpha(0f, 0f, true);

        Color textMeshProFixedColor = textMeshPro.color;
        textMeshProFixedColor.a = 1;
        textMeshPro.color = textMeshProFixedColor;
        textMeshPro.CrossFadeAlpha(0f, 0f, true);

        textMeshPro.CrossFadeAlpha(1, fadeDuration,false);
        background.CrossFadeAlpha(1, fadeDuration, false);
        yield return new WaitForSeconds(fadeDuration + 2f);
        StartCoroutine(DoHide(fadeDuration));
    }
    public IEnumerator DoShowOutro(float fadeDuration)
    {
        Busy = true;
        textMeshPro.text = endText;

        Color backgroundFixedColor = background.color;
        backgroundFixedColor.a = 1;
        background.color = backgroundFixedColor;
        background.CrossFadeAlpha(0f, 0f, true);

        Color textMeshProFixedColor = textMeshPro.color;
        textMeshProFixedColor.a = 1;
        textMeshPro.color = textMeshProFixedColor;
        textMeshPro.CrossFadeAlpha(0f, 0f, true);
        textMeshPro.CrossFadeAlpha(1, fadeDuration, false);
        background.CrossFadeAlpha(1, fadeDuration, false);
        yield return new WaitForSeconds(fadeDuration + 2f);
        Busy = false;
    }

    private IEnumerator DoHide(float fadeDuration)
    {
        textMeshPro.CrossFadeAlpha(0, fadeDuration, false);
        background.CrossFadeAlpha(0, fadeDuration, false);
        yield return new WaitForSeconds(fadeDuration);

        textMeshPro.text = "";
        Busy = false;
    }
}
