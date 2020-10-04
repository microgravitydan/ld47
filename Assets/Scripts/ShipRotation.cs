using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRotation : MonoBehaviour {
    [SerializeField]
    public float rotationPeriod; //Rotation Period (8seconds/rotation for mars)
    [SerializeField]
    private bool xRotAxis, yRotAxis, zRotAxis;

    // Start is called before the first frame update
    void Awake() {
        rotationPeriod = 4/rotationPeriod;
    }

    // Update is called once per frame
    void Update() {
        //Rotate
        if (xRotAxis == true ) {
            transform.Rotate(rotationPeriod, 0, 0, Space.Self);
        } else if (yRotAxis == true) {
            transform.Rotate(0, rotationPeriod, 0, Space.Self);
        } else if (zRotAxis == true) {
            transform.Rotate(0, 0, rotationPeriod, Space.Self);
        } else {
            transform.rotation = Quaternion.identity;
        }
    }
}
