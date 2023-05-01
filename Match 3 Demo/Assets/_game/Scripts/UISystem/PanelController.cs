using System;
using System.Collections.Generic;
using _game.Scripts.Storage;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

[Serializable]
public class PanelTypePair
{
   public PanelType Type;
   public IPanel Panel;
}

public enum PanelType
{
   Register = 0,
   Menu = 1,
   Game = 2,
}
public class PanelController : MonoBehaviour
{

   [SerializeField, TableList(AlwaysExpanded = true)]
   private List<PanelTypePair> m_panels;

   [SerializeField] private Transform m_userLayout;
   [Inject] private IStorage _storage;
   private void Start()
   {
      Show(!_storage.Data.PlayerData.IsRegistered ? PanelType.Register : PanelType.Menu);
   }

   public void Show(PanelType type)
   {
      var panel = m_panels.Find(x => x.Type == type);

      if (panel == null) return;
      
      foreach (var pair in m_panels)
      {
         if (pair.Type == type)
         {
            pair.Panel.Show();
         }
         else
         {
            pair.Panel.Hide();
         }
      }
   }

   public Transform GetUserLayout()
   {
      return m_userLayout;
   }

}
