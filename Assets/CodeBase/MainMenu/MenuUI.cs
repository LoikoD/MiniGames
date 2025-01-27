using System;
using UnityEditor;
using UnityEngine;

namespace CodeBase.MainMenu
{
    public class MenuUI : MonoBehaviour, IMenuUI
    {
        public event Action StartClicker;
        public event Action StartTriPeaks;

        public void PlayClicker()
        {
            StartClicker?.Invoke();
        }
        public void PlayTriPeaks()
        {
            StartTriPeaks?.Invoke();
        }
        public void QuitGame()
        {
            #if UNITY_EDITOR
                EditorApplication.ExitPlaymode();
            #else
                Application.Quit();
            #endif
        }
    }
}