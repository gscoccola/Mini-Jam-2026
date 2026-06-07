using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class InGameUI : Singleton<InGameUI>
{

    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _gameOverMenu;
    private UIControls _controls;
    public bool IsGameOver;

    #region Setup
    protected override void Awake()
    {
        base.Awake();

        _controls = new UIControls();
        _controls.Player.TogglePause.performed += ctx => TogglePauseMenu();

        _pauseMenu.SetActive(false);
        _gameOverMenu.SetActive(false);
    }

    private void OnEnable()
    {
        _controls.Enable(); // Must explicitly enable the action map
    }

    private void OnDisable()
    {
        _controls.Disable(); // Clean up to avoid memory leaks
    }
    #endregion

    #region Pause
    public void TogglePauseMenu()
    {
        if (IsGameOver) return;
        if (_pauseMenu.activeSelf) UnPauseGame();
        else PauseGame();
    }

    private void PauseGame()
    {
        _pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    private void UnPauseGame()
    {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    #endregion

    #region GameOver

    public void TriggerGameOver(bool explode = true)
    {
        if (IsGameOver ) return;
        PlayerMovement.Instance.Die(explode);
        IsGameOver = true;
        DOVirtual.Float(0f, 1f, 1.8f, value => { }).OnComplete(() =>
        {
            Time.timeScale = 0f;
            _gameOverMenu.SetActive(true);
        });
    }

    #endregion

    #region Button Methods
    public void RestartRun()
    {
        Time.timeScale = 1f;
        LevelLoader.Instance.ReloadLevel();
    }

    public void BackToMain()
    {
        Time.timeScale = 1f;
        LevelLoader.Instance.LoadLevel("Menu");
    }
    #endregion
}
