using Scraps.AI.GOAP;
using Scraps.Utilities;
using Scraps.Cinematic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps
{
    public class GameManager : MonoBehaviour
    {
        public Robot robot;
        public Robot robot2;
        public GoapAgent goapAgent;
        public Transform spawnpoint;
        public Transform spawnpoint2;
        private void Start()
        {
            robot = robot.Copy();
            robot2 = robot2.Copy();
            SpawnRobot(robot, spawnpoint, robot2);
            SpawnRobot(robot2, spawnpoint2, robot);
            CinematicManager.instance.SetCamera(CinematicManager.CameraType.Group);
        }
        private void SpawnRobot(Robot robot, Transform spawnPoint, Robot target)
        {
            GoapAgent agent = Instantiate(goapAgent, spawnPoint.position, spawnPoint.rotation);
            robot.Spawn(agent, target);
            CinematicManager.instance.AddTarget(agent.transform);
            CinematicManager.instance.AddTarget(robot.headController.transform.parent);
        }
    }
}