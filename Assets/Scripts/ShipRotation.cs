using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRotation : MonoBehaviour {
    // Variables
    [SerializeField]
    public float rotationPeriod; //Rotation Period (8seconds/rotation for mars)
    [SerializeField]
    private float rotationAngle;
    private GameObject rotatingObject;

    // Start is called before the first frame update
    void Awake() {
        
    }

    // Update is called once per frame
    void Update() {
        //Rotate
        rotationAngle = Time.deltaTime - rotationPeriod;
        rotatingObject.transform.localRotation = rotationAngle;
    }
}
