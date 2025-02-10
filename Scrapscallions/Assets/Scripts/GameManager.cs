using Scraps.AI.GOAP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Robot robot;
    public GoapAgent goapAgent;
    public Transform spawnpoint;
    public Transform spawnpoint2;
    private void Start()
    {
        Robot robot1 = Instantiate(robot);
        Robot robot2 = Instantiate(robot);
        SpawnRobot(robot1, spawnpoint, robot2);
        SpawnRobot(robot2, spawnpoint2, robot1);
    }
    private void SpawnRobot(Robot robot, Transform spawnPoint, Robot target)
    {
        GoapAgent agent = Instantiate(goapAgent, spawnPoint.position, spawnPoint.rotation);
        robot.Spawn(agent, target);
    }
}
