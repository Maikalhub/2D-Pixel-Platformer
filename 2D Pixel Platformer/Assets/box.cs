using UnityEngine;
using UnityEngine.SceneManagement;

public class Box : MonoBehaviour
{
    private bool isColliding = false; // Флаг, чтобы избежать повторных срабатываний

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!isColliding && col.gameObject.CompareTag("Enemy"))
        {
            isColliding = true; // Устанавливаем флаг, чтобы не вызывать метод повторно

            SoundEffectManager.Play("PlayerHit"); // Сначала проигрываем звук
            Invoke("ReloadScene", 0.2f); // Даем время звуку проиграться перед загрузкой сцены
        }
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
