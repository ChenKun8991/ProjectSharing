using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridCalculation : MonoBehaviour
{
    public GridLayoutGroup cardStore;
    public GridLayoutGroup yourHand;

    public RectTransform topdisplay;
    public RectTransform rerollbutton;

    public RectTransform bottomdisplay;


    public void Start()
    {
        float currentWidth = this.GetComponent<RectTransform>().sizeDelta.x;
        float topWidth = currentWidth / 5;
        float bottomWidth = currentWidth / 10;
        cardStore.cellSize = new Vector2(topWidth, topWidth * StaticValues.ASPECT_RATIO);
        yourHand.cellSize = new Vector2(bottomWidth, bottomWidth * StaticValues.ASPECT_RATIO);

        topdisplay.anchoredPosition = new Vector2(topdisplay.anchoredPosition.x, -topWidth * StaticValues.ASPECT_RATIO);
        rerollbutton.anchoredPosition = new Vector2(rerollbutton.anchoredPosition.x, -topWidth * StaticValues.ASPECT_RATIO);
        bottomdisplay.anchoredPosition = new Vector2(bottomdisplay.anchoredPosition.x, bottomWidth * StaticValues.ASPECT_RATIO);
    }
}
