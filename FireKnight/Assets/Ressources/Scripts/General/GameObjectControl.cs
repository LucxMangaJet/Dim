using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim
{

    /////////////////////////////////////////////////
    /// Interface used to recieve damage.
    /////////////////////////////////////////////////
    public interface IGameObjectDamageTaker
    {
        void TakeDamage();
    }

    public interface ISoundMechanicTaker
    {
        void RegisterSoundHeard(SoundHeard sound);
    }

    /////////////////////////////////////////////////
    /// Interface used by Energy using Elements, used by EnergyHandler to calculate detection.
    /////////////////////////////////////////////////
    public interface IEnergyObject
    {
        float GetEnergyAmount();
        Transform GetTransform();
    }
}
