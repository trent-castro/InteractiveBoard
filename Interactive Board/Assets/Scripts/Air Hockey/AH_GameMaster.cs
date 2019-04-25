using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// A script that determines if the game is able to continue play.
/// </summary>
public class AH_GameMaster : MonoBehaviour
{
    /// <summary>
    /// A struct that keeps references to the left player and the right player's text objects
    /// </summary>
    [System.Serializable]
    struct PlayerText
    {
        public TextMeshProUGUI leftText;
        public TextMeshProUGUI rightText;

        public PlayerText(TextMeshProUGUI lefttext, TextMeshProUGUI righttext)
        {
            leftText = lefttext;
            rightText = righttext;
        }
    }

    [Header("Debug Mode")]
    [SerializeField]
    [Tooltip("Enables Debug Mode")]
    private bool m_debugMode = false;

    [Header("External References")]
    [Tooltip("References to the in game score text mesh pro objects")]
    [SerializeField]
    private PlayerText scoreTextReferences = default;
    [Tooltip("References to the in game result text mesh pro objects")]
    [SerializeField]
    private PlayerText winLossResultTextReferences = default;
    [Tooltip("The reference to the parent object for the win loss texts")]
    [SerializeField]
    private GameObject winLossResultTextParentReference = null;
    [Tooltip("A reference to the play again menu object")]
    [SerializeField]
    private GameObject gameOverMenu = null;
    [Tooltip("A reference to the victory particles that trigger on game end")]
    [SerializeField]
    private GameObject victoryParticles = null;

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
        // Iterate and update score texts
        if (isRightGoal)
        {
            ++m_PlayerOneScore;
            scoreTextReferences.leftText.text = "" + m_PlayerOneScore;
        }
        else
        {
            ++m_PlayerTwoScore;
            scoreTextReferences.rightText.text = "" + m_PlayerTwoScore;
        }

        // Begin the check for ending the game
        AH_Puck scoringPuck = collision.gameObject.GetComponent<AH_Puck>();
        scoringPuck.GetComponentInChildren<TrailRenderer>().enabled = false;
        StartCoroutine(CheckGameState(scoringPuck));
    }

    IEnumerator ScoreAnimationCoroutine(bool isRightPlayer, Collider2D collision)
    {
        //determine who scored
        //start score animation
        //wait for animation to finish
        yield return new WaitForSecondsRealtime(1.0f);
        //change player score
    }

    /// <summary>
    /// Checks the state of the game after points have been allocated and takes appropriate actions.
    /// </summary>
    /// <param name="scoringPuck">Reference to the puck that scored the points</param>
    /// <returns>Time taken before action of the coroutine</returns>
    IEnumerator CheckGameState(AH_Puck scoringPuck)
    {
        // Wait an appropriate amount of time before proceeding
        yield return new WaitForSecondsRealtime(m_checkGameStateDelay);

        // Reset the puck location
        scoringPuck.ResetPuck();

        // Check the game scene
        if (DetermineWinState() && !m_winEffectsTriggered)
        {
            // Activate win effects
            victoryParticles.SetActive(true);
            winLossResultTextParentReference.SetActive(true);
            GetComponent<AudioSource>().Play();
            
            // Mark effects as triggered
            m_winEffectsTriggered = true;

            // Begin end game coroutine
            StartCoroutine(EndGame());
        }

        // Stop this coroutine from constantly occuring
        StopCoroutine(CheckGameState(scoringPuck));
    }

    /// <summary>
    /// Handles the operations carried out upon game completion
    /// </summary>
    /// <returns>Time taken before action of the coroutine</returns>
    IEnumerator EndGame()
    {
        // Wait an appropriate amount of time before proceeding
        yield return new WaitForSecondsRealtime(m_endGameDelay);

        // Stop the game from being active
        Time.timeScale = 0.0f;

        // Checks if the game over menu exists
        if (gameOverMenu)
        {
            // Activates the menu
            gameOverMenu.SetActive(true);
        }

        // Stop this coroutine from constantly occuring
        StopCoroutine(EndGame());
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
            
            winLossResultTextReferences.leftText.text = playerOneText;
            winLossResultTextReferences.rightText.text = playerTwoText;
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
}
