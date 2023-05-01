using _game.Scripts;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuController : MonoBehaviour
{
   [SerializeField] private Button m_matchFindButton;

   [Inject] private GameManager _gameManager;
   private void Awake()
   {
      m_matchFindButton.onClick.AddListener(OnMatchFindClicked);
   }

   private void OnMatchFindClicked()
   {
      _gameManager.NakamaConnection.FindMatch();
   }
}
