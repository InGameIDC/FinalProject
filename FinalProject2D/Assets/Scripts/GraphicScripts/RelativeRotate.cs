using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeRotate : MonoBehaviour
{
    private GameObject _rotator;
    [SerializeField] Quaternion defualt;
    [SerializeField] Quaternion upDownRight;
    [SerializeField] Quaternion upDownLeft;
    [SerializeField] Quaternion aa;
    [SerializeField] Quaternion bb;
    [SerializeField] float a;
    [SerializeField] float b;
    [SerializeField] float c;
    private Vector3 _scale;
    private bool isPrevRight;
    // Start is called before the first frame update
    void Start()
    {
        _rotator = transform.parent.parent.gameObject;
        _scale = transform.localScale;
        isPrevRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        float rotation = _rotator.transform.rotation.eulerAngles.z;
        if (rotation % 360 >= 0 && rotation % 360 < 180 && isPrevRight) { // left
            transform.localScale = new Vector3(_scale.x, _scale.y, _scale.z);
            isPrevRight = false;
        }
        else if (rotation % 360 >= 180 && rotation % 360 < 360 &&  !isPrevRight) {
            transform.localScale = new Vector3(_scale.x, _scale.y * -1, _scale.z);
            isPrevRight = true;
        }

    }
}
