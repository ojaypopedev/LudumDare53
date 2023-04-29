using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotDogCannon.FoodPrep
{
    public class FoodSpawner : MonoBehaviour
    {
        [Header("Setup")]
        public Ingredient ingredient;

        [Header("Spawn Settings")]
        public Transform spawnPos;
        public int spawnAmount;

        List<FoodObject> foodObjects = new List<FoodObject>();

        public void SpawnItem()
        {
            var newitem = ingredient.SpawnWorldObject();
            newitem.LinkSpawner(this);
            foodObjects.Add(newitem);
        }

        public void Init()
        {
            foodObjects.ForEach(f => Destroy(f.gameObject));
            foodObjects.Clear();
            StartCoroutine(StartSpawn());
           
        }

        IEnumerator StartSpawn()
        {
            while (foodObjects.Count < spawnAmount)
            {
                SpawnItem();
                yield return new WaitForSeconds(1);
            }
        }
    }
}
