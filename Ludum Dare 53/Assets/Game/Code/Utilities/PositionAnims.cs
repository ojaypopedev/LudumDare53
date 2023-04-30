using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotDogCannon.Utils
{
    public class PositionAnims : MonoBehaviour
    {
        public Transform model;
        public Vector3 startPos;
        public Transform targetPos;
        public float shakeAmount;
        public float time;
        public System.Action callback;

        public PositionAnims(Transform model, Vector3 startPos, Transform targetPos, float time)
        {
            this.model = model;
            this.startPos = startPos;
            this.targetPos = targetPos;
            this.time = time;
        }

        public void StartAnim()
        {
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
            callback?.Invoke();
            Destroy(gameObject);
        }

        public void StartShake()
        {
            StartCoroutine(DoShake());
        }

        float getShakeAmount => Random.Range(-shakeAmount, shakeAmount);

        IEnumerator DoShake()
        {
            float elapsedTime = 0;

            while (elapsedTime < time)
            {
                if (model != null)
                    model.position = startPos + new Vector3(getShakeAmount, getShakeAmount, getShakeAmount);

                elapsedTime += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }

            if(model != null)
                model.position = startPos;
            callback?.Invoke();
            Destroy(gameObject);
        }
    }

    public static class PosAnims
    {

        public static void AnimatPos(Transform model, Vector3 startPos, Transform targetPos, float time, System.Action callback = null)
        {
            var animObj = new GameObject("posAnim");

            var posAnimScript = animObj.AddComponent<PositionAnims>();
            posAnimScript.model = model;
            posAnimScript.startPos = startPos;
            posAnimScript.targetPos = targetPos;
            posAnimScript.time = time;
            posAnimScript.callback = callback;
            posAnimScript.StartAnim();
        }

        public static void AnimateShake(Transform target, Vector3 startPos, float shakeAmount, float time, System.Action callback = null)
        {
            var animObj = new GameObject("shakeAnim");
            var shakeAnimScript = animObj.AddComponent<PositionAnims>();
            shakeAnimScript.model = target;
            shakeAnimScript.startPos = target.position;
            shakeAnimScript.shakeAmount = shakeAmount;
            shakeAnimScript.time = time;
            shakeAnimScript.callback = callback;
            shakeAnimScript.StartShake();
        }

    }
}
