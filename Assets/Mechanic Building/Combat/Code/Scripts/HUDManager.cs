using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] Button pauseButt, continueButt, restartButt, menuButt;
    [SerializeField] GameObject pausePanel;
    [SerializeField] HealthBar _healthBar;
    [SerializeField] TransitionAnimation _transitionAnimation;
    PlayerStats _playerStats;

    void Awake()
    {
        Time.timeScale = 1f;
        _playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        pausePanel.SetActive(false);
    }
    void OnEnable()
    {
        _playerStats.OnHealthChanged += OnHealthChanged;
        pauseButt.onClick.AddListener(PauseGame);
        continueButt.onClick.AddListener(Continue);
        restartButt.onClick.AddListener(Restart);
        menuButt.onClick.AddListener(Menu);
    }

    void Update(){
        if (Input.GetKey(KeyCode.Escape)) PauseGame();
    }

    void OnHealthChanged(PlayerStats stats)
    {
        _healthBar.SetValue(stats.CurrentHealth/stats.MaxHealth);
        if(stats.CurrentHealth <= 0)
        {
            Time.timeScale = 0.3f;
            Invoke(nameof(Restart), 0.5f);
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void Continue()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    void Restart()
    {
        Time.timeScale = 1f;
        _transitionAnimation.StartSceneTransition(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        _transitionAnimation.StartSceneTransition("Menu");
    }
}