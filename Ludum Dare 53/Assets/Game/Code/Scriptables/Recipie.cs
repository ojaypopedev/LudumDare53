using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HotDogCannon.FoodPrep
{
    [CreateAssetMenu(fileName = "Ingredient", menuName = "HotDogCannon/Recipie")]
    public class Recipie : ScriptableObject
    {
        public string recipeName;
        public Sprite icon;
        [Tooltip("The order of the list from first to last is the order the recipe should be made in")]
        public List<Ingredient> ingredients = new List<Ingredient>();

        public bool CompareFoodObject(FoodObject toCompare)
        {
            if (ingredients.Count != toCompare.ingredients.Count) return false;

            for (int i = 0; i < ingredients.Count; i++)
            {
                if (ingredients[i].Equals(toCompare.ingredients[i])==false)
                {
                    return false;
                }
            }

            return true;

        }

        [ContextMenu("Create recipe")]
        public GameObject CreateRecipe()
        {
            FoodObject ingredientObject = null;
            FoodObject lastIngredient = null;
            ingredients.ForEach(i =>
            {
                var newIngredientObj = i.SpawnWorldObject();
                newIngredientObj.OnSpawn(i);

                if (lastIngredient != null)
                {
                    lastIngredient.Merge(newIngredientObj);
                }
                else
                {
                    ingredientObject = newIngredientObj;
                }

                lastIngredient = newIngredientObj;
            });

            return ingredientObject.gameObject;
        }
    }
}
