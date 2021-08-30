using System.Collections;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    // создаем класс Rigidbody, для последующего доступа к нему.
    private Rigidbody playerRb;
    // скорость передвижения Player 500.
    private float playerSpeed = 500;
    // создаем класс GameObject, для последующего доступа к нему в иерархии.
    private GameObject focalPoint;
    // создаем GameObject smokeParticle, который виден в инспекторе, чтобы поместить в Player - префаб Smoke Particle.
    public GameObject smokeParticle;
    // создаем переменную, чтобы определить поднят ли префаб Powerup.
    public bool hasPowerup;
    // создаем GameObject powerupIndicator, который виден в инспекторе, чтобы поместить в Player - префаб Powerup Indicator.
    public GameObject powerupIndicator;
    // Продолжительность усиления.
    public int powerUpDuration = 20;
    // С какой силой Player будет отталкивать префаб Enemy без усиления.
    private float normalStrength = 20;
    // С какой силой Player будет отталкивать префаб Enemy с усилением.
    private float powerupStrength = 40;
    
    void Start()
    {
        // получаем доступ к Rigidbody в Player через компонент Rigidbody GetComponent
        playerRb = GetComponent<Rigidbody>();
        // получаем доступ к focalPoint в иерархии через класс GameObject.Find и имя объекта "Focal Point" 
        focalPoint = GameObject.Find("Focal Point");
    }

    void Update()
    {
        // Input.GetAxis("Vertical"); получаем доступ к менеджеру в инспекторе Project settings - Input Manager - Axes - Vertical - Name (Vertical).
        // теперь видим в инспекторе что значения движения меняются от -1 до 1, но Player не двигается.
        float verticalInput = Input.GetAxis("Vertical");
        // добавляем к Player Rigidbody силу передвижения AddForce,
        // forwardInput (передвижение вверх и вниз) умножаем на playerSpeed скорость передвижения Player. 
        // focalPoint.transform.forward позиция камеры в сторону которой будет передвигаться Player, с одинаковой скоростью на всех пк.
        playerRb.AddForce(focalPoint.transform.forward * verticalInput * playerSpeed * Time.deltaTime);
        // привязываем префаб Powerup Indicator  к координатам Player + опускаем его позицию на -0.6 по оси y.
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);
        // привязываем префаб Smoke Particle к координатам Player + опускаем его позицию на -1 по оси y.
        smokeParticle.transform.position = transform.position + new Vector3(0, -1f, 0);
        // если нажимаем пробел.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // получаем доступ к smokeParticle через компонент ParticleSystem и проигрываем эффект дыма.
            smokeParticle.GetComponent<ParticleSystem>().Play();
            // добавляем к Player Rigidbody силу ускорения в 30 раз, в стороно направления камеры, используем ForceMode.Impulse, чтобы это произошло мгновенно.
            playerRb.AddForce(focalPoint.transform.forward * 30, ForceMode.Impulse);
        }
    }

    // создаем метод взаимодействия boxcollider Player с boxcollider префаба Powerup
    private void OnTriggerEnter(Collider other)
    {
        // если коллайдер Player соприкасается с объектом у которого тэг "Powerup"
        if (other.gameObject.CompareTag("Powerup"))
        {
            // префаб Powerup поднимается Player.
            hasPowerup = true;
            // коллайдер игрового объекта Powerup уничтожается и не виден больше в иерархии.
            Destroy(other.gameObject);
            // запускам курутину, время усиления на 20 секунд, после поднятия усиления.
            StartCoroutine(PowerupCooldown());
            // Powerup Indicator становится активным и виден в иерархии 20 секунд.
            powerupIndicator.SetActive(true);
        }
    }

    // создаем курутину/интерфейс для использования таймера вне метода Update()
    IEnumerator PowerupCooldown()
    {
        // время усиления длится 20 секунд.
        yield return new WaitForSeconds(powerUpDuration);
        // усиление заканчивается через 20 секунд.
        hasPowerup = false;
        // Powerup Indicator становится неактивным и не виден в иерархии через 20 секунд.
        powerupIndicator.SetActive(false);
    }

    // используем OnCollisionEnter за место OnTriggerEnter, так как будем взаимодействовать с физикой (OnTriggerEnter взаимодействует с boxcollider)
    private void OnCollisionEnter(Collision other)
    {
        // если Player соприкасается с префабом Enemy, содержащим тэг "Enemy"
        if (other.gameObject.CompareTag("Enemy"))
        {
            // получаем доступ к Rigidbody Enemy, для последующего взаимодействия с ним Player.
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            // при использовании данной формулы при столкновении с Player, префаб Enemy будет отлетать.
            // other.gameObject.transform.position - transform.position (позиция enemy - позиция player).
            Vector3 awayFromPlayer = other.gameObject.transform.position - transform.position;
            // если у Player есть Powerup
            if (hasPowerup)
            {
                // добавляем к enemyRigidbody силу отталкивания в 40 раз от Player, используем ForceMode.Impulse, чтобы это произошло мгновенно.
                enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
            // Иначе
            else
            {
                // добавляем к enemyRigidbody силу отталкивания в 20 раз от Player, используем ForceMode.Impulse, чтобы это произошло мгновенно.
                enemyRigidbody.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
            }
        }
    }
}
