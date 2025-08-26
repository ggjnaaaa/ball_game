using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//~~~~~~    ������ ���������� �������    ~~~~~~//
public class CameraScript : MonoBehaviour
{
    void Update()
    {
        int moveSpeed = 10;
        int angSpeed = 40;
        float mouse_sens = 3f;

        float vert = Input.GetAxisRaw("Vertical");
        float hor = Input.GetAxisRaw("Horizontal");

        //~~~~~~    ��������� ������� ������    ~~~~~~//
        Vector3 dir = new Vector3(hor, 0, vert);
        dir.Normalize();

        dir = transform.TransformDirection(dir) * Time.fixedDeltaTime * moveSpeed;
        transform.position += dir;

        //~~~~~~    ��������� �������� ������ ��� ������� �� ���    ~~~~~~//
        if (Input.GetAxis("Fire2") == 1)
        {
            float x_axis = Input.GetAxis("Mouse X") * mouse_sens;
            float y_axis = Input.GetAxis("Mouse Y") * mouse_sens;
            transform.Rotate(Vector3.up, x_axis, Space.World);
            transform.Rotate(Vector3.right, y_axis, Space.Self);
        }

        //~~~~~~    ��������� �������� ������ ��� ������� �� Q � E    ~~~~~~//
        if (Input.GetKey(KeyCode.Q) == true)
        {
            transform.Rotate(new Vector3(0, 1, 0), -angSpeed * Time.fixedDeltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.E) == true)
        {
            transform.Rotate(new Vector3(0, 1, 0), angSpeed * Time.fixedDeltaTime, Space.World);
        }
    }
}
