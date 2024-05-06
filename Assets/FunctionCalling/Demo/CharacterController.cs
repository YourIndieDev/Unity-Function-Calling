using Indie.Attributes;
using Indie.OpenAI.Brain;
using System;
using UnityEngine;

namespace Indie.Demo
{
    public class CharacterController : MonoBehaviour
    {
        public Brain brain;

        public bool playerControlled = false;
        public float moveSpeed = 5f; // Adjust speed as needed
        public float rotationSpeed = 100f; // Adjust rotation speed as needed
        public float minDistanceToPlayer = 2f; // Adjust minimum distance to player as needed

        private Rigidbody rb;

        public Transform pickUpPosition;
        public GameObject targetObject;
        public GameObject player;

        public bool isPickedUp = false;

        public event Action OnPickUp;
        public event Action OnRelease;

        private void OnEnable()
        {
            // Register this script to the brain
            brain?.RegisterScript(GetType(), this);
        }

        private void OnDisable()
        {
            // Unregister this script from the brain
            brain?.UnRegisterScript(GetType());
        }


        void Start()
        {
            rb = GetComponent<Rigidbody>();
            if (!player) player = GameObject.FindGameObjectWithTag("Player"); // Assuming the player has a tag "Player"
        }

        void FixedUpdate()
        {
            if (targetObject != null && !isPickedUp)
            {
                MoveTowardsObject(targetObject);
            }
            else if (isPickedUp)
            {
                // If the target object is null, move back towards the player
                MoveTowardsObject(player);
            }

            if (playerControlled)
            {
                HandlePlayerControl();
            }

        }


        public void HandlePlayerControl()
        {
            float moveInput = Input.GetAxis("Vertical");
            float turnInput = Input.GetAxis("Horizontal");

            // Move the character forward or backward
            Vector3 movement = transform.forward * moveInput * moveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + movement);

            // Rotate the character based on turning input
            Quaternion turnRotation = Quaternion.Euler(0f, turnInput * rotationSpeed * Time.deltaTime, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }



        [Tool("GetTargetObject", "A function that allows an the Fetch AI to move towards a desired object. Go and get an object. Fetch an object. Bring an object to the player")]
        [Parameter("targetObj", "The name of the object that the Ai will move towards, get and bring back to the player", "cube", "sphere", "cylinder", "capsule")]
        public void GetTargetObject(string targetObj)
        {
            var target = GameObject.FindGameObjectWithTag(targetObj);

            if (target != null)
                targetObject = target;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == targetObject)
            {
                PickUp(other.gameObject);
            }
            
            if (other.gameObject.CompareTag("Player"))
            {
                Release();
            }
        }

        public void MoveTowardsObject(GameObject objectToGet)
        {
                // Move towards the object
                Vector3 targetPosition = objectToGet.transform.position;
                targetPosition.y = transform.position.y; // Ignore changes in the Y-axis

                Vector3 direction = (targetPosition - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime));

                Vector3 movement = direction * moveSpeed * Time.deltaTime;
                rb.MovePosition(rb.position + movement);
        }


        public void PickUp(GameObject obj)
        {
            // get obj with the tag and put it in the pickup position
            obj.transform.position = pickUpPosition.position;
            obj.transform.parent = pickUpPosition;

            obj.GetComponent<Rigidbody>().isKinematic = true;

            isPickedUp = true;

            OnPickUp?.Invoke();
        }

        public void Release()
        {
            // drop the object
            targetObject.GetComponent<Rigidbody>().isKinematic = false;
            targetObject.transform.parent = null;
            targetObject = null;

            isPickedUp = false;

            OnRelease?.Invoke();
        }
    }
}
