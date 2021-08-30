using UnityEngine;

public class SpawnManagerX : MonoBehaviour
{
    // создаем GameObject enemyPrefab, для чтобы поместить в него префаб Enemy в инспекторе.
    public GameObject enemyPrefab;
    // создаем GameObject powerupPrefab, для чтобы поместить в него префаб PowerUp в инспекторе.
    public GameObject powerupPrefab;
    // создаем GameObject player, для чтобы поместить в него player в инспекторе.
    public GameObject player;
    // создаем переменную для рандомного спавна префаба Enemy по оси x от -10 до 10.
    private float spawnRangeX = 10;
    // минимальная позиция спавна префаба Enemy по оси z 15.
    private float spawnZMin = 15;
    // максимальная позиция спавна префаба Enemy по оси z 25.
    private float spawnZMax = 25;
    // создаем переменую для отслеживания количества врагов на экране.
    public int enemyCount;
    // создаем переменную для подсчета номера волны.
    public int waveCount = 1;

    void Update()
    {
        // Получаем доступ к игровому объекту с тэгом "Enemy" и ищем количество всех врагов на экране через массив.
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        // Если количество врагов равно 0
        if (enemyCount == 0)
        {
            // получаем доступ к скрипту EnemyX через префаб Enemy и привабляем 1 бонусной скорости.
            enemyPrefab.GetComponent<EnemyX>().bonusSpeed += 1f;
            // Вызываем метод SpawnEnemyWave() в методе Update(), иначе следующая волна префабов Enemy не будут появляться.
            SpawnEnemyWave(waveCount);
        }
    }

    // Используем метод GenerateSpawnPosition() для рандомного спавна мобов и усилений по оcям x,y,z.
    Vector3 GenerateSpawnPosition()
    {
        // переменная по оси х с рандомным спавном от -10 до 10
        float xPos = Random.Range(-spawnRangeX, spawnRangeX);
        // переменная по оси z с рандомным спавном от 15 до 25
        float zPos = Random.Range(spawnZMin, spawnZMax);
        // можем использовать случайную генерацию префабов Enemy и PowerUp в игре, по оси y значение остается неизменным.
        return new Vector3(xPos, 0, zPos);
    }

    // создаем метод SpawnEnemyWave() с параметром int enemiesTospawn, чтобы спавнить в нем нужное количество префабов Enemy в каждой волне.
    void SpawnEnemyWave(int enemiesToSpawn)
    {
        // префаб Powerup смещается при спавне по оси z относительно Player.
        Vector3 powerupSpawnOffset = new Vector3(0, 0, -15);
        // Если объектов с тэгом Powerup == 0.
        if (GameObject.FindGameObjectsWithTag("Powerup").Length == 0)
        {
            // Instantiate создаем клоны powerup, в позиции координат new Vector3 используем метод рандомного спавна GenerateSpawnPosition(), 
            // powerupPrefab.transform.rotation поворот оставляем по умолчанию.
            // GenerateSpawnPosition() + powerupSpawnOffset используем рандомную генерацию и плюсуем к ней -15 по оси z.
            Instantiate(powerupPrefab, GenerateSpawnPosition() + powerupSpawnOffset, powerupPrefab.transform.rotation);
        }

        // используем цикл for для появления нужного количества префабов Enemy в каждой волне, 
        // в нашем случае от одного в первой волне и увеличиваем на 1 в последующих волнах.
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // Instantiate создаем клоны префаба Enemy, в позиции координат new Vector3 используем метод рандомного спавна GenerateSpawnPosition(), 
            // enemyPrefab.transform.rotation поворот оставляем по умолчанию.
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }
        // количество врагов с каждой волной увеличивается на один.
        waveCount++;
        // Возвращаем Player в стартовую позицию при новой волне.
        ResetPlayerPosition();
    }

    // Метод для сброса позиции Player при следующей волне.
    void ResetPlayerPosition()
    {
        // Позиция Player при следующей волне по осям x,y,z.
        player.transform.position = new Vector3(0, 1, -7);
        // получаем доступ к Rigidbody в player через компонент Rigidbody GetComponent, скорость Player при следующей волне 0.
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        // получаем доступ к Rigidbody в player через компонент Rigidbody GetComponent, скорость поворота Player при следующей волне 0.
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
