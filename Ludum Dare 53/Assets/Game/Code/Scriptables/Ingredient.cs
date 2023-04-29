using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HotDogCannon.FoodPrep
{
    [CreateAssetMenu(fileName = "Ingredient", menuName = "HotDogCannon/Scirptables/FoodPrep/Ingredient")]
    public class Ingredient : ScriptableObject
    {
        public string foodName;
        public Sprite icon;
        public BaseFoodAffect affect;
        public FoodObject worldObject;

        public FoodObject SpawnWorldObject()
        {
            var foodObj = Instantiate(worldObject);
            foodObj.name = foodName;
            return foodObj;
        }
    }
}
