using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGradientColor : MonoBehaviour
{
    public UIGradient _uiGradient;

    //Min: 0.2
    //Max : 0.85

    private float r = 0.2f;
    private float g = 0.85f;
    private float b = 0.2f;

    private float changeMaxPerRound = 0.65f;
    private float currentChange = 0f;
    private float changeAmount = 0.005f;

    private int colorToChange = 1;
    private int mult = 1;
    // Update is called once per frame
    void FixedUpdate()
    {
        _uiGradient.m_color1 = new Color(r, g, b);

        if (currentChange >= changeMaxPerRound)
        {
            currentChange = 0;
            colorToChange++;
            if(colorToChange > 3) { colorToChange = 1; }

            if(colorToChange == 1) { 
                if(r >= 0.85) { r = 0.85f; mult = -1; } 
                else if(r <= 0.2f) { r = 0.2f; mult = 1; }
            }
            else if(colorToChange == 2) {
                if (g >= 0.85) { g = 0.85f; mult = -1; }
                else if (g <= 0.2f) { g = 0.2f; mult = 1; }
            }
            else if(colorToChange == 3) {
                if (b >= 0.85) { b = 0.85f; mult = -1; }
                else if (b <= 0.2f) { b = 0.2f; mult = 1; }
            }
        }

        if (colorToChange == 1)
        {
            r += changeAmount * mult;
            currentChange += changeAmount;
        }

        if (colorToChange == 2)
        {
            g += changeAmount * mult;
            currentChange += changeAmount;
        }

        if (colorToChange == 3)
        {
            b += changeAmount * mult;
            currentChange += changeAmount;
        }
    }
}
