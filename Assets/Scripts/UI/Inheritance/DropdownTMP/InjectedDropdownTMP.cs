using TMPro;
using UnityEngine;
using Zenject;

namespace ToolsACG.UI.Inheritances.DropdownTMP
{
    public class InjectedDropdownTMP : TMP_Dropdown
    {
        #region Fields

        [Header("References")]
        [Inject] private readonly DiContainer _container;

        #endregion

        #region Overrides

        protected override DropdownItem CreateItem(DropdownItem itemTemplate)
        {
            DropdownItem item = base.CreateItem(itemTemplate);
            
            Component[] components = item.GetComponents(typeof(Component));
            foreach (Component component in components)
                _container.Inject(component);

            return item;
        }

        #endregion
    }
}