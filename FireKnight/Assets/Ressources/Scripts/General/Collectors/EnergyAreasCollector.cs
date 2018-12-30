using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dim;

namespace Dim.Collector
{
    /////////////////////////////////////////////////
    /// Sets up the EnergyHandler with information of a new Level at start of every Level.
    /////////////////////////////////////////////////
    public class EnergyAreasCollector : MonoBehaviour
    {


        void Collect()
        {
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            EnergyHandler.SetupLevel(player);
        }


    }
}
