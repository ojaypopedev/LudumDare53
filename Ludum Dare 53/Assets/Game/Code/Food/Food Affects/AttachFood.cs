using System.Collections;
using System.Linq;
using System.Collections.Generic;
using HotDogCannon.Utils;
using UnityEngine;
namespace HotDogCannon.FoodPrep {
    public class AttachFood : BaseFoodAffect
    {
        public override void OnMerge(FoodObject fromItem, FoodObject toItem)
        {
            fromItem.UnGrab();

            fromItem.DestroyRigidBody();

            fromItem.col.enabled = true;
            fromItem.isMerged = true;

            var startPos = fromItem.transform.position;

            FoodObject topItem = null;

            if (toItem.mergedItems.Count > 0)
                topItem = toItem.mergedItems.Last();
            else
                topItem = toItem;

            toItem.mergedItems.Add(fromItem);

            if (!Application.isPlaying)
            {
                toItem.transform.rotation = Quaternion.Euler(Vector3.zero);
                fromItem.transform.rotation = topItem.mergepos.rotation;
                fromItem.transform.position = topItem.mergepos.position;

                fromItem.transform.SetParent(toItem.mergepos);
            }
            else
            {

                PosAnims.AnimatPos(fromItem.transform, startPos, topItem.mergepos, 0.1f, () =>
                {
                    //Particles Here;

                    if(toItem.rb) toItem.rb.transform.rotation = Quaternion.Euler(Vector3.zero);
                    fromItem.transform.rotation = topItem.mergepos.rotation;
                    fromItem.transform.position = topItem.mergepos.position;

                    fromItem.transform.SetParent(toItem.mergepos);
                });
            }

        }
    }
}
