using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateCrowd : MonoBehaviour
{
    public Transform[] spawnPoints;
    public Sprite[] crowdMembers;
    [SerializeField] private GameObject crowdMemberPrefab;
    private GameObject currentCrowdMember;
    void Start()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            currentCrowdMember = Instantiate(crowdMemberPrefab);
            currentCrowdMember.transform.position = spawnPoints[i].position;
            currentCrowdMember.GetComponent<SpriteRenderer>().sprite = crowdMembers[Random.Range(0, crowdMembers.Length)];
        }
    }
}
