using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * This script manages object pooling for card objects and contains main functions
 * related to card handling within the game such as rerolling, buying, and selling.
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


    // "card store" parent game object
    public GameObject storeLocation;

    // "your cards" parent game object
    public GameObject yourCardLocation;

    // the dictionary that contain all key value of rank and the corresponding cards in a list
    public Dictionary<int, List<EnemyBase>> groupedCards = new Dictionary<int, List<EnemyBase>>();

    // Awake is called when the script instance is being loaded
    public void Awake()
    {

        // Set up the initial object pool with inactive gameObjects
        // Chanllenge make the object pooling dynamic ~ 
        for (int i = 0; i < poolSize; i ++)
        {
            //it instantiates a prefab (a template GameObject)
            GameObject g =  Instantiate(prefab); 

            g.GetComponent<Card>().assetManagement = this.GetComponent<AssetManagement>();

            //Adds the instantiated GameObject to Enemy Pool queue.
            //used to manage the pool of enemy/card objects.
            ENEMYPOOL.Enqueue(g);

            //makes the gameObject inactive
            g.SetActive(false);
        }

        // set up dictionary for different rank cards
        for (int i = 0 ; i < enemyObject.Length; i++)
        {
            /*
             * Retrieves a list of EnemyBase objects from a dictionary named groupedCards based on the rank of the current enemyObject. 
             * If no list exists for that rank, a new list is created.
             */
            List<EnemyBase> a = groupedCards.GetValueOrDefault(enemyObject[i].rank, new List<EnemyBase>());
            a.Add(enemyObject[i]);

            //Updates the groupedCards dictionary, assigning the list a to the key corresponding to the current enemyObject's rank.
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
            // Get each child GameObject (card) of the storeLocation
            GameObject g = storeLocation.transform.GetChild(i).gameObject;
            gl.Add(g);

            // Get the Button component of the GameObject
            Button button = g.GetComponent<Button>();

            // Add an onClick event listener to the button, calling buyCard function when clicked
            button.onClick.AddListener(() => buyCard(g));

        }

        while (gl.Count != StaticValues.CARDS_PER_ROLL)
        {
            GameObject g = ENEMYPOOL.Dequeue();
            // Set the parent of the GameObject to storeLocation, making it appear in the store UI
            g.transform.SetParent(storeLocation.transform);

            //Activates the game object
            g.SetActive(true); 
            gl.Add(g);
            Button button = g.GetComponent<Button>();

            // Add an onClick event listener to the button, calling buyCard function when clicked
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

            // Set the parent of the GameObject to yourCardLocation,
            // moving the card to the player's collection

            g.SetActive(true);

            g.transform.SetParent(yourCardLocation.transform);
            Button button = g.GetComponent<Button>();

            // Remove all previously registered listeners 
            // from the button to avoid duplicate actions
            button.onClick.RemoveAllListeners();

            // Add a new onClick listener to the button,
            // setting up the sellCard function to be called when the card is clicked
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
