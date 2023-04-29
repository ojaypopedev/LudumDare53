using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static HotDogCannon.FoodPrep.FoodObject;

namespace HotDogCannon.FoodPrep
{
    [CreateAssetMenu(fileName = "Ingredient", menuName = "HotDogCannon/Scirptables/FoodPrep/Ingredient")]
    public class Ingredient : ScriptableObject
    {
        public string foodName;
        public Sprite icon;

        public enum FoodAffectType
        {
            ATTACH,
        }

        public FoodAffectType mergeAffectBehaviour;

        public FoodObject worldObject;

        public FoodObject SpawnWorldObject()
        {
            var foodObj = Instantiate(worldObject);
            foodObj.name = foodName;
            return foodObj;
        }
        public override bool Equals(object other)
        {
            var otherFood = other as Ingredient;

            if (otherFood == null) return false;

            return otherFood.foodName == foodName;
        }
    }
}
