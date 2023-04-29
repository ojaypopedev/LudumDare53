using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotDogCannon.Utils
{
    public class PosAnim : MonoBehaviour
    {
        public Transform model;
        public Vector3 startPos;
        public Transform targetPos;
        public float time;

        public PosAnim(Transform model, Vector3 startPos, Transform targetPos, float time)
        {
            this.model = model;
            this.startPos = startPos;
            this.targetPos = targetPos;
            this.time = time;
            StartCoroutine(DoAnim());
        }

        IEnumerator DoAnim()
        {
            float elapsedTime = 0;

            while (elapsedTime < time)
            {
                float dt = elapsedTime / time;

                model.position = Vector3.Lerp(startPos, targetPos.position, dt);

                elapsedTime += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }

            model.position = targetPos.position;

            Destroy(gameObject);
        }
    }

    public static class PositionAnims
    {

        public static void AnimatPos(Transform model, Vector3 startPos, Transform targetPos, float time)
        {
            var animObj = new GameObject("posAnim");

            var posAnimScript = animObj.AddComponent<PosAnim>();
            posAnimScript = new PosAnim(model, startPos, targetPos, time);

        }

    }
}
