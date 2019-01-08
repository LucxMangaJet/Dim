using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Visualize
{
    public class IntroUIElementController : MonoBehaviour
    {
        [SerializeField] float maxDist=10;
        [SerializeField] AnimationCurve animationCurve;
        [SerializeField] KeyCode keyCode;
        SpriteRenderer sRenderer;
        Collider sCollider;

        bool faded = false;

        void Start()
        {
            sRenderer = GetComponent<SpriteRenderer>();
            sCollider = GetComponent<Collider>();
        }

        
        void Update()
        {
            if (!faded)
            {
                DistanceBasedAlpha();
                if (sCollider.bounds.Contains(LevelHandler.GetPlayer().position))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        faded = true;
                        StartCoroutine(EndingFade());
                    }
                }
            }
        }

        private IEnumerator EndingFade()
        {
            float counter = 0;
            float TIME = 0.5f;
            float startingAlpha = sRenderer.color.a;
            Color c = sRenderer.color;
            while (counter < TIME)
            {
                c.a = Mathf.Lerp(startingAlpha, 0, counter / TIME);
                c.b = Mathf.Lerp(1, 0, counter / TIME);
                sRenderer.color = c;
                yield return null;
                counter += Time.deltaTime;
            }

            Destroy(gameObject);
        }

        private void DistanceBasedAlpha()
        {
            Color c = sRenderer.color;

            float dist = Vector3.Distance(transform.position, LevelHandler.GetPlayer().position);
            float val = 1;

            val = 1- Mathf.Min(1, dist / maxDist);
            
            c.a = animationCurve.Evaluate(val);
            sRenderer.color = c;
        }
    }
}
