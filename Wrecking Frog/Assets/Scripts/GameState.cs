using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{

    public static GameState Instance { get; private set;}
    const string END_SCENE_PATH = "Scenes/EndScene";
    const string TITLE_SCREEN_PATH = "Scenes/TitleScreen";
    /**
    <summary>
    The player's health is intimately tied into the game
    win and loss condition.  
    </summary>
    */
    private PlayerHealth m_player;
    private int m_numberOfTadpoles;
    private bool m_transition;
  
    private enum eGameState
    {
        TitleScreen,
        Playing,
        Win,
        Lose
    }

    private eGameState m_currentState;

    public void GoToTitle()
    {
        m_currentState = eGameState.TitleScreen;
        m_transition = true;
        SceneManager.LoadScene(TITLE_SCREEN_PATH);
    }



    public void StartGame()
    {
        m_currentState = eGameState.Playing;
        m_transition = true;
    }

    private void Awake() 
    {
        if( Instance != null && Instance != this) 
        {
            Destroy(this);
            return;
        }
        else 
        {
            if(Instance != this)
            {
                Instance = this;
                m_transition = true;
                DontDestroyOnLoad(Instance);
                string sceneName = SceneManager.GetActiveScene().name;
                switch(sceneName)
                {
                    case "TitleScreen":
                        m_currentState = eGameState.TitleScreen;
                        break;
                    case "EndScene":
                        m_currentState = eGameState.Lose;
                        Debug.Log("Defaulting EndScene");
                        break;
                    default:
                        m_currentState = eGameState.Playing;
                        break;
                }

            }
        }
    }

    private bool verifyPlayer(PlayerHealth player) 
    {
        return player.gameObject == GameObject.Find("Frog");
    }

    public void PlayerWin(PlayerHealth player)
    {
        //  To prevent other callers from accidentally calling this
        if (verifyPlayer(player))
        {
            m_currentState = eGameState.Win;
            SceneManager.LoadScene(2);
            m_transition = true;
        }
    }

    public void PlayerLose(PlayerHealth player)
    {
        if(verifyPlayer(player))
        {
            m_currentState = eGameState.Lose;
            SceneManager.LoadScene(2);
            m_transition = true;
        }
    }

    private TextMeshProUGUI findEndSceneMessageText()
    {
        if(GameObject.Find("EndMessageText") != null)
        {
            return GameObject.Find("EndMessageText").GetComponent<TextMeshProUGUI>();
        }
        else
        {
            return null;
        }
    }

    public bool allTadpolesCollected(Stack<Tadpole> playerStack)
    {
        return playerStack.Count == m_numberOfTadpoles;
    }

    
    void Update()
    {
        if(m_transition)
        {
            m_transition = false;
            switch (m_currentState)
            {
                case eGameState.TitleScreen:
                    break;
                case eGameState.Playing:
                    m_numberOfTadpoles = 
                        FindObjectsOfType<Tadpole>().Length;
                    m_transition = m_numberOfTadpoles == 0;
                    Debug.Log($"Found {m_numberOfTadpoles} tadpoles");
                    break;
                case eGameState.Win:
                    if(findEndSceneMessageText() != null)
                    {
                        findEndSceneMessageText().SetText("Congratulations! You Won!");
                    }
                    else
                    {
                        m_transition = true;
                    }
                    break;
                case eGameState.Lose:
                    if(findEndSceneMessageText() != null)
                    {
                        findEndSceneMessageText().SetText("Game Over! You Lost");
                    }
                    else
                    {
                        m_transition = true;
                    }
                    break;
            }
        }
    }
}

