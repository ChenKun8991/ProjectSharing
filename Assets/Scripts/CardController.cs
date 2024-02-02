using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public GameObject prefab;
    public static Queue<GameObject> ENEMYPOOL = new Queue<GameObject>();


    // Card pooling 
    // Reroll chance
    public EnemyBase[] enemyObject;
    public int poolSize = 20;


    public GameObject locationToSpawn;

    public Dictionary<int, List<EnemyBase>> groupedCards = new Dictionary<int, List<EnemyBase>>();
    public void Awake()
    {
        for (int i = 0 ; i < enemyObject.Length; i++)
        {
            List<EnemyBase> a = groupedCards.GetValueOrDefault(enemyObject[i].rank, new List<EnemyBase>());
            a.Add(enemyObject[i]);
            groupedCards[enemyObject[i].rank] = a;
        }

        for (int i = StaticValues.CARDS_MIN_RANK; i <= StaticValues.CARDS_MAX_RANK; i++)
        {
            Debug.Log(groupedCards[i].Count);
        }
        Reroll();
    }
    public void Reroll()
    {
        for(int i = 0; i < StaticValues.CARDS_PER_ROLL; i++)
        {
            // a function to calculate the change 
            // let say 0 - 100 
            // 0 - 75
            // 76 - 95
            // 96 - 99
            int chance = Random.Range(0, 100);
            if(chance >= 96)
            {
                // legendary card
                Instantiate(prefab, locationToSpawn.transform);
                Debug.Log(getSelectedRankCard(5).description);
            }
            else if( chance >= 76)
            {
                // epic card
                Instantiate(prefab, locationToSpawn.transform);
                Debug.Log(getSelectedRankCard(4).description);

            }
            else
            {
                // rare card
                Instantiate(prefab, locationToSpawn.transform);
                Debug.Log(getSelectedRankCard(3).description);
            }
        }
    }

    public EnemyBase getSelectedRankCard(int rank)
    {
        EnemyBase e = null;
        if (groupedCards.ContainsKey(rank))
        {
            int index = Random.Range(0, groupedCards[rank].Count);
            return groupedCards[rank][index];
        }

        Debug.Log("No card containing rank: " + rank);
        return null;
    }

    public List<EnemyBase> getAllSelectedRankCard(int rank)
    {
        List<EnemyBase> enemyBases = new List<EnemyBase>();

        return enemyBases;
    }
}
