using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// <see cref="LevelManager"/> manages transitions between Unity scenes.
/// </summary>
public class LevelManager : MonoBehaviour
{
	public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
