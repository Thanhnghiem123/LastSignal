using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera camera;

    private float xRotation = 0f;
    private float yRotation = 0f; // Thêm biến để lưu góc xoay trục Y
    private float xSensitivity = 30f;
    private float ySensitivity = 30f;

    void Update()
    {
        // Lấy input từ chuột
        Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        ProcessLook(mouseInput);
    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        // Xoay lên/xuống (trục X của camera)
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        // Xoay trái/phải (trục Y của camera)
        yRotation += (mouseX * Time.deltaTime) * xSensitivity;

        // Áp dụng cả hai góc xoay cho camera
        camera.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}