using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Indie.Demo
{

    public class Player : MonoBehaviour
    {
        public float throwForce = 10f; // Adjust throw force as needed
        public RaycastHit hit;
        public LayerMask ignoreLayer; // Layer mask to ignore player collisions


        void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, Mathf.Infinity, ~ignoreLayer);

            // Check for mouse click
            if (Input.GetMouseButtonDown(0))
            {
                Throw(hit.transform.gameObject);
            }
        }

        void Throw(GameObject obj)
        {
            if (obj.CompareTag("cube") || obj.CompareTag("sphere") || obj.CompareTag("cylinder") || obj.CompareTag("capsule"))
            {
                // Check if the hit object has a rigidbody
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                    // Calculate the force direction (in the Z-direction)
                    Vector3 forceDirection = transform.forward * throwForce;

                    // Apply force to the object
                    rb.AddForce(forceDirection, ForceMode.Impulse);
                }
            }
        }
    }
}