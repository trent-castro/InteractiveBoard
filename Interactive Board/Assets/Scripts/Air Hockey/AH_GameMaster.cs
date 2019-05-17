using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// A script that determines if the game is able to continue play.
/// </summary>
public class AH_GameMaster : MonoBehaviour
{
    // Singleton Paradigm
    public static AH_GameMaster Instance = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    [Header("Debug Mode")]
    [SerializeField]
    [Tooltip("Enables Debug Mode")]
    private bool m_debugMode = false;

    [Header("External References")]
    [Tooltip("A reference to the victory particles that trigger on game end")]
    [SerializeField]
    private GameObject victoryParticles = null;
    [Tooltip("Referenc to the Canvas Animation Script")]
    [SerializeField]
    private AH_CanvasManager canvasManager = null;
    [Tooltip("The offset of the puck after the puck goes into the right goal")]
    [SerializeField]
    Transform m_PuckResetOffsetForRightGoal = null;
    [Tooltip("The offset of the puck after the puck goes into the left goal")]
    [SerializeField]
    Transform m_PuckResetOffsetForLeftGoal = null;

    [SerializeField]
    GameObject m_LeftPaddle = null;

    [SerializeField]
    GameObject m_RightPaddle = null;

    [SerializeField]
    GameObject m_Puck = null;

    [Header("Configuration")]
    [Tooltip("The amount of score a single player must make before the game recognizes a win")]
    [SerializeField]
    private int m_scoreToWin = 8;
    [Tooltip("The amount of time before the check game state coroutine, to prevent the puck trail from messing up")]
    [SerializeField]
    private float m_checkGameStateDelay = 0.5f;
    [Tooltip("The amount of time before the end game coroutine begins, ensuring animation")]
    [SerializeField]
    private float m_endGameDelay = 0.5f;
    [Tooltip("The win text displayed after winning the game")]
    [SerializeField]
    private string m_winText = "WIN!!";
    [Tooltip("The lose text displayed after losing the game")]
    [SerializeField]
    private string m_loseText = "LOSE!!";


    [Header("Exposed Editor Values - Do Not Touch")]
    [Tooltip("The scores for the respective player")]
    [SerializeField] private int m_PlayerOneScore = 0;
    [Tooltip("The scores for the respective player")]
    [SerializeField] private int m_PlayerTwoScore = 0;

    /// Private sibling components
    private AudioSource m_audioSource;

    /// Private Data Members
    // A boolean tracking whether or not the win condition effects (such as a victory sound or the victory particle effects) have already been activated while the game is still running but is in the win state.
    private bool m_winEffectsTriggered = false;

    /// <summary>
    /// [DEBUG MODE] Records a message if debug mode is enabled.
    /// </summary>
    /// <param name="debugLog">The message to record</param>
    private void DebugLog(string debugLog)
    {
        if (m_debugMode)
        {
            Debug.Log(debugLog);
        }
    }

    /// <summary>
    /// The initialization of the script
    /// </summary>
    private void Start()
    {
        GetSiblingComponents();

        if (PlayerPrefs.HasKey("AirHockeyPointsRequired"))
        {
            m_scoreToWin = PlayerPrefs.GetInt("AirHockeyPointsRequired");
        }
        StartCoroutine(DropObjectsAnimationCoroutine());
        AH_PickUpManager.instance.m_canSpawn = false;
    }

    /// <summary>
    /// Fixes references to sibling components
    /// </summary>
    private void GetSiblingComponents()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Allocates points to the correct player based on the location of the goal
    /// </summary>
    /// <param name="isRightGoal">Which goal the puck has gone into</param>
    /// <param name="collision">The reference to the puck that had entered into the goal</param>
    public void GivePointToPlayer(bool isRightGoal, Collider2D collision)
    {
        // Begin the check for ending the game
        AH_Puck scoringPuck = collision.gameObject.GetComponent<AH_Puck>();
        scoringPuck.GetComponentInChildren<TrailRenderer>().enabled = false;
        StartCoroutine(CheckGameState(scoringPuck, isRightGoal));
    }

    /// <summary>
    /// Checks the state of the game after points have been allocated and takes appropriate actions.
    /// </summary>
    /// <param name="scoringPuck">Reference to the puck that scored the points</param>
    /// <returns>Time taken before action of the coroutine</returns>
    IEnumerator CheckGameState(AH_Puck scoringPuck, bool isRightGoal)
    {
        if (!m_winEffectsTriggered)
        {
            // Start score update
            canvasManager.OnPointEarned(isRightGoal);

            // Wait an appropriate amount of time before proceeding
            yield return new WaitForSecondsRealtime(m_checkGameStateDelay);

            // Reset the puck location
            if (isRightGoal) scoringPuck.ResetPuck(m_PuckResetOffsetForRightGoal.position);
            else scoringPuck.ResetPuck(m_PuckResetOffsetForLeftGoal.position);

            // Check the game scene
            if (DetermineWinState() && !m_winEffectsTriggered)
            {
                // Activate win effects
                victoryParticles.SetActive(true);
                canvasManager.ActivateWinLossText();
                m_audioSource.Play();
                AH_PickUpManager.instance.m_canSpawn = false;

                // Mark effects as triggered
                m_winEffectsTriggered = true;

                // Begin end game coroutine
                StartCoroutine(EndGame());
            }
        }
    }

    IEnumerator DropObjectsAnimationCoroutine()
    {
        yield return new WaitUntil(() => !canvasManager.IsGameStartPanelActive());
        AH_PickUpManager.instance.m_canSpawn = true;

        //drop paddles
        m_LeftPaddle.gameObject.SetActive(true);
        yield return new WaitUntil(() => m_LeftPaddle.activeInHierarchy);
        m_LeftPaddle.GetComponent<AH_Paddle>().StartDropAnimation();
        m_RightPaddle.gameObject.SetActive(true);
        yield return new WaitUntil(() => m_RightPaddle.activeInHierarchy);
        m_RightPaddle.GetComponent<AH_Paddle>().StartDropAnimation();
        yield return new WaitForSeconds(0.2f);
        //drop puck
        m_Puck.gameObject.SetActive(true);
        yield return new WaitUntil(() => m_Puck.activeInHierarchy);
        m_Puck.GetComponent<AH_Puck>().StartDropAnimation();

    }


    /// <summary>
    /// Handles the operations carried out upon game completion
    /// </summary>
    /// <returns>Time taken before action of the coroutine</returns>
    IEnumerator EndGame()
    {
        GetComponent<MultiTouchManager>().enabled = false;

        // Wait an appropriate amount of time before proceeding
        yield return new WaitForSecondsRealtime(m_endGameDelay);

        // Activate the end game menu
        canvasManager.ActivateEndGameMenu();
    }

    /// <summary>
    /// Determines the state of the game
    /// </summary>
    /// <returns>Whether or not the game has been won</returns>
    private bool DetermineWinState()
    {
        // Check if either player has won the game
        bool playerOneWin = m_PlayerOneScore >= m_scoreToWin;
        bool playerTwoWin = m_PlayerTwoScore >= m_scoreToWin;
        bool gameIsWon = playerOneWin || playerTwoWin;

        if (gameIsWon)
        {
            // Assign appropriate player text
            string playerOneText = (playerOneWin) ? m_winText : m_loseText;
            string playerTwoText = (playerTwoWin) ? m_winText : m_loseText;

            canvasManager.UpdateWinLossTexts(playerOneText, playerTwoText);
            return true;
        }
        else return false;
    }

    /// <summary>
    /// A method that allows the use of buttons to change the time scale of the game
    /// </summary>
    /// <param name="timescale">The new proposed time scale of the scene</param>
	public void SetTimeScale(float timescale)
    {
        Time.timeScale = timescale;
    }

    /// <summary>
    /// Gets the score of Left Player
    /// </summary>
    /// <returns>Player one's score</returns>
    public int GetPlayerOneScore()
    {
        return m_PlayerOneScore;
    }

    /// <summary>
    /// Gets the score of the Right Player
    /// </summary>
    /// <returns>Player two's score</returns>
    public int GetPlayerTwoScore()
    {
        return m_PlayerTwoScore;
    }

    /// <summary>
    /// Incraments the repective players score by 1
    /// </summary>
    /// <param name="isRightPlayer"></param>
    public void IncreaseScore(bool isRightPlayer)
    {
        if (isRightPlayer)
            m_PlayerOneScore++;
        else
            m_PlayerTwoScore++;
        DetermineWinState();
    }
}
