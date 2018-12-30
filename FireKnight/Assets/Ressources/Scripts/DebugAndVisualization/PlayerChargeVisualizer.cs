using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Visualize
{

    /////////////////////////////////////////////////
    /// Visualizes the Energy owned by the Player by spawning/destroying the appropriate charge prefab.
    /////////////////////////////////////////////////
    public class PlayerChargeVisualizer : MonoBehaviour
    {

        List<Transform> charges = new List<Transform>();
        [SerializeField] GameObject prefabCharges;
        Transform playerTransform;
        Vector3 localOffset;
        Vector3 lastPosition;


        private void Start()
        {
            playerTransform = LevelHandler.GetPlayer();
            playerTransform.GetComponent<Player.PlayerController>().OnEnergyChange += CatchEnergyChange;

            CatchEnergyChange(playerTransform.GetComponent<Player.PlayerController>().EnergyAmount);

            localOffset = transform.localPosition;
            lastPosition = Vector3.zero;

            transform.parent = null;
        }

        private void Update()
        {
            transform.position = ((playerTransform.position + localOffset) + lastPosition)/2;
            lastPosition = transform.position;
        }

        void CatchEnergyChange(byte e)
        {
            int len = charges.Count;
            int diff = e - len;

            if (diff > 0)
            {
                for (int i = 0; i < diff; i++)
                {
                    AddCharge();
                }
            }
            else
            {
                for (int i = 0; i < Mathf.Abs(diff); i++)
                {
                    RemoveCharge();
                }
            }

        }

        void AddCharge()
        {

            Transform c = Instantiate(prefabCharges, this.transform).transform;
            charges.Add(c);
        }

        void RemoveCharge()
        {
            Transform c = charges[charges.Count - 1];
            if (charges.Count > 0)
            {
                charges.Remove(c);
                Destroy(c.gameObject);

            }
        }
    }
}