using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using HotDogCannon.Utils;

namespace HotDogCannon.FoodPrep
{
    public class BottleSqueeze : BaseFoodAffect
    {
        enum LongestAxis
        {
            X,
            Z
        }
        public System.Action callback;

        public override void OnMerge(FoodObject fromItem, FoodObject toItem)
        {
            FoodObject topItem = null;

            if (toItem.mergedItems.Count > 0)
                topItem = toItem.mergedItems.Last();
            else
                topItem = toItem;

            toItem.transform.rotation = Quaternion.Euler(Vector3.zero);
            fromItem.rb.isKinematic = true;
            fromItem.col.enabled = false;
            var boxSize = toItem.col.bounds;


            Dictionary<LongestAxis, float> sizes = new Dictionary<LongestAxis, float>()
            {
                { LongestAxis.X, boxSize.extents.x},
                { LongestAxis.Z, boxSize.extents.z}
            };

            var longesSide = sizes.OrderBy(v => v.Value).First();

            var pos = toItem.transform.position;
            pos.y = topItem.transform.position.y + .4f;
            var axis = toItem.transform.forward;

            var startPos = pos + axis * -longesSide.Value;
            var endPos = pos + axis * longesSide.Value;


            PosAnims.AnimatPos(fromItem.transform, fromItem.transform.position, startPos, 0.2f, () =>
            {
                fromItem.transform.LookAt(fromItem.transform.position + Vector3.down);

                var result = fromItem.objectToPickupIngredient.SpawnWorldObject();
                fromItem.transform.position = topItem.mergepos.position;
                result.OnSpawn(fromItem.objectToPickupIngredient);
                topItem.Merge(result);

                if (this == null) return;
                PosAnims.AnimatPos(fromItem.transform, startPos, endPos, .5f, () =>
                {
                    if (this == null) return;
                    callback?.Invoke();
                    fromItem.rb.isKinematic = false;
                    fromItem.col.enabled = true;
                    fromItem.rb.AddForce(Vector3.up * Random.Range(1, 4) + Vector3.forward * Random.Range(1,4), ForceMode.Impulse);
                    fromItem.rb.angularVelocity = new Vector3(Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3));
                });
            });

        }
    }
}
