using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target a seguir")]
    public Transform target; // Asigná aquí el jugador en el inspector

    [Header("Ajustes de camara")]
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 1, -10); // Podés darle un offset (ejemplo: (0,1,-10))

    void LateUpdate()
    {
        if (target == null) return;

        // Posición deseada (jugador + offset)
        //Vector3 desiredPosition = target.position + offset;
        Vector3 desiredPosition = new Vector3(target.position.x, transform.position.y, transform.position.z);

        // Movimiento suave
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Actualizar posición de la cámara
        transform.position = smoothedPosition;
    }
}