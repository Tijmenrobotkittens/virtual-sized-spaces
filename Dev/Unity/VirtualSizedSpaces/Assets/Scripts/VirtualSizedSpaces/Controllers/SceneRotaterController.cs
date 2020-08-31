using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneRotaterController : MonoBehaviour
{
    private GameObject _user;
    private GameObject _stage;
    private float _threshold = 3;
    public SceneRotaterController(GameObject user, GameObject stage) {
        _user = user;
        _stage = stage;

    }

    public void setRotation(Quaternion rotate, Quaternion lastrotate) {
        Vector3 movement = rotate.eulerAngles - lastrotate.eulerAngles;
        float totalmovement = Mathf.Abs(movement.x) + Mathf.Abs(movement.y) + Mathf.Abs(movement.z);
        float move = 0f;
        if (totalmovement > _threshold) {
            move = totalmovement - _threshold;
            Debug.Log("moving "+move);
        }
    }
}