using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using StateMachine.Quests;
using UnityEngine.Events;

/// <summary>
/// This class solves the logic for how quests are prioritized.
/// </summary>

[System.Serializable]
//[RequireComponent(typeof(QuestStatus))]
public class QuestPrioritizer : MonoBehaviour {

    public QuestStatus QS;
    public virtual void SetQuestStatus(QuestStatus qs)
    {
        QS = qs;
        AddAllQuestToSortingLists();
    }
    // Requires new input is still broken.
    public bool TrackPriorityQuests(QuestStatus qs, Quest[] quests, Callback<List<Info>> infoCallBack, bool requiresNewInputUnit)
    {
        SetQuestStatus(qs);
        List<QuestInProgress> QIPs = new List<QuestInProgress>();
        List<QuestInProgress> ToAddLater = new List<QuestInProgress>();
        int questsLength = quests.Length;

        List<QuestInProgress> priorityQuests = PriorityQuests;
        int priorityLength = priorityQuests.Count;
        // Looks for priority quests first
        for (int y = priorityLength - 1; y >= 0; y--)
            for (int x = 0; x < questsLength; x++)
                if (quests[x] == priorityQuests[y].Quest)
                {
                    //if (!requiresNewInputUnit || (requiresNewInputUnit && priorityQuests[y].Info.GetNewState()))
                    //    QIPs.Add(priorityQuests[y]);
                    //else
                    //    ToAddLater.Add(priorityQuests[y]);
                }

        //QIPs.AddRange(ToAddLater);

        priorityQuests = PriorityList;
        priorityLength = priorityQuests.Count;
        for (int y = priorityLength - 1; y >= 0; y--)
            for (int x = 0; x < questsLength; x++)
                if (quests[x] == priorityQuests[y].Quest)
                {
                    //if (!requiresNewInputUnit || (requiresNewInputUnit && priorityQuests[y].Info.GetNewState()))
                     //   QIPs.Add(priorityQuests[y]);
                    //else
                        //ToAddLater.Add(priorityQuests[y]);
                }

        //QIPs.AddRange(ToAddLater);
        
        List<Info> QIPsInfoPriority = new List<Info>();
        List<Info> QIPsInfo = new List<Info>();
        questsLength = QIPs.Count;
        for (int i = 0; i < questsLength; i++)
        {
            QuestInProgress QIP = QIPs[i];
            switch (QIP.QuestProgress)
            {
                case EQuestCompletion.NotStarted:
                    break;
                case EQuestCompletion.Started:
                    //QIPsInfoPriority.Add(QIP.Info);
                    break;
                case EQuestCompletion.Succeeded:
                case EQuestCompletion.Failed:
                    //QIPsInfo.Add(QIP.Info);
                    break;
                default:
                    break;
            }
        }
        QIPsInfoPriority.AddRange(QIPsInfo);
        if (QIPsInfoPriority.Count > 0)
        {
            if (infoCallBack != null)
                infoCallBack(QIPsInfoPriority);
            return true;
        }
        return false;
    }

   protected void Enable()
    {
        if (QS == null)
            return;

        AddAllQuestToSortingLists();
        QS.OnBeginQuest += (AddQuestToSortingLists);
        QS.OnQuestNewState += (UpdateSortingLists);
    }
    protected void Disable()
    {
        if (QS == null)
            return;

        QS.OnBeginQuest -= (AddQuestToSortingLists);
        QS.OnQuestNewState -= (UpdateSortingLists);
    }

    protected void OnEnable() { Enable(); }
    protected void OnDisable() { Disable(); }

   

    public void AddAllQuestToSortingLists()
    {
        if (QS == null)
            return;

        int length = QS.QuestList.Count;
        for (int i = 0; i < length; i++)
            AddQuestToSortingLists(QS.QuestList[i]);
    }
    public void AddQuestToSortingLists(QuestInProgress QIP)
    {
        //if (QIP.Info == null)
        //    QIP.Info = new Info(QIP.Quest, QIP.Quest.QuestStateMachine, null);
        if (QIP.Quest.PriorityQuest)
        {
            if (!PriorityQuests.Contains(QIP))
                PriorityQuests.Add(QIP);
        }
        else
        {
            if (!PriorityList.Contains(QIP))
                PriorityList.Add(QIP);
        }
    }
    public void UpdateSortingLists(QuestInProgress QIP)
    {
        if (QIP.Quest.PriorityQuest)
            PriorityQuests.SetItemToLast(QIP);
        else
            PriorityList.SetItemToLast(QIP);
    }

    protected void Reset()
    {
        QS = gameObject.SearchComponent<QuestStatus>();
    }

    public List<QuestInProgress> PriorityQuests = new List<QuestInProgress>();
    public List<QuestInProgress> PriorityList = new List<QuestInProgress>();
}
