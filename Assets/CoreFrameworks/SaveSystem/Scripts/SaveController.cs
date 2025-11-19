using UnityEngine;

public class SaveController : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;

    public GameObject saveMenuUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenuUI.SetActive(isPaused);
        Time.timeScale = isPaused ? 0.0001f : 1f;
    }

    public void SaveGame()
    {
        saveMenuUI.SetActive(true);
    }

}

public enum SaveLoadMode
{
    Save,
    Load
}
