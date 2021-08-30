using UnityEngine;

public class EnemyX : MonoBehaviour
{
    // скорость передвижения префаба Enemy 1.
    private float enemySpeed = 1f;
    // создаем класс Rigidbody, для передвижения объекта и для последующего доступа к нему.
    private Rigidbody enemyRb;
    // создаем класс GameObject, для последующего доступа к нему в иерархии.
    private GameObject player;
    // бонусная скорость Enemy.
    public float bonusSpeed;

    void Start()
    {
        // получаем доступ к Rigidbody в префабе Enemy через компонент Rigidbody GetComponent
        enemyRb = GetComponent<Rigidbody>();
        // получаем доступ к Player в иерархии через класс GameObject.Find и строку "Player" 
        player = GameObject.Find("Player");
    }

    void Update()
    {
        // префаб Enemy двигается вслед за Player.
        // для этого используем позицию Player - позицию Enemy (получаем Vector3 позиции по которой должен двигаться Enemy) * скорость передвижения Enemy.
        // normalized делает так, чтобы Enemy двигался с определенной скоростью, в не зависимости от расстояния до Player.
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        // добавляем силу AddForce, которая будет двигать Enemy в направлении Player.
        enemyRb.AddForce(lookDirection * enemySpeed * bonusSpeed);
    }

    // используем OnCollisionEnter за место OnTriggerEnter, так как будем взаимодействовать с физикой (OnTriggerEnter взаимодействует с boxcollider)
    private void OnCollisionEnter(Collision other)
    {
        // Если префаб Enemy сталкивается с коллайдером объекта имеющим тэг "Enemy Goal"
        if (other.gameObject.name == "Enemy Goal")
        {
            // Enemy уничтожается.
            Destroy(gameObject);
        }
        // Или же префаб Enemy сталкивается с коллайдером объекта имеющим тэг "Player Goal"
        else if (other.gameObject.name == "Player Goal")
        {
            // Enemy уничтожается.
            Destroy(gameObject);
        }
    }
}
