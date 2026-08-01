using UnityEngine;

public class ActivatePhysicsOnTrigger : MonoBehaviour
{
    public Rigidbody[] targetRigidbodies;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (Rigidbody rb in targetRigidbodies)
            {
                if (rb != null)
                {
                    rb.isKinematic = false; 
                    rb.useGravity = true;   
                }
            }

            gameObject.SetActive(false);
        }
    }
}
