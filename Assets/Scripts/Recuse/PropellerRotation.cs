using UnityEngine;

public class PropellerRotation : MonoBehaviour
{
    // Trục quay, cho phép người dùng chọn trong Inspector
    public enum Axis { X, Y, Z }
    public Axis RotateAxis = Axis.Z; // Trục mặc định là Z

    [SerializeField] private float rotationSpeed = 500f; // Tốc độ quay (độ/giây)

    void Update()
    {
        // Xác định vector quay dựa trên trục được chọn
        Vector3 rotationVector = Vector3.zero;
        switch (RotateAxis)
        {
            case Axis.X:
                rotationVector = Vector3.right; // Trục X
                break;
            case Axis.Y:
                rotationVector = Vector3.up; // Trục Y
                break;
            case Axis.Z:
                rotationVector = Vector3.forward; // Trục Z
                break;
        }

        // Quay cánh quạt quanh trục được chọn
        transform.Rotate(rotationVector * rotationSpeed * Time.deltaTime);
    }
}