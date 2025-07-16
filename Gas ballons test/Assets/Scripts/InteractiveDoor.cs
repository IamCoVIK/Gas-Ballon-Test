using UnityEngine;
using Valve.VR.InteractionSystem;

public class InteractiveDoor : MonoBehaviour
{
    public float rotationSpeed = 90f;   // Скорость вращения двери (градусов в секунду)
    public float maxRotation = 90f;      // Максимальный угол поворота двери
    public Vector3 hingeAxis = Vector3.up; // Ось вращения двери (по умолчанию - вертикальная)

    private float currentRotation = 0f;  // Текущий угол поворота двери
    private bool isRotating = false;       // Флаг, указывающий, вращается ли дверь
    private float targetRotation = 0f;      // Целевой угол поворота

    public void StartRotating(float direction)
    {
        targetRotation += direction * maxRotation;
        targetRotation = Mathf.Clamp(targetRotation, 0, maxRotation);
        isRotating = true;
    }

    public void StopRotating()
    {
        isRotating = false;
    }

    void Update()
    {
        if (isRotating)
        {
            float step = rotationSpeed * Time.deltaTime;
            currentRotation = Mathf.MoveTowards(currentRotation, targetRotation, step);
            transform.localRotation = Quaternion.AngleAxis(currentRotation, hingeAxis);
        }
    }
}
