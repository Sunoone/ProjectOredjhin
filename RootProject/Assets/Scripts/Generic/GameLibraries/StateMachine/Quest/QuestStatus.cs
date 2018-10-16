using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using StateMachine.Quests;
using UnityEngine.Events;

[System.Serializable]
public class QuestStatus : MonoBehaviour {

    public Callback<QuestInProgress> OnQuestNewState;
    public Callback<QuestInProgress> OnQuestEndState;
    public Callback<QuestInProgress> OnBeginQuest;

    private void Start()
    {
        
    }

    /*public UnityEvent<QuestInProgress> OnQuestNewState;
    public UnityEvent<QuestInProgress> OnQuestEndState;
    public UnityEvent<QuestInProgress> OnBeginQuest;*/

    public InputUnit Keypad1;
    public InputUnit Keypad2;
    public InputUnit Keypad3;
    public InputUnit Keypad4;
    public InputUnit Keypad5;
    public InputUnit Keypad6;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            Debug.Log("T is for test");

        if (Input.GetKeyDown(KeyCode.Keypad1))
            UpdateQuests(Keypad1);
        if (Input.GetKeyDown(KeyCode.Keypad2))
            UpdateQuests(Keypad2);
        if (Input.GetKeyDown(KeyCode.Keypad3))
            UpdateQuests(Keypad3);
        if (Input.GetKeyDown(KeyCode.Keypad4))
            UpdateQuests(Keypad4);
        if (Input.GetKeyDown(KeyCode.Keypad5))
            UpdateQuests(Keypad5);
        if (Input.GetKeyDown(KeyCode.Keypad6))
            UpdateQuests(Keypad6);
    }

    public void UpdateQuestsWithFullBacklog(QuestStatus QS)
    {
        int length = QS.QuestList.Count;
        for (int i = 0; i < length; i++)
        {
            StartQuest(QS.QuestList[i].Quest);
        }
        length = QS.QuestActivities.Count;
        for (int i = 0; i < length; i++)
        {
            UpdateQuests(QS.QuestActivities[i]);
        }
    }

    public List<QuestInProgress> UpdateQuests(InputUnit questActivity)
    {
        List<QuestInProgress> RecentlyUpdatedQuests = new List<QuestInProgress>();
        List<QuestInProgress> RecentlyCompletedQuests = new List<QuestInProgress>();

        //Debug.Log("Input: " + questActivity);
        QuestActivities.Add(questActivity); // Update the mast list of everything we've ever done.

        int length = QuestList.Count;
        for (int i = 0; i < length; i++) // Update to individual quests (if they care about this activity) and see if they are complete.
        {
            if (QuestList[i].UpdateQuest(this, questActivity))
            {
                RecentlyUpdatedQuests.Add(QuestList[i]);
                if (QuestList[i].QuestProgress != EQuestCompletion.Started)
                    RecentlyCompletedQuests.Add(QuestList[i]);
            }
        }

        length = RecentlyUpdatedQuests.Count;
        for (int i = 0; i < length; i++)
        {
            if (OnQuestNewState != null)
                OnQuestNewState.Invoke(RecentlyUpdatedQuests[i]);
            //UpdateSortingLists(RecentlyUpdatedQuests[i]);
        }

        // Process completed quests after updating all quests.
        // This way, a completed quest can't inject out-of-order input units into other quests.
        length = RecentlyCompletedQuests.Count;
        for (int i = 0; i < length; i++)
        {
            QuestInProgress QIP = RecentlyCompletedQuests[i];
            if (QIP.QuestProgress == EQuestCompletion.Succeeded)
            {
                QIP.Quest.OnSucceeded(this);
            }
            else// if (QIP.QuestProgress == EQuestCompletion.Failed)
            {
                QIP.Quest.OnFailed(this);
            }

            if (OnQuestEndState != null)
                OnQuestEndState.Invoke(QIP);
        }

        return RecentlyCompletedQuests;
        /*length = QuestList.Count;
        for (int i = 0; i < length; i++)
        {
            Debug.Log(QuestList[i].Quest.ToString());
        }*/
    }

    

   
    // Add a new quest-in-progress entry, or begin the quest provided if it's already in the list and hasn't been started yet.
    public bool StartQuest(Quest quest)
    {
        QuestInProgress QIP;
        int length = QuestList.Count;
        for (int i = 0; i < length; i++)
        {
            QIP = QuestList[i];
            if (QIP.Quest == quest)
            {
                if (QIP.QuestProgress == EQuestCompletion.NotStarted)
                {
                    Debug.LogWarning("Changing quest " + QIP.Quest.QuestName + " to Started status.");
                    QIP.QuestProgress = EQuestCompletion.Started;
                    return true;
                }
                Debug.LogWarning("Quest " + QIP.Quest.QuestName + " is already in the list.");
                return false;
            }
        }
        QIP = new QuestInProgress(quest);
        QuestList.Add(QIP);
        if (OnBeginQuest != null)
            OnBeginQuest.Invoke(QIP);
        return true;
    }

    public QuestInProgress GetQuestInProgress(Quest quest)
    {
        int length = QuestList.Count;
        for (int i = 0; i < length; i++)
        {
            QuestInProgress QIP = QuestList[i];
            if (QIP.Quest == quest)
            {
                return QIP;
            }
        }
        return null;
    }

    public List<InputUnit> QuestActivities = new List<InputUnit>();
    public List<QuestInProgress> QuestList = new List<QuestInProgress>();
    public List<Quest> CompletedQuestList
    {
        get
        {
            List<Quest> completedQuestList = new List<Quest>();
            int length = QuestList.Count;
            for (int i = 0; i < length; i++)
            {
                if (QuestList[i].QuestProgress == EQuestCompletion.Succeeded)
                    completedQuestList.Add(QuestList[i].Quest);
            }
            return completedQuestList;
        }
    }
    
    public bool HasQuest(Quest quest)
    {
        int length = QuestList.Count;
        for (int i = 0; i < length; i++)
        {
            if (QuestList[i].Quest == quest)
                return true;
        }
        return false;
    }
    public bool HasCompletedQuest(Quest quest) { return CompletedQuestList.Contains(quest); }
}
