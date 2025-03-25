using UnityEngine;
using UnityEngine.UI;  // Для работы с UI
using UnityEngine.SceneManagement;  // Для смены сцен
using System.Collections;  // Для использования IEnumerator

public class LevelCompletion : MonoBehaviour
{
    public Text messageText;  // Ссылка на текстовое поле UI
    public float messageDuration = 3f;  // Время, через которое сообщение исчезнет
    private bool levelCompleted = false;

    // Метод для обработки столкновений
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !levelCompleted)
        {
            levelCompleted = true;

            // Воспроизведение звука при столкновении
            SoundEffectManager.Play("LevelComplete");

            // Получаем индекс текущей сцены
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            // Загружаем следующую сцену с небольшой задержкой, чтобы звук успел проиграться
            StartCoroutine(LoadNextScene(currentSceneIndex + 1, 1.5f));
        }
    }

    // Корутина для загрузки сцены с задержкой
    private IEnumerator LoadNextScene(int sceneIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneIndex);
    }
}
