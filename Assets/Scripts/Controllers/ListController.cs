using System;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using Modifiers;
using UnityEngine;

namespace Controllers
{
    public class ListController : MonoBehaviour
    {
        public List<Ability> playersAbilities;

        public ListItem listItem;

        private void Start()
        {
            playersAbilities.ForEach(ability =>
            {
                ability.OnModifierAdded += AddItem;
                ability.OnModifierRemoved += RemoveItem;
            });
        }

        private void UpdateItem(Modifier modifier)
        {
            GetComponentsInChildren<ListItem>()
                .First(item => item.modifier == modifier)
                .barWithIcon.UpdateValue(modifier.Remained);
        }

        private void AddItem(Modifier modifier)
        {
            if (GetComponentsInChildren<ListItem>().Any(item => item.modifier == modifier))
                throw new InvalidOperationException("An element with this modifier already exists");

            var instance = Instantiate(listItem, Vector3.one, Quaternion.identity, gameObject.transform);
            modifier.OnUpdate += UpdateItem;
            instance.modifier = modifier;
            instance.barWithIcon.SetIcon(modifier.Icon);
            instance.barWithIcon.SetHealthBar((int) modifier.Remained, (int) modifier.Duration, true);
        }

        private void RemoveItem(Modifier modifier) =>
            GetComponentsInChildren<ListItem>()
                .Where(item => item.modifier == modifier)
                .Select(i => i.gameObject).ToList()
                .ForEach(Destroy);
    }
}