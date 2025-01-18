using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private struct ObjectState
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;
        public Rigidbody RigidbodyState;
    }

    private Dictionary<GameObject, ObjectState> initialStates = new Dictionary<GameObject, ObjectState>();

    public List<GameObject> objectsToReset;

    void Start()
    {
        // Capture the initial state of all objects
        foreach (var obj in objectsToReset)
        {
            if (obj != null)
            {
                var state = new ObjectState
                {
                    Position = obj.transform.position,
                    Rotation = obj.transform.rotation,
                    Scale = obj.transform.localScale,
                    RigidbodyState = obj.GetComponent<Rigidbody>()
                };
                initialStates[obj] = state;
            }
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
            var obj = kvp.Key;
            var state = kvp.Value;

            // Reset position, rotation, and scale
            obj.transform.position = state.Position;
            obj.transform.rotation = state.Rotation;
            obj.transform.localScale = state.Scale;

            // Reset physics if Rigidbody is present
            var rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true; // Temporarily disable physics to reset state
                rb.position = state.Position;
                rb.rotation = state.Rotation;
                rb.isKinematic = false; // Re-enable physics
            }
        }
    
        
    }
}
