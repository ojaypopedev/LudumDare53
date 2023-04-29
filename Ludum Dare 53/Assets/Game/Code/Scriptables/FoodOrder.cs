using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HotDogCannon.FoodPrep
{
    public class FoodOrder : ScriptableObject
    {
        public string recipeName;
        public Sprite icon;
        [Tooltip("The order of the list from first to last is the order the recipe should be made in")]
        public List<Ingredient> ingredients = new List<Ingredient>();

        public bool CompareFoodObject(FoodObject toCompare)
        {
            return false;
        }

    }
}
