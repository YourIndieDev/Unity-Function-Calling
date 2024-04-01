using UnityEngine;

namespace Indie.Demo
{
    public class CameraController : MonoBehaviour
    {
        public GameObject lookAtObject; // The GameObject the camera will look at
        public Vector3 offset;

        void Update()
        {
            if (lookAtObject == null) return;

            // Set the position of the camera
            Vector3 targetPosition = lookAtObject.transform.position + offset;


            // Make the camera look at the fetcher GameObject
            transform.LookAt(targetPosition);

        }
    }
}
