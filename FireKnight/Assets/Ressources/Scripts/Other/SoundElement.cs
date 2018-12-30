using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Visualize
{

    /////////////////////////////////////////////////
    /// Visual rapresentation of Sound.
    /////////////////////////////////////////////////
    public class SoundElement : MonoBehaviour
    {
        public const float SPEED_MULTIPLYER = 15;
        public const float MAX_LOUNDNESS = 10;
        public const float FIXED_SIZE = 5;
        public float Loudness = 0;
        public float ExpansionLeft = 0;
        public float CurrentSize = 0;
        SpriteRenderer sprite;
        bool setup = false;
        float startingExpansion;

        public void Setup(float loudness, float expansion)
        {


            Loudness = loudness;
            ExpansionLeft = expansion;
            startingExpansion = expansion;
            CurrentSize = 0;
            sprite = GetComponent<SpriteRenderer>();
            setup = true;
        }

        public void Setup(float loudness)
        {
            Loudness = loudness;
            ExpansionLeft = FIXED_SIZE;
            startingExpansion = FIXED_SIZE;
            CurrentSize = 0;
            sprite = GetComponent<SpriteRenderer>();
            setup = true;
        }



        private void Update()
        {
            if (setup)
            {
                CurrentSize += Time.deltaTime * SPEED_MULTIPLYER;
                transform.localScale = Vector3.one * CurrentSize * 2;

                ExpansionLeft -= Time.deltaTime * SPEED_MULTIPLYER;
                if (ExpansionLeft <= 0)
                {
                    Destroy(gameObject);
                }

                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, (ExpansionLeft / startingExpansion));
            }
        }


    }
}
    
namespace Dim { 
    /////////////////////////////////////////////////
    /// Class passed by the SouundMechanicHandler when calling a Listener.
    /////////////////////////////////////////////////
    public class SoundHeard
    {
        public readonly float Loudness;
        public readonly Vector3 Origin;
        public readonly float TimeStamp;

        public SoundHeard(float loudness , Vector3 origin)
        {
            Loudness = loudness;
            Origin = origin;
            TimeStamp = Time.time;
        }


        public static bool operator >(SoundHeard a, SoundHeard b)
        {


            if (a.Loudness > b.Loudness)
            {
                return true;
            }
            else if (a.Loudness == b.Loudness)
            {
                if (a.TimeStamp > b.TimeStamp)
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }

        }

        public static bool operator <(SoundHeard a, SoundHeard b)
        {


            if (a.Loudness < b.Loudness)
            {
                return true;
            }
            else if (a.Loudness == b.Loudness)
            {
                if (a.TimeStamp < b.TimeStamp)
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}
