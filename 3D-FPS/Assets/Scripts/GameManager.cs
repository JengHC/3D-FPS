using TMPro;
using UnityEditor;
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
    public int enemyNumber;
    public TMP_Text title;

    void Start()
    {
        isPlaying = true;
    }
    public void PlayerDie()
    {
        title.text = "You Died";
        GameEnd();
    }

    public void GameEnd()
    {
        isPlaying = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        GameOverCanvas.SetActive(true);
    }

    public void EnemyDie()
    {
        enemyNumber--;
        if(enemyNumber<=0)
        {
            title.text = "You win";
            GameEnd();
        }
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
