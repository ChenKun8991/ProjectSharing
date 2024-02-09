using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * The script for each invidual card, after passing in the enemybase from controller, 
 * this is where we should set all different setting to the card gameobject
 */
public class Card : MonoBehaviour
{
    public EnemyBase card;
    public Image border;
    public Image img;
    public AssetManagement assetManagement;

    private void setIt()
    {
        if (card)
        {
            border.sprite = assetManagement.cardBorders[card.rank - 3];
            img.sprite = card.img;
        }
    }

    public void setCard(EnemyBase card)
    {
        this.card = card;
        setIt();
    }

    private void OnDisable()
    {
        card = null;
    }
}
