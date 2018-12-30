using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dim.Visualize
{

    /////////////////////////////////////////////////
    /// Visualization for Energy Detection.
    /////////////////////////////////////////////////
    public class EnergyDetectionVisualization : MonoBehaviour
    {

        public const float TIME_IN_SEC = 1;

        float expansionRate = 0;
        public float ExpansionLeft = 0;
        public float CurrentSize = 0;
        SpriteRenderer sprite;
        bool setup = false;

        public void Setup(float expansion)
        {
            ExpansionLeft = expansion;
            expansionRate = expansion / TIME_IN_SEC;
            CurrentSize = 0;
            sprite = GetComponent<SpriteRenderer>();
            setup = true;
        }

        private void Update()
        {
            if (setup)
            {
                CurrentSize += Time.deltaTime * expansionRate;
                transform.localScale = Vector3.one * CurrentSize * 2;

                ExpansionLeft -= Time.deltaTime * expansionRate;
                if (ExpansionLeft <= 0)
                {
                    Destroy(gameObject);
                }

            }
        }
    }
}
