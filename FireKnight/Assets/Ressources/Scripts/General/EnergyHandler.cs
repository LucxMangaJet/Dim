using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim
{
    /////////////////////////////////////////////////
    /// Singleton, stores info about EnergyObjects  and deals with Energy Detection.
    /////////////////////////////////////////////////
    public class EnergyHandler
    {
        public static EnergyHandler instance;

        private List<IEnergyObject> energyObjects;


        public EnergyHandler()
        {
            energyObjects = new List<IEnergyObject>();
        }

        public static void Initiate()
        {
            instance = new EnergyHandler();
        }

        public static List<IEnergyObject> GetEnergyObjects()
        {
            return instance.energyObjects;
        }

        public static void SetupLevel(Transform player)
        {
            instance.energyObjects = new List<IEnergyObject>();
        }

        public static void AddEnergyObject(IEnergyObject energyObject )
        {
            instance.energyObjects.Add(energyObject);
        }

        public static void RemoveEnergyObject(IEnergyObject energyObject)
        {
            instance.energyObjects.Remove(energyObject);
        }



        public static EnergyArea CreateEnergyArea(Vector3 position)
        {
            GameObject go =  GameObject.Instantiate(PrefabHolder.EnergyArea(), position, Quaternion.identity);
            EnergyArea ha = go.GetComponent<EnergyArea>();
            AddEnergyObject(ha);
            return ha;

        }

        

        public static bool GetStrongestEnergyInCone(Vector3 pos, Vector3 dir, float range, float baseRadius, out Transform strongest)
        {
            List<IEnergyObject> energyInCone = new List<IEnergyObject>();
            
           
            //check all EnergyAreas
            for (int i = 0; i < instance.energyObjects.Count; i++)
            {

                Vector3 energyPos = instance.energyObjects[i].GetTransform().position;

                if (IsEnergyInCone(pos, energyPos, dir, range, baseRadius))
                {
                    energyInCone.Add(instance.energyObjects[i]);
                }
            }

            strongest = GetTransformOfStrongestEnergy(energyInCone);

            if(strongest != null)
            {
                return true;
            }

            return false;
        }

        private static bool IsEnergyInCone(Vector3 conePos,Vector3 energyPos, Vector3 dir, float range, float baseRadius)
        {
            //https://stackoverflow.com/questions/12826117/how-can-i-detect-if-a-point-is-inside-a-cone-or-not-in-3d-space

            float coneDist = Vector3.Dot(energyPos - conePos, dir);

            if (coneDist < 0 || coneDist > range)
            {
                return false;
            }

            float radiusAtPoint = (coneDist / range) * baseRadius;

            float orthogonalDistance = Vector3.Magnitude((energyPos - conePos) - coneDist * dir);

            if (orthogonalDistance < radiusAtPoint)
            {
                return true;
            }

            return false;
        }

        private static Transform GetTransformOfStrongestEnergy( List<IEnergyObject> energyObjectList)
        {
            Transform t = null;
            float maxStrength = -1;

            for (int i = 0; i < energyObjectList.Count; i++)
            {
                float currentStrength = energyObjectList[i].GetEnergyAmount();
                if (currentStrength > maxStrength)
                {
                    t = energyObjectList[i].GetTransform();
                    maxStrength = currentStrength;
                }
            }

            return t;
        }


        public static bool IsTransformInEnergyArea(Transform t, out EnergyArea heatArea)
        {
            heatArea = null;

            for (int i = 0; i < instance.energyObjects.Count; i++)
            {
                IEnergyObject current = instance.energyObjects[i];

                if (current is EnergyArea)
                {
                    EnergyArea area = current as EnergyArea;
                    if (Vector3.Distance(t.position, instance.energyObjects[i].GetTransform().position) < area.Size)
                    {
                        heatArea = area;
                        return true;
                    }
                }
            }

            return false;
        }

        

    }

  
}


