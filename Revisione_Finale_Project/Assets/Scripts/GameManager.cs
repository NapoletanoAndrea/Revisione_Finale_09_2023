using AI_Perception.Senses;
using AI_Perception.Stimuli;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public GameObject uiPanel;
    public TMP_Text finishText;

    public string loseSightString;
    public string loseHearString;
    public string winString;

    public MonoBehaviour playerController;
    public StimulusType playerType;

    private bool _enabled = true;

    private void Awake()
    {
        Instance = this;
    }

    public void Lose(Sense sense)
    {
        if (!_enabled)
            return;
        
        if (sense.Type == typeof(Sight))
            finishText.text = loseSightString;
        else if (sense.Type == typeof(Hearing))
            finishText.text = loseHearString;
        else return;
            
        uiPanel.SetActive(true);
        Time.timeScale = 0;
        _enabled = false;
    }

    public void Win()
    {
        if (!_enabled)
            return;
        
        playerController.enabled = false;
        finishText.text = winString;
        uiPanel.SetActive(true);
        Time.timeScale = 0;
        _enabled = false;
    }

    public void Retry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
