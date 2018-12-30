using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Interaction
{
    /////////////////////////////////////////////////
    /// Base for the RhinoTrain, connects to a Storage to activate/deactivate the RhinoTrain when detecting sound.
    /////////////////////////////////////////////////
    public class RhinoTrainBase : InteractionBase , ISoundMechanicTaker
    {

        [SerializeField] byte threshhold;
        bool active = false;
        public Vector3 target;

        public event System.Action OnSoundHeard;

        public override void Awake()
        {
            target = transform.GetChild(0).position;
            base.Awake();
        }

        private void Start()
        {
            SoundMechanicHandler.AddListener(transform);
        }

        private void OnDestroy()
        {
            SoundMechanicHandler.RemoveListener(transform);
        }

        public void RegisterSoundHeard(SoundHeard sound)
        {
            if (sound.Loudness > 5 && active)
            {
                OnSoundHeard?.Invoke();
            }
        }

        public override void OnEnergyChange(byte newEnergy)
        {
            if(newEnergy >= threshhold)
            {
                active = true;
            }
            else
            {
                active = false;
            }
        }

       
    }
}