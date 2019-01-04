using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


namespace Dim
{
    /////////////////////////////////////////////////
    /// Emits sound and activates the SoundMechanic when impacting with something.
    /////////////////////////////////////////////////
    public class ImpactSoundEmitter : MonoBehaviour
    {
        [SerializeField] float impactSoundRange;

        AudioSource source;

        private void Start()
        {
            source = GetComponent<AudioSource>();
        }


		private void OnCollisionEnter(Collision collision)
        {
            SoundMechanicHandler.PlaySound(transform, collision.contacts[0].point, Mathf.Min(10, collision.relativeVelocity.magnitude/4), impactSoundRange,true);
			 source.Play();
        }
       
    }
}

