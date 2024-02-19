using UnityEngine;
using UnityEngine.Events;

public class CollisionPointEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent<Vector3> OnCollision;

    private void OnCollisionEnter(Collision collision)
    {
        OnCollision?.Invoke(collision.contacts[0].point);
    }
}