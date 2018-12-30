using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Visualize { 

    ////////////////////
    /// Visualizes the Energy on the CoreDrainer.
    ///////////////////
public class CoreDrainerChargeVisualizer : MonoBehaviour {

    List<Transform> charges = new List<Transform>();
    [SerializeField] GameObject prefabCharges;
    Transform drainerTransform;
    Vector3 localOffset;
    Vector3 lastPosition;


    private void Start()
    {
        drainerTransform = transform.parent;
        
        drainerTransform.GetComponent<Enemies.CoreDrainerController>().OnEnergyChange += CatchEnergyChange;

        CatchEnergyChange(drainerTransform.GetComponent<Enemies.CoreDrainerController>().Energy);

        localOffset = transform.localPosition;
        lastPosition = Vector3.zero;

        transform.parent = null;
    }

    private void Update()
    {
        transform.position = ((drainerTransform.position + localOffset) + lastPosition) / 2;
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
