using UnityEngine;

public class WalkingEnemy : MonoBehaviour
{
    public float speed = 7f;
    private float direction = -1f;
    private float changeDirectionInterval = 2f;  // Время перед сменой направления
    private float timeSinceLastChange = 0f;

    private Transform player;  // Ссылка на игрока
    private AudioSource audioSource;  // Аудио компонент

    public float maxHearingDistance = 10f; // Максимальная дистанция, на которой слышен звук

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Найти игрока по тегу "Player" (убедитесь, что у игрока стоит этот тег)
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        // Включаем звук, если есть аудиофайл
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.loop = true;  // Делаем звук цикличным
            audioSource.Play();
        }
    }

    private void Update()
    {
        timeSinceLastChange += Time.deltaTime;

        if (timeSinceLastChange >= changeDirectionInterval)
        {
            direction *= -1;  // Меняем направление движения
            timeSinceLastChange = 0f;
        }

        // Двигаем врага
        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(speed * direction, GetComponent<Rigidbody2D>().linearVelocity.y);

        // Обновляем громкость звука в зависимости от расстояния до игрока
        UpdateSoundVolume();
    }

    private void UpdateSoundVolume()
    {
        if (audioSource != null && player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            float volume = Mathf.Clamp01(1 - (distance / maxHearingDistance)); // Чем ближе, тем громче
            audioSource.volume = volume;
        }
    }
}
