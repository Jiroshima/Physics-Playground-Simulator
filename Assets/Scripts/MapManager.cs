using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private struct ObjectState
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;
    }

    private Dictionary<GameObject, ObjectState> initialStates = new Dictionary<GameObject, ObjectState>();

    [Header("Settings")]
    public Transform parentObject;  // Assign the parent GameObject in the Inspector

    void Start()
    {
        if (parentObject != null)
        {
            // Loop through all children of the parent object
            foreach (Transform child in parentObject)
            {
                GameObject obj = child.gameObject;

                // Store the initial transform state
                var state = new ObjectState
                {
                    Position = obj.transform.position,
                    Rotation = obj.transform.rotation,
                    Scale = obj.transform.localScale
                };

                // Save the object's state in the dictionary
                initialStates[obj] = state;
            }
        }
        else
        {
            Debug.LogWarning("Parent object is not assigned to the MapManager!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetMap();
        }
    }

    public void ResetMap()
    {
        foreach (var kvp in initialStates)
        {
            GameObject obj = kvp.Key;
            ObjectState state = kvp.Value;

            // Reset position, rotation, and scale
            obj.transform.position = state.Position;
            obj.transform.rotation = state.Rotation;
            obj.transform.localScale = state.Scale;

            // Reset physics if a Rigidbody is present
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;  // Use linearVelocity instead of velocity
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;  // Temporarily disable physics
                rb.position = state.Position;
                rb.rotation = state.Rotation;
                rb.isKinematic = false; // Re-enable physics
            }
        }

        Debug.Log("Map has been reset.");
    }
}
