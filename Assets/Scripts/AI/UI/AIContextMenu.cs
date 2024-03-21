using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI.UI
{
    public class AIContextMenu : MonoBehaviour
    {
        public enum MenuState
        {
            Mini,
            Open
        }

        public Context<MenuState> state;

        public Panel miniContent;
        public Panel openContent;

        public MenuState State { get; private set; }

        private void OnEnable()
        {
            Init();
        }

        public void Init()
        {
            ChangeState(MenuState.Mini);
        }


        public void Minimize()
        {
            openContent.DeInit();
        }

        public void Open()
        {
            openContent.Init();
        }

        public void ChangeState(MenuState newState)
        {
            switch (newState)
            {
                case MenuState.Mini:
                    Minimize();
                    break;
                case MenuState.Open:
                    Open();
                    break;
            }

            State = newState;
        }

        public void ToggleState()
        {
            if (State == MenuState.Mini) { ChangeState(MenuState.Open); }
            else { ChangeState(MenuState.Mini); }
        }



    }
}
