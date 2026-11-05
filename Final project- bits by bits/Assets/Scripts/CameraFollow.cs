using UnityEngine;

public class CameraFollow : MonoBehaviour
{
   private Vector3 offset;
   private float damping;

   public Transform target;

   private Vector3 velocity = Vector3.zero;

   private void FixedUpdate()
   {
       Vector3 targetPosition = target.position + offset;
       targetPosition.z = transform.position.z;
       
       transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, damping);
   }
}
