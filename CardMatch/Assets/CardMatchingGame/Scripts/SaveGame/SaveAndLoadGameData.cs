using System.Collections.Generic;
using System.IO;
using CardGame.GamePlay;
using UnityEngine;

/// <summary>
/// Handles saving and loading of game data to persistent storage.
/// Stores the current game state, including grid, score, turns, and power-ups.
/// </summary>
public class SaveAndLoadGameData
{
    #region Fields

    private GameConfig gameConfig;           // Reference to game configuration
    private CardsManager cardsManager;       // Reference to the cards manager
    private ScoreManager scoreManager;       // Reference to the score manager
    private TurnsManager turnsManager;       // Reference to the turns manager
    private GameState gameState;             // Current game state to save/load
    private GameResultEvaluator evaluator;   // Reference to game result evaluator

    private string path;                     // File path for saving/loading game data

    #endregion


    #region Constructor

    /// <summary>
    /// Initializes a new instance of <see cref="SaveAndLoadGameData"/> and loads saved data if enabled.
    /// </summary>
    /// <param name="gameConfig">Game configuration settings.</param>
    /// <param name="cardsManager">Cards manager instance.</param>
    /// <param name="scoreManager">Score manager instance.</param>
    /// <param name="turnsManager">Turns manager instance.</param>
    /// <param name="evaluator">Game result evaluator instance.</param>
    public SaveAndLoadGameData(GameConfig gameConfig, CardsManager cardsManager, ScoreManager scoreManager,
        TurnsManager turnsManager, GameResultEvaluator evaluator)
    {
        path = Application.persistentDataPath + "/SaveGameData.json";

        this.gameConfig = gameConfig;
        this.cardsManager = cardsManager;
        this.scoreManager = scoreManager;
        this.turnsManager = turnsManager;
        this.evaluator = evaluator;

        gameState = new GameState(gameConfig.Rows, gameConfig.Cols);

        if (gameConfig.LoadSavedGame)
        {
            ConfigureSavedData();
        }

        Debug.Log(path);
    }

    #endregion


    #region Private Methods

    /// <summary>
    /// Configures game state from previously saved data.
    /// </summary>
    private void ConfigureSavedData()
    {
        gameState = Load();

        // Apply loaded values to managers and game state
        scoreManager.SetScore(gameState.score);
        turnsManager.SetTurns(gameState.turns);
        PowerUpManager.Instance.Init(ConfigurePowerUps(gameState.powerCount));

        gameConfig.Rows = gameState.gridData.row;
        gameConfig.Cols = gameState.gridData.col;

        cardsManager.InstantiateFromSavedData(gameState.gridData.cardsData, evaluator);
    }

    /// <summary>
    /// Creates a list of power-ups based on the saved count.
    /// </summary>
    /// <param name="count">Number of power-ups to configure.</param>
    /// <returns>List of configured power-ups.</returns>
    private List<PowerUp> ConfigurePowerUps(int count)
    {
        List<PowerUp> powerUps = new List<PowerUp>();
        for (int i = 0; i < count; i++)
        {
            powerUps.Add(new RevealAllCardsPowerUp(cardsManager));
        }

        return powerUps;
    }

    #endregion


    #region Public Methods

    /// <summary>
    /// Saves the current game state to persistent storage.
    /// </summary>
    public void Save()
    {
        Debug.Log("game saved");

        // Collect card state data
        List<CardStateData> cardsData = new List<CardStateData>();
        List<GameObject> cards = cardsManager.GetCards();
        foreach (GameObject cardObj in cards)
        {
            Card card = cardObj.GetComponent<Card>();
            CardData cardData = card.GetCardData();
            CardStateData data = new CardStateData(cardData.id, cardData.suit, cardData.rank,
                !cardObj.activeSelf, card.CurrentFace);
            cardsData.Add(data);
        }

        gameState.SetGridData(cardsData);
        gameState.SetScore(scoreManager.Score);
        gameState.SetTurns(turnsManager.Turn);
        gameState.SetPowerCount(PowerUpManager.Instance.PowerUpCount);

        // Serialize to JSON and save
        string jsonData = JsonUtility.ToJson(gameState);
        File.WriteAllText(path, jsonData);
    }

    /// <summary>
    /// Loads the game state from persistent storage.
    /// </summary>
    /// <returns>Deserialized <see cref="GameState"/> object.</returns>
    public GameState Load()
    {
        string jsonData = File.ReadAllText(path);
        return JsonUtility.FromJson<GameState>(jsonData);
    }

    #endregion
}
