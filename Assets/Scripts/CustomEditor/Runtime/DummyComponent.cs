using UnityEngine;

namespace Danejw
{
    public class DummyComponent : MonoBehaviour
    {
        [Range(0, 100)]
        public float someFloatValue;



        [Button("Print")]
        public void Print()
        {
            Debug.Log("Hello");
        }

        [Button("On")]
        public void On()
        {
            Debug.Log("Hello");
        }

        [Button("Demand")]
        public void Demand()
        {
            Debug.Log("Hello");
        }

        [Button("Yay")]
        public void Yay()
        {
            Debug.Log("Hello");
        }
    }
}

