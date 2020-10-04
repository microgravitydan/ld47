using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRotation : MonoBehaviour {
    [SerializeField]
    public float rotationPeriod; //Rotation Period (8seconds/rotation for mars)

    // Start is called before the first frame update
    void Awake() {
    }

    // Update is called once per frame
    void Update() {
        //Rotate
        transform.Rotate(4/rotationPeriod, 0, 0, Space.Self);
    }
}
