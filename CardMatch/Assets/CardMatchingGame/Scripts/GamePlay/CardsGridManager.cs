using System.Collections.Generic;
using CardGame.GamePlay;
using UnityEngine;
using UnityEngine.U2D;

/// <summary>
/// Manages the grid layout and game logic for the cards, including initialization, event handling, and UI updates.
/// </summary>
public class CardsGridManager : MonoBehaviour, IGameResultListner, IHUDEventsListner, IGameCompletePopupEventListner
{
    #region Constants

    private float hSpacing = 1.5f;
    private float vSpacing = 2f;

    #endregion

    #region Serialized Fields

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private SpriteAtlas spriteAtlas;
    [SerializeField] private HUDManager hudManager;

    #endregion

    #region Private Fields

    private CardsManager cardsManager;
    private GameResultEvaluator gameResultEvaluator;
    private ScoreManager scoreManager;
    private TurnsManager turnsManager;
    private SaveAndLoadGameData saveAndLoadGameData;

    #endregion

    #region Unity Methods

    private void OnEnable()
    {
        // Play background music when the grid manager is enabled
        AudioManager.Instance.Play(AudioGroupConstants.BGM_LOOP, AudioGroupConstants.BGM_LOOP, AudioGroupConstants.GAMEPLAYSFX);
    }

    private void OnDisable()
    {
        // Stop background music when the grid manager is disabled
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.Stop(AudioGroupConstants.BGM_LOOP);
        }
    }

    private void OnDestroy()
    {
        // Clean up managers and listeners on destruction
        gameResultEvaluator.OnDisable();
        cardsManager.OnDestroy();
        scoreManager.OnDestroy();
        turnsManager.OnDestroy();
    }

    #endregion

    #region Initialization

    /// <summary>
    /// Initializes the grid manager and all related managers and listeners.
    /// </summary>
    public void Init(GameConfig gameConfig)
    {
        cardsManager = new CardsManager(cardPrefab, transform, gameConfig.Rows * gameConfig.Cols, spriteAtlas);

        scoreManager = new ScoreManager(0);
        scoreManager.AddScoreListner(hudManager.OnScoreUpdate);

        turnsManager = new TurnsManager(0);
        turnsManager.AddTurnsListner(hudManager.OnTurnsUpdate);

        gameResultEvaluator = new GameResultEvaluator(cardsManager.GetCards());
        gameResultEvaluator.AddListener(this);
        gameResultEvaluator.AddListener(cardsManager);

        GameCompletePopup popup = (GameCompletePopup)UiScreenFactory.Instance.GetUIScreen(Constants.GAME_COMPLETE_POPUP);
        popup.Init();
        popup.AddListener(this);

        List<PowerUp> powerUps = new List<PowerUp>();
        for (int i = 0; i < 3; i++)
        {
            powerUps.Add(new RevealAllCardsPowerUp(cardsManager));
        }
        PowerUpManager.Instance.Init(powerUps);

        saveAndLoadGameData = new SaveAndLoadGameData(gameConfig, cardsManager, scoreManager, turnsManager, gameResultEvaluator);

        hudManager.AddListener(this);
        hudManager.UpdatePowerUpCount();

        CreateGrid(gameConfig.Rows, gameConfig.Cols);
    }

    #endregion

    #region Grid Layout

    /// <summary>
    /// Creates and positions the card grid based on the specified rows and columns.
    /// </summary>
    private void CreateGrid(int rows, int cols)
    {
        float gridWidth = (cols - 1) * hSpacing;
        float gridHeight = (rows - 1) * vSpacing;

        Vector3 startPosition = new Vector3(-gridWidth / 2, gridHeight / 2, 0);

        var cards = cardsManager.GetCards();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                int index = i * cols + j;
                if (index < cards.Count)
                {
                    GameObject card = cards[index];
                    card.transform.localPosition = new Vector3(startPosition.x + j * hSpacing, startPosition.y - i * vSpacing, startPosition.z);
                }
            }
        }
    }

    #endregion

    #region Game Event Handlers

    /// <summary>
    /// Called when result evaluation starts for two cards.
    /// </summary>
    public void OnResultEvaluationStarted(Card firstCard, Card secondCard)
    {
        // Optional: Add logic for when evaluation starts
    }

    /// <summary>
    /// Called when a match is found between two cards.
    /// </summary>
    public void OnMatchFound(Card firstCard, Card secondCard)
    {
        scoreManager.AddScore(1);
        turnsManager.IncreamentTurnCount();
    }

    /// <summary>
    /// Called when no match is found between two cards.
    /// </summary>
    public void OnNoMatch(Card firstCard, Card secondCard)
    {
        turnsManager.IncreamentTurnCount();
    }

    /// <summary>
    /// Called when the game is finished.
    /// </summary>
    public void OnGameFinished()
    {
        UIManager.Instance.PushScreen(UiScreenFactory.Instance.GetUIScreen(Constants.GAME_COMPLETE_POPUP));
    }

    #endregion

    #region UI Event Handlers

    /// <summary>
    /// Handles the Home button click event.
    /// </summary>
    public void OnHomeBtnClicked()
    {
        UIManager.Instance.PushScreen(UiScreenFactory.Instance.GetUIScreen(Constants.HOME_SCREEN));
        DestroyGame();
    }

    /// <summary>
    /// Handles the Save button click event.
    /// </summary>
    public void OnSaveBtnClicked()
    {
        saveAndLoadGameData.Save();
    }

    /// <summary>
    /// Handles the PowerUp button click event.
    /// </summary>
    public void OnClickPowerUp()
    {
        PowerUpManager.Instance.ActivatePowerUp();
    }

    /// <summary>
    /// Handles the Home button click event from another source.
    /// </summary>
    public void OnClickHomeButton()
    {
        DestroyGame();
    }

    #endregion

    #region Utility

    /// <summary>
    /// Destroys the grid manager game object.
    /// </summary>
    private void DestroyGame()
    {
        GameObject.Destroy(gameObject);
    }

    #endregion
}