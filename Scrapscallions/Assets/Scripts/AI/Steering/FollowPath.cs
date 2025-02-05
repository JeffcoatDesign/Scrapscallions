using UnityEngine;

namespace Scraps.AI
{
    [CreateAssetMenu(fileName = "New Follow Path", menuName = "Steering/Follow Path")]
    public class FollowPath : Seek
    {
        public float threshold = 1f;

        private int index = 0;

        protected override Vector2 GetTargetPosition(GameObject target)
        {
            Path path = robotState.Path;
            Vector3 directionToTarget = path.pathNodes[index].transform.position - robotState.character.transform.position;
            float distanceToTarget = directionToTarget.magnitude;

            if (distanceToTarget < threshold)
            {
                index++;
                if (index >= path.pathNodes.Length)
                    index = 0;
            }


            Transform targetNode = path.pathNodes[index];
            return new(targetNode.position.x, targetNode.position.z);
        }
    }
}