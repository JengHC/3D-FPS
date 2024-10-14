using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static private GameManager instance = null;
    static public GameManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        instance = this;
    }

    public bool isPlaying;
    public GameObject GameOverCanvas;

    private void Start()
    {
        isPlaying = true;
    }
    public void PlayerDie()
    {
        isPlaying = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        GameOverCanvas.SetActive(true);
    }

    public void AgainPressed()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitPressed()
    {
        Application.Quit();
    }

}
