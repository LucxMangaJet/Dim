using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Interaction
{
    /////////////////////////////////////////////////
    /// Storage Behavior, can be linked to other Storages to have synced Energy Levels.
    /////////////////////////////////////////////////
    public class Storage : MonoBehaviour, Visualize.IExtraVisualization
    {
        public bool debug;

        public byte MaxEnergy;
        public byte StartingEnergy;
        private byte energy;

        public Storage[] LinkedStorages;

        public event EnergyChange OnEnergyChange;

        protected bool playerInArea;

        public byte Energy
        {
            get
            {
                return energy;
            }

            set
            {
                energy = value;
                OnEnergyChange?.Invoke(energy);
            }
        }

        protected void Start()
        {
            Energy = StartingEnergy;

            if (LinkedStorages != null)
            {
                foreach (var storage in LinkedStorages)
                {
                    if (storage.StartingEnergy != StartingEnergy || storage.MaxEnergy != MaxEnergy)
                    {
                        Debug.LogError(gameObject.name + " and at least 1 of its liked Storages do not have the same settings!");
                        return;
                    }
                }
            }
        }

        public bool AddEnergy()
        {
            if (Energy < MaxEnergy)
            {
                Energy+=1;
                UpdateLinkedStorages();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RemoveEnergy()
        {
            if (Energy > 0)
            {
                Energy-=1;
                UpdateLinkedStorages();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HasSpace()
        {
            return Energy < MaxEnergy;
        }

        public bool IsEmpty()
        {
            return Energy < 1;
        }
       
        private void UpdateLinkedStorages()
        {
            if (LinkedStorages != null)
            {
                foreach (var st in LinkedStorages)
                {
                    st.Energy = Energy;
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (debug)
            {
                if (LinkedStorages != null)
                {
                    foreach (var item in LinkedStorages)
                    {
                        Gizmos.DrawLine(transform.position, item.transform.position);
                    }
                }
                
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                playerInArea = true;
                other.GetComponent<Player.PlayerController>().currentStorage = this;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                playerInArea = false;
                other.GetComponent<Player.PlayerController>().currentStorage = null;
            }
        }

        public string[] GetExtraVisualizationElements()
        {
            return new string[]
            {
                "Current Energy: " + Energy + "/" + MaxEnergy,
                gameObject.name,
                (playerInArea)?"Player in use Area":"",
            };
        }

    }

    public delegate void EnergyChange(byte NewEnergy);

}