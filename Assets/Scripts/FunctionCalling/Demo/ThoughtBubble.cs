using UnityEngine;


namespace Indie.Demo.UI
{
    public class ThoughtBubble : MonoBehaviour
    {
        [SerializeField] private BrainBody brainBody;
        [SerializeField] private GameObject bubble;


        private void Update()
        {
            if (!brainBody || !bubble)
                return;
            

            if (brainBody.awaitingResponse)
            {
                if (!bubble.activeSelf)
                    bubble.SetActive(true);
            }
            else
            {
                if (bubble.activeSelf)
                    bubble.SetActive(false);
            }
        }
    }
}
