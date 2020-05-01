using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Command plotPlayerAObjectCommand, plotPlayerBObjectCommand, strikeOffCommand, updateScoreCommand, replayCommand;
    public static bool isPlayerA = true, isPlayerB = false;
    public static List<Command> replayCommands;
    public static List<Vector3> replayPositions;
    public static List<GameObject> placedObjects;
    public GameObject[] gridValues;
    public static bool gameOver = false;
    public static int angle = 0;
    enum Players{PlayerA, PlayerB};
    Button replayButton, undoButton;
    public static TMPro.TextMeshProUGUI playerNameText, winnerText;
    public static TMPro.TextMeshProUGUI playerAScoreText, playerBScoreText;
    public static GameObject scoreCard;
    GameObject currGameObject;

    // Start is called before the first frame update
    void Start()
    {
        plotPlayerAObjectCommand = new PlayerA();
        plotPlayerBObjectCommand = new PlayerB();
        strikeOffCommand = new StrikeOff();
        updateScoreCommand = new Score();
        replayCommand = new Replay();

        replayCommands = new List<Command>();
        replayPositions = new List<Vector3>();
        placedObjects = new List<GameObject>();

        replayButton = GameObject.Find("Replay").GetComponent<Button>();
        undoButton = GameObject.Find("Undo").GetComponent<Button>();

        playerNameText = GameObject.Find("Player Name").GetComponent<TMPro.TextMeshProUGUI>();
        winnerText = GameObject.Find("Winner").GetComponent<TMPro.TextMeshProUGUI>();
        playerAScoreText = GameObject.Find("Player A Score").GetComponent<TMPro.TextMeshProUGUI>();
        playerBScoreText = GameObject.Find("Player B Score").GetComponent<TMPro.TextMeshProUGUI>();
        scoreCard = GameObject.Find("Score Card");
        scoreCard.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (placedObjects.Count == 9 && !gameOver)
        {
            playerNameText.text = "DRAW!";
            UpdateScores();
        }
        if (placedObjects.Count == 9 || gameOver)
            return;
        if(Input.touchCount == 1 || Input.GetMouseButtonDown(0))
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
#endif
#if UNITY_EDITOR
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#endif
            RaycastHit hit;
            if (Physics.Raycast(ray.origin,ray.direction,out hit))
            {
                Debug.Log(hit.collider.gameObject.transform.position);
                if (isPlayerA && hit.collider.gameObject.tag == "Respawn")
                {
                    playerNameText.text = "Player B";
                    plotPlayerAObjectCommand.Execute(hit.collider.transform.position, plotPlayerAObjectCommand);
                    replayCommands.Add(plotPlayerAObjectCommand);
                    replayPositions.Add(hit.collider.transform.position);
                    for(int i = 0; i < gridValues.Length; i++)
                    {
                        Debug.Log(i + "outside" + gridValues[i].name);

                        if (gridValues[i].name == hit.collider.gameObject.name)
                        {
                            Debug.Log(i + "inside");
                            hit.collider.gameObject.name = "Player A";
                        }
                    }
                    currGameObject = hit.collider.gameObject;
                    hit.collider.enabled = false;
                    isPlayerA = false;
                    isPlayerB = true;
                    undoButton.interactable = true;
                }
                else if (isPlayerB && hit.collider.gameObject.tag == "Respawn")
                {
                    playerNameText.text = "Player A";
                    plotPlayerBObjectCommand.Execute(hit.collider.transform.position, plotPlayerBObjectCommand);
                    replayCommands.Add(plotPlayerBObjectCommand);
                    replayPositions.Add(hit.collider.transform.position);
                    for (int i = 0; i < gridValues.Length; i++)
                    {
                        if (gridValues[i].name == hit.collider.gameObject.name)
                        {
                            hit.collider.gameObject.name = "Player B";
                            Debug.Log(hit.collider.gameObject.name);
                        }
                    }
                    currGameObject = hit.collider.gameObject;
                    hit.collider.enabled = false;
                    isPlayerA = true;
                    isPlayerB = false;
                    undoButton.interactable = true;
                }

                if (placedObjects.Count > 4)
                    CheckGameStatus();
            }
        }
    }

    public void OnReplayClicked()
    {
        ClearObjects();
        replayButton.interactable = false;
        StartCoroutine(Replay());
    }

    public void OnUndoClicked()
    {
        if (isPlayerA)
        {
            plotPlayerBObjectCommand.Undo();
        }
        else
        {
            plotPlayerAObjectCommand.Undo();
        }
        currGameObject.GetComponent<Collider>().enabled = true;
        undoButton.interactable = false;
    }

    void ClearObjects()
    {
        foreach (GameObject ga in placedObjects)
            Destroy(ga);
    }

    public IEnumerator Replay()
    {
        scoreCard.SetActive(false);
        int i = 0;
        for (i = 0; i < replayCommands.Count; i++)
        {
            yield return new WaitForSeconds(0.5f);
            if(i < replayCommands.Count - 1)
                replayCommands[i].Plot(replayPositions[i]);
            else if(i == replayCommands.Count - 1 && !gameOver)
                replayCommands[i].Plot(replayPositions[i]);
            else
                replayCommands[i].StrikeOffDots(replayPositions[i], angle);
        }
        yield return new WaitForSeconds(0.5f);
        replayButton.interactable = true;
        scoreCard.SetActive(true);
    }

    public void CheckGameStatus()
    {
        Debug.Log(gridValues[0].name + gridValues[1].name + gridValues[2].name);
        if (gridValues[0].name == gridValues[1].name && gridValues[1].name == gridValues[2].name)
        {
            playerNameText.text = gridValues[0].name + "  WINS!";
            replayPositions.Add(gridValues[1].transform.position);
            strikeOffCommand.Execute(gridValues[1].transform.position, strikeOffCommand, 90);
            UpdateScores();
        }

        if (gridValues[3].name == gridValues[4].name && gridValues[4].name == gridValues[5].name)
        {
            playerNameText.text = gridValues[3].name + "  WINS!";
            replayPositions.Add(gridValues[4].transform.position);
            strikeOffCommand.Execute(gridValues[4].transform.position, strikeOffCommand, 90);
            UpdateScores();
        }

        if (gridValues[6].name == gridValues[7].name && gridValues[7].name == gridValues[8].name)
        {
            playerNameText.text = gridValues[6].name + "  WINS!";
            replayPositions.Add(gridValues[7].transform.position);
            strikeOffCommand.Execute(gridValues[7].transform.position, strikeOffCommand, 90);
            UpdateScores();
        }

        if (gridValues[0].name == gridValues[3].name && gridValues[3].name == gridValues[6].name)
        {
            playerNameText.text = gridValues[0].name + "  WINS!";
            replayPositions.Add(gridValues[3].transform.position);
            strikeOffCommand.Execute(gridValues[3].transform.position, strikeOffCommand, 0);
            UpdateScores();
        }

        if (gridValues[1].name == gridValues[4].name && gridValues[4].name == gridValues[7].name)
        {
            playerNameText.text = gridValues[1].name + "  WINS!";
            replayPositions.Add(gridValues[4].transform.position);
            strikeOffCommand.Execute(gridValues[4].transform.position, strikeOffCommand, 0);
            UpdateScores();
        }

        if (gridValues[2].name == gridValues[5].name && gridValues[5].name == gridValues[8].name)
        {
            playerNameText.text = gridValues[2].name + "  WINS!";
            replayPositions.Add(gridValues[5].transform.position);
            strikeOffCommand.Execute(gridValues[5].transform.position, strikeOffCommand, 0);
            UpdateScores();
        }

        if (gridValues[0].name == gridValues[4].name && gridValues[4].name == gridValues[8].name)
        {
            playerNameText.text = gridValues[0].name + "  WINS!";
            replayPositions.Add(gridValues[4].transform.position);
            strikeOffCommand.Execute(gridValues[4].transform.position, strikeOffCommand, 45);
            UpdateScores();
        }

        if (gridValues[2].name == gridValues[4].name && gridValues[4].name == gridValues[6].name)
        {
            playerNameText.text = gridValues[2].name + "  WINS!";
            replayPositions.Add(gridValues[4].transform.position);
            strikeOffCommand.Execute(gridValues[4].transform.position, strikeOffCommand, -45);
            UpdateScores();
        }
        if (gameOver)
            replayCommands.Add(strikeOffCommand);
    }

    void UpdateScores()
    {
        undoButton.interactable = false;
        gameOver = true;
        Debug.Log(playerNameText.text);
        if (playerNameText.text.Contains("Player A"))
            updateScoreCommand.Execute(Vector3.zero, updateScoreCommand, 0, Score.PlayerNames.PLAYER_A);
        else if(playerNameText.text.Contains("Player B"))
            updateScoreCommand.Execute(Vector3.zero, updateScoreCommand, 0, Score.PlayerNames.PLAYER_B);
        else
            updateScoreCommand.Execute(Vector3.zero, updateScoreCommand, 0, Score.PlayerNames.NONE);

        winnerText.text = playerNameText.text;
    }

    public void ReplayGame()
    {
        replayCommand.Execute(Vector3.zero, replayCommand, 0, Score.PlayerNames.NONE);
        var i = 0;
        foreach (GameObject ga in gridValues)
        {
            ga.GetComponent<Collider>().enabled = true;
            ga.name = "point" + i;
            i++;
        }
        scoreCard.SetActive(false);
    }
}
