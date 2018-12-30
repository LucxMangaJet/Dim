using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dim.Visualize {

    /////////////////////////////////////////////////
    /// Visualization of the Laser Shot by a Laser Turret.
    /////////////////////////////////////////////////
    [RequireComponent(typeof(LineRenderer))]
    public class EnergyLaser : MonoBehaviour {

        [SerializeField] float maxDistance;
        [SerializeField] float elongationTime;
        [SerializeField] float holdTime;

        Vector3 endPosition;
        LineRenderer lr;
        BoxCollider bc;
        Vector3[] points;


        /// missing box collider position, size


        void Start() {

            lr = GetComponent<LineRenderer>();
            bc = GetComponent<BoxCollider>();

            endPosition = GetEndPosition();
            points = new Vector3[]
            {
            transform.position,
            endPosition
            };

            StartCoroutine(StartLaser());
        }

        Vector3 GetEndPosition()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.right, out hit, -maxDistance)){
                return hit.point + transform.right*-3;
            }
            else
            {
                return transform.position + (-maxDistance * transform.right);
            }
        }

        private IEnumerator StartLaser()
        {

            float t = 0;
            while (t < 1)
            {
                points[1] = Vector3.Lerp(transform.position, endPosition, t);
                lr.SetPositions(points);
                float length = Vector3.Distance(points[0], points[1]);
                bc.center = new Vector3(0, 0, length / 2);
                bc.size = new Vector3(bc.size.x,bc.size.y,length);
                yield return null;
                t += Time.deltaTime/elongationTime;
            }

            StartCoroutine(HoldAndEndLaser());
        }

        private IEnumerator HoldAndEndLaser()
        {
            yield return new WaitForSeconds(holdTime);
            Destroy(gameObject);
        }

    }
}