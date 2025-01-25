using UnityEngine;
using UnityEngine.Rendering;

public class AirResistanceTest : MonoBehaviour
{
    public bool inWindZone = false;
    public GameObject windZone;
    
    Rigidbody rb; 

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        if(inWindZone){
            rb.AddForce(windZone.GetComponent<FanAirFlow>().direction * windZone.GetComponent<FanAirFlow>().strength);
        }
    }

    void OnTriggerEnter(Collider coll){
        if(coll.gameObject.tag =="windArea"){
            windZone = coll.gameObject;
            inWindZone = true;
        }
    }
    
    void OnTriggerExit(Collider coll) {
        if(coll.gameObject.tag =="windArea") {
            inWindZone = false;
        }
    }

}