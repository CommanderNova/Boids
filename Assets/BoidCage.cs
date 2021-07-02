using UnityEngine;

public class BoidCage : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        var extents = gameObject.GetComponent<BoxCollider>().extents;

        var otherTransform = other.gameObject.transform;
        var newPosition = otherTransform.position;

        if (newPosition.x > extents.x || newPosition.x < -extents.x)
            newPosition.x = -newPosition.x;
        else if (newPosition.y > extents.y || newPosition.y < -extents.y)
            newPosition.y = -newPosition.y;
        else if (newPosition.z > extents.z || newPosition.z < -extents.z)
            newPosition.z = -newPosition.z;

        otherTransform.position = newPosition;
    }

    private void OnDrawGizmos()
    {
        var extents = gameObject.GetComponent<BoxCollider>().extents;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, extents * 2);
    }
}
