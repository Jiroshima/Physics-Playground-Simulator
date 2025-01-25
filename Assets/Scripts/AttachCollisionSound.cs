using UnityEngine;

public class AttachCollisionSound : MonoBehaviour
{
    [SerializeField] 
    private AudioClip collisionSound;  // Sound to play on collision for all children

    private void Start()
    {
        // Get the parent GameObject (MapObjects) that contains all of the objects
        GameObject mapObjectsParent = this.gameObject;  // Assuming this script is attached to the MapObjects parent object

        // Check if the parent object exists
        if (mapObjectsParent != null)
        {
            // Loop through all child objects of the MapObjects parent
            foreach (Transform child in mapObjectsParent.transform)
            {
                // Check if the child has a Collider component
                if (child.GetComponent<Collider>() != null)
                {
                    // Add the CollisionSound script to the child object if not already added
                    CollisionSound collisionSoundScript = child.GetComponent<CollisionSound>();
                    if (collisionSoundScript == null)
                    {
                        collisionSoundScript = child.gameObject.AddComponent<CollisionSound>();
                    }

                    // Assign the collision sound to the CollisionSound script
                    collisionSoundScript.SetCollisionSound(collisionSound);
                }
            }
        }
    }
}
