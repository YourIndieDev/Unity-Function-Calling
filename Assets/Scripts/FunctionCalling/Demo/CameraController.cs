using UnityEngine;

namespace Indie.Demo
{
    public class CameraController : MonoBehaviour
    {
        public GameObject lookAtObject; // The GameObject the camera will look at
        public Vector3 offset;

        private Vector3 targetPosition;

        void Update()
        {
            if (lookAtObject == null) return;

            targetPosition = lookAtObject.transform.position + offset;

            // Make the camera look at the fetcher GameObject
            transform.LookAt(targetPosition);
        }
    }
}
