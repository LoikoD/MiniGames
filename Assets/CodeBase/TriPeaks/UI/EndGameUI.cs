using System;
using TMPro;
using UnityEngine;

namespace CodeBase.TriPeaks.UI
{
    public class EndGameUI : MonoBehaviour, IEndGameUI
    {
        [SerializeField] private TMP_Text _endGameText;

        public event Action NewGameClicked;

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ShowWin()
        {
            _endGameText.text = "You won";
            _endGameText.color = Color.green;
            gameObject.SetActive(true);
        }

        public void ShowLose()
        {
            _endGameText.text = "You lost";
            _endGameText.color = Color.red;
            gameObject.SetActive(true);
        }

        public void NewGame()
        {
            Hide();
            NewGameClicked?.Invoke();
        }
    }
}
