using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.Quests
{
    public enum EQuestCompletion
    {
        NotStarted,
        Started,
        Succeeded,
        Failed,
    }

    //namespace Quest
    [System.Serializable]
    [CreateAssetMenu(fileName = "NewQuest", menuName = "Quests/NewQuest", order = 1)]
    public class Quest : ScriptableObject
    {
        public string QuestName;
        public State QuestStateMachine; // If this machine accepts our QuestActivities log, the quest is successful.
        public bool isBlackList; // If true, the inputlist is a blacklist. Otherwise, it's a whitelist.
        public List<InputUnit> InputList; // The blacklist/whitelist used to filter InputUnity this quest recognizes.
        public bool PriorityQuest;

        public virtual void OnSucceeded(QuestStatus questStatus)
        {
            Debug.Log("Quest Completed: " + QuestName);
        }
        public virtual void OnFailed(QuestStatus questStatus)
        {
            Debug.Log("Quest Failed: " + QuestName);
        }
        public virtual void OnSucceededGlobal(QuestStatus questStatus) { }
        public virtual void OnFailedGlobal(QuestStatus questStatus) { }
    }

    public class Info
    {
        public Info(Quest quest, State state, InputUnit inputUnit)
        {
            Quest = quest;
            QuestState = state;
            QuestInputUnit = inputUnit;
        }
        public Quest Quest;
        public State QuestState;
        public State LastState;
        public InputUnit QuestInputUnit;

        public void UpdateQuestProgressionInfo(Quest quest, State state, InputUnit inputUnit)
        {
            LastState = QuestState;
            Quest = quest;
            QuestState = state;
            QuestInputUnit = inputUnit;
        }

        public bool GetNewState()
        {
            if (QuestState != LastState)
            {
                LastState = QuestState;
                Debug.Log("New state: " + QuestState);
                return true;
            }
            return false;
        }
    }

    [System.Serializable]
    public class QuestInProgress
    {
        public Quest Quest; // constant in the original source.
        [SerializeField]
        public EQuestCompletion QuestProgress;
        public List<InputUnit> QuestActivities = new List<InputUnit>();

        private State questState;
        public State QuestState
        {
            get { return (questState) ? questState : Quest.QuestStateMachine; }
            set { questState = value; }
        }
        public int priority = 0;


        public QuestInProgress() { }
        public QuestInProgress(Quest quest)
        {
            Quest = quest;
            QuestProgress = EQuestCompletion.Started;
            QuestState = Quest.QuestStateMachine;
            //Info = new Info(Quest, Quest.QuestStateMachine, null);
        }

        public void Reset()
        {
            QuestProgress = EQuestCompletion.Started;
            QuestState = Quest.QuestStateMachine;
            QuestActivities.Clear();
        }


        public bool UpdateQuest(object refObject, InputUnit questActivity)
        {
            if (Quest && (QuestProgress == EQuestCompletion.Started) && Quest.isBlackList != Quest.InputList.Contains(questActivity))
            {
                SM_Input QuestResult;
                QuestActivities.Add(questActivity);
                QuestResult = Quest.QuestStateMachine.RunState(refObject, QuestActivities);
                QuestState = QuestResult.FinalState;
                switch (QuestResult.CompletionType)
                {
                    case EStateMachineCompletionType.Accepted:
                        //Debug.Log("Quest set to succeeded.");
                        QuestProgress = EQuestCompletion.Succeeded;
                        return true;
                    case EStateMachineCompletionType.Rejected:                     
                        //Debug.Log("Quest set to failed.");
                        QuestProgress = EQuestCompletion.Failed;
                        return true;
                    case EStateMachineCompletionType.OutOfSteps:
                        break;
                    case EStateMachineCompletionType.NotAccepted:
                        QuestProgress = EQuestCompletion.Started;
                        return true;
                    default:
                        //Debug.LogError("Error updating quest.");
                        break;
                }
            }
            return false;
        }


    }

    
}

