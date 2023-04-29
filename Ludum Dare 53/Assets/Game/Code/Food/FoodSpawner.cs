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

        public float regenTime = 0.4f;

        List<FoodObject> foodObjects = new List<FoodObject>();


        private void Awake()
        {
            GameManager.onReset += OnReset;
        }

        public void SpawnItem()
        {
            var newitem = ingredient.SpawnWorldObject();
            newitem.transform.position = spawnPos.position;
            newitem.LinkSpawner(this);
            newitem.OnSpawn(ingredient);
            newitem.onGrabbed += OnItemGrabbed;
            foodObjects.Add(newitem);
        }

        public void OnReset()
        {
            foodObjects.ForEach(f => Destroy(f.gameObject));
            foodObjects.Clear();
            StartCoroutine(StartSpawn());
           
        }

        public void OnItemGrabbed(FoodObject foodObject)
        {
            foodObjects.Remove(foodObject);
            foodObject.onGrabbed -= OnItemGrabbed;
            SpawnItem();
        }

        IEnumerator StartSpawn()
        {
            while (foodObjects.Count < spawnAmount)
            {
                SpawnItem();
                yield return new WaitForSeconds(regenTime);
            }
        }
    }
}
