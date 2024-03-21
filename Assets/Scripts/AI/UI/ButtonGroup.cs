using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


namespace Sim.UI
{
    public class ButtonGroup : MonoBehaviour
    {
        [System.Serializable]
        public struct ButtonGameObjectPair
        {
            public Button button;
            public GameObject associatedObject;
        }

        [SerializeField] private bool useInitialButton = false; // Enable or disable initial button feature
        [SerializeField] private int initialButtonIndex = 0; // Index of the initial button

        public Color activeColor = Color.blue; // Color for the selected button
        public Color inactiveColor = Color.black; // Color for the unselected buttons

        public List<ButtonGameObjectPair> buttonGameObjectPairs; // Assign in inspector
        private Button activeButton = null; // Currently active (selected) button


        void Start()
        {
            // Initialize button colors and add click listeners
            foreach (var pair in buttonGameObjectPairs)
            {
                SetButtonColor(pair.button, inactiveColor);
                pair.button?.onClick.AddListener(() => OnButtonClicked(pair.button, pair.associatedObject));
                pair.associatedObject?.SetActive(false); // Initially disable all associated GameObjects
            }


            // Set initial button if the feature is enabled
            if (useInitialButton && initialButtonIndex >= 0 && initialButtonIndex < buttonGameObjectPairs.Count)
            {
                var initialPair = buttonGameObjectPairs[initialButtonIndex];
                OnButtonClicked(initialPair.button, initialPair.associatedObject);
            }
            else
            {
                DeselectButttons();
            }
        }

        void OnButtonClicked(Button clickedButton, GameObject associatedObject)
        {
            if (clickedButton == activeButton)
            {
                // Toggle off the active button
                SetButtonColor(clickedButton, inactiveColor);
                associatedObject?.SetActive(false); // Disable the associated GameObject
                activeButton = null;
            }
            else
            {
                // Toggle off all buttons and set their color to inactive
                foreach (var pair in buttonGameObjectPairs)
                {
                    SetButtonColor(pair.button, inactiveColor);
                    pair.associatedObject?.SetActive(false); // Disable all associated GameObjects
                }

                // Toggle on the clicked button, set it as active and change its color
                SetButtonColor(clickedButton, activeColor);
                associatedObject?.SetActive(true); // Enable the associated GameObject
                activeButton = clickedButton;
            }
        }

        public void DeselectButttons()
        {
            // Toggle off all buttons and set their color to inactive
            foreach (var pair in buttonGameObjectPairs)
            {
                SetButtonColor(pair.button, inactiveColor);
                pair.associatedObject?.SetActive(false); // Disable all associated GameObjects
                activeButton = null;
            }
        }

        private void SetButtonColor(Button button, Color color)
        {
            button.targetGraphic.color = color;
        }
    }
}
