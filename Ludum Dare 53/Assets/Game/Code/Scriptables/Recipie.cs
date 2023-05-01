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
        public Texture2D icon;
        [Tooltip("The order of the list from first to last is the order the recipe should be made in")]
        public List<Ingredient> ingredients = new List<Ingredient>();

        public bool CompareFoodObject(FoodObject toCompare)
        {
            if (ingredients.Count != toCompare.ingredients.Count) return false;

            List<Ingredient> requiredIngredients = new List<Ingredient>();
            requiredIngredients.AddRange(ingredients);

            List<Ingredient> inFood = new List<Ingredient>();
            inFood.AddRange(toCompare.ingredients);
           
            foreach (var requirement in requiredIngredients)
            {
                Ingredient ingredient = inFood.Find(i=>i.foodName == requirement.foodName);

                if(ingredient != null)
                {
                    inFood.Remove(ingredient);
                }
                else
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
