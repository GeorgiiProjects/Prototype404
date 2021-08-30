using UnityEngine;

public class RotateCameraX : MonoBehaviour
{
    // создаем переменную для скорости вращения камеры.
    private float rotationSpeed = 200;
    // создаем GameObject player, который виден в инспекторе, чтобы поместить в Focal Point (фокус камеры) - объект Player.
    // делается это для того чтобы камера следовала за Player.
    public GameObject player;

    void Update()
    {
        // Input.GetAxis("Horizontal"); получаем доступ к менеджеру в инспекторе Project settings - Input Manager - Axes - Horizontal - Name (Horizontal)
        // Теперь видим в инспекторе что значения движения меняются от -1 до 1, но камера не двигается.
        float horizontalInput = Input.GetAxis("Horizontal");
        // Контролируем скорость плавного вращения камеры вокруг оси y на любом пк.
        // Заставляем вращаться камеру со скоростью rotationSpeed при нажатии влево и вправо.
        transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
        // Focal Point (фокус камеры) теперь будет двигаться за Player.  
        transform.position = player.transform.position;
    }
}
