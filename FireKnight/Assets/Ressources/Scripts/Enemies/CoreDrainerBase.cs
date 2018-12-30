using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Interaction
{
    /////////////////////////////////////////////////
    /// Base of the CoreDrainer, inherits from InteractionBase to deposit energy;
    /////////////////////////////////////////////////
    public class CoreDrainerBase : InteractionBase
    {   

        public byte TryDepositQuantityOfEnergy(byte quantity)
        {
            for (byte i = quantity; i >=1; i--)
            {
                if (!Storage.AddEnergy())
                {
                    return i;
                }
            }
            return 0;
        }

        public byte DrainAllEnergy()
        {
            byte b = Storage.Energy;
            while (Storage.Energy > 0)
            {
                Storage.RemoveEnergy();
            }
            return b;
        }

        public override void OnEnergyChange(byte newEnergy)
        {
        }

        public override string[] GetExtraVisualizationElements()
        {
            return new string[]
            {
                "Core Drainer Base",
                (Storage!=null)? "Linked to " + Storage.name: "UNLINKED"
            };
        }

    }
}
