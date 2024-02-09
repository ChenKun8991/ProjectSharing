using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * This script to manage object pooling and all main functions
 */
public class CardController : MonoBehaviour
{
    // card prefab
    public GameObject prefab;
    // card prefab object pooling list 
    public static Queue<GameObject> ENEMYPOOL = new Queue<GameObject>();


    // all scriptable objects
    public EnemyBase[] enemyObject;
    
    // object pooling size 
    public int poolSize = 20;


    // card store parent game object
    public GameObject storeLocation;
    // your cards parent game object
    public GameObject yourCardLocation;

    // the dictionary that contain all key value of rank and the corresponding cards in a list
    public Dictionary<int, List<EnemyBase>> groupedCards = new Dictionary<int, List<EnemyBase>>();

    public void Awake()
    {

        // Set up the initial object pool 
        // Chanllenge make the object pooling dynamic ~ 
        for (int i = 0; i < poolSize; i ++)
        {
            GameObject g =  Instantiate(prefab);
            g.GetComponent<Card>().assetManagement = this.GetComponent<AssetManagement>();
            ENEMYPOOL.Enqueue(g);
            g.SetActive(false);
        }

        // set up dictionary for different rank cards
        for (int i = 0 ; i < enemyObject.Length; i++)
        {
            List<EnemyBase> a = groupedCards.GetValueOrDefault(enemyObject[i].rank, new List<EnemyBase>());
            a.Add(enemyObject[i]);
            groupedCards[enemyObject[i].rank] = a;
        }

        Reroll();
    }

    /*
     * Function for the store reroll
     */
    public void Reroll()
    {
        List<GameObject> gl = populateStoreCardObject();

        for (int i = 0; i < StaticValues.CARDS_PER_ROLL; i++)
        {
            // a function to calculate the change 
            // let say 0 - 100 
            // 0 - 75
            // 76 - 95
            // 96 - 99
            GameObject g = gl[i];
            int chance = Random.Range(0, 100);
            if(chance >= 96)
            {
                // legendary card
                g.GetComponent<Card>().setCard(getSelectedRankCard(5));
            }
            else if( chance >= 76)
            {
                // epic card
                g.GetComponent<Card>().setCard(getSelectedRankCard(4));

            }
            else
            {
                // rare card
                g.GetComponent<Card>().setCard(getSelectedRankCard(3));
            }
        }
    }

    /*
     * function to make sure there are 5 gameobject to display for each reroll
     */
    public List<GameObject> populateStoreCardObject()
    {
        List <GameObject> gl =new List<GameObject>();
        for (int i = 0; i < storeLocation.transform.childCount; i++)
        {
            GameObject g = storeLocation.transform.GetChild(i).gameObject;
            gl.Add(g);
            Button button = g.GetComponent<Button>();
            button.onClick.AddListener(() => buyCard(g));
         
        }

        while (gl.Count != StaticValues.CARDS_PER_ROLL)
        {
            GameObject g = ENEMYPOOL.Dequeue();
            g.transform.SetParent(storeLocation.transform);
            g.SetActive(true);
            gl.Add(g);
            Button button = g.GetComponent<Button>();
            button.onClick.AddListener(() => buyCard(g));
        }
        return gl;
    }

    /*
     * function to get a random corresponding rank card
     */
    public EnemyBase getSelectedRankCard(int rank)
    {
        if (groupedCards.ContainsKey(rank))
        {
            int index = Random.Range(0, groupedCards[rank].Count);
            return groupedCards[rank][index];
        }

        Debug.Log("No card containing rank: " + rank);
        return null;
    }

    /*
     * function to buy card with max 10 card as max
     */
    public void buyCard(GameObject g)
    {
        if (yourCardLocation.transform.childCount < 10)
        {
            g.SetActive(true);
            g.transform.SetParent(yourCardLocation.transform);
            Button button = g.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => sellCard(g));

        }
    }

    /*
     * function to sell card
     */
    public void sellCard(GameObject g)
    {
        Button button = g.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        g.transform.parent = null;
        g.SetActive(false);
        ENEMYPOOL.Enqueue(g);
    }

  
}
