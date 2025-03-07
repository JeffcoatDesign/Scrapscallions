using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateCrowd : MonoBehaviour
{
    public Transform[] spawnPoints;
    public Sprite[] crowdMembers;
    [SerializeField] private GameObject crowdMemberPrefab;
    private GameObject currentCrowdMember;

    private int randomCrowdSelect;
    private int previousCrowdSelect = -1;

    void Start()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            currentCrowdMember = Instantiate(crowdMemberPrefab, transform);
            currentCrowdMember.transform.position = spawnPoints[i].position;
            do
            {
                randomCrowdSelect = Random.Range(0, crowdMembers.Length + 3);
            } while (randomCrowdSelect == previousCrowdSelect);
            //Debug.Log(randomCrowdSelect);
            if (randomCrowdSelect >= crowdMembers.Length)
                currentCrowdMember.GetComponent<SpriteRenderer>().sprite = null;
            else
            {
                currentCrowdMember.GetComponent<SpriteRenderer>().sprite = crowdMembers[randomCrowdSelect];
                previousCrowdSelect = randomCrowdSelect;
            }
        }
    }
}
