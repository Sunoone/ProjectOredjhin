using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Freethware.Inputs;
using StateMachine.Quests;
using StateMachine;
using UnityEngine.SceneManagement;
public class GameManager : Singleton<GameManager> {
    /*
    public bool WaitForCompletion = true;

    private void Reset()
    {
        CurrentCanvas = FindObjectOfType<Canvas>();
    }

    public GameObject PlayerPrefab;
    public int PlayerCount = 2;
    public List<Player> Players;
    public List<Color> PlayerColors = new List<Color>();
    public Transform[] SpawnPoints;

    public Color CombineColor;

    MultipleTargetCamera MTC;
    CameraFollow CF;
    public void Awake()
    {
        CurrentCanvas = FindObjectOfType<Canvas>();
        SentencePool = CurrentCanvas.GetComponentInChildren<SentencePool>();
        PlayerPool = CurrentCanvas.GetComponentInChildren<UserSectionPool>();
        Inventory = CurrentCanvas.GetComponentInChildren<Inventory>();

        Debug.Log("Reinit");
        SpawnPoints = transform.GetChild(0).GetComponentsInChildren<Transform>();
        MTC = Camera.main.GetComponent<MultipleTargetCamera>();
        CF = Camera.main.GetComponent<CameraFollow>();
        for (int i = 0; i < PlayerCount; i++)
        {            
            Quaternion spawnRotation = new Quaternion();
            GameObject go = Instantiate(PlayerPrefab, SpawnPoints[i].position, spawnRotation);
            go.name += i;
            Player player = go.GetComponent<Player>();
            player.Index = i;
            player.Color = PlayerColors[i];
            Players.Add(player);

            MTC.Targets.Add(go.transform);
        }

        Pointer_Inputs_Entity.Instance.User = Players[0];
        Inventory.Owner = Players[0];
        CF.target = Players[0].GetComponent<Controller2D>();
    }

    public void RestartQuest(Quest quest)
    {
        int length = PlayerCount;
        for (int i = 0; i < length; i++)
        {
            List<QuestInProgress> questList = Players[i].GetQuestStatus().QuestList;
            int width = questList.Count;
            for (int w = 0; w < length; w++)
            {
                if (questList[w].Quest == quest)
                    questList[w].Reset();
            }
        }
    }
    public List<Player> GetUGetPayerQuestsUncompleted(Quest quest) { return GetPlayerQuestConditional(quest, EQuestCompletion.Succeeded, false); }
    public List<Player> GetUGetPayerQuestsCompleted(Quest quest) { return GetPlayerQuestConditional(quest, EQuestCompletion.Succeeded, true); }
    private List<Player> GetPlayerQuestConditional(Quest quest, EQuestCompletion completionType, bool condition)
    {
        List<Player> playerList = new List<Player>();
        int length = PlayerCount;
        for (int i = 0; i < length; i++)
        {
            List<QuestInProgress> questList = Players[i].GetQuestStatus().QuestList;
            int width = questList.Count;
            for (int w = 0; w < length; w++)
            {
                if (questList[w].Quest == quest)
                    if ((questList[w].QuestProgress == completionType) == condition)
                        playerList.Add(Players[i]);
            }
        }
        return playerList;
    }

    public List<Player> GetPlayerQuests(Quest quest)
    {
        List<Player> playerList = new List<Player>();
        int length = PlayerCount;
        for (int i = 0; i < length; i++)
        {
            List<QuestInProgress> questList = Players[i].GetQuestStatus().QuestList;
            int width = questList.Count;
            for (int w = 0; w < length; w++)
            {
                if (questList[w].Quest == quest)
                        playerList.Add(Players[i]);
            }
        }
        return playerList;
    }

    public void GlobalQuestUpdate(InputUnit input)
    {
        int length = PlayerCount;
        for (int i = 0; i < length; i++)
            Players[i].GetQuestStatus().UpdateQuests(input);
    }
    public void GlobalQuestStart(Quest quest)
    {
        int length = PlayerCount;
        for (int i = 0; i < length; i++)
            Players[i].GetQuestStatus().StartQuest(quest);
    }


    public Canvas CurrentCanvas;
    public SymbolParticlePool SymbolParticlePool;


    #region Debug
    public Inventory Inventory;
    public UserSectionPool PlayerPool;
    public SentencePool SentencePool;
    #endregion

    public bool IsPlayer(Entity player) { return (player == null)? false : player.GetPlayerIndex() > -1; }


    public PlayerStatistics PS;
    private void Update()
    {
        DebugUpdate();

        if (Input.GetKey(KeyCode.JoystickButton6) && Input.GetKey(KeyCode.JoystickButton7))
            SceneManager.LoadScene("MainMenu");

        if (Input.GetKeyDown(KeyCode.JoystickButton6))
            PS.Toggle();
    }

    public Quest quest;

    private void DebugUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E))
            DebugCleanSentences();
        if (Input.GetKeyDown(KeyCode.I))
            GlobalQuestStart(quest);
    }


    private void DebugCleanSentences()
    {
        PlayerPool.PoolAll();
        Inventory.BuildEmpty();

        int length = SentencePool.PoolList.Count;
        for (int i = 0; i < length; i++)
        {
            var sentence = SentencePool.PoolList[i];
            if (sentence.gameObject.activeInHierarchy)
            {
                sentence.Rebuild();
            }
        }

        SymbolParticlePool.PoolAll();
    }

    */
}
