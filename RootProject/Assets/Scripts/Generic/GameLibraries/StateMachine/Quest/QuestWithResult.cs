using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.Quests
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "NewQuestWithResult", menuName = "Quests/NewQuestWithResult", order = 1)]
    public class QuestWithResult : Quest
    {

        public override void OnSucceeded(QuestStatus questStatus)
        {
            base.OnSucceeded(questStatus);

            int length = SuccessQuests.Count;
            for (int i = 0; i < length; i++)
            {
                questStatus.StartQuest(SuccessQuests[i]);
            }
            length = SuccessInputs.Count;
            for (int i = 0; i < length; i++)
            {
                questStatus.UpdateQuests(SuccessInputs[i]);
            }
        }

        public override void OnFailed(QuestStatus questStatus)
        {
            base.OnFailed(questStatus);

            int length = FailureQuests.Count;
            for (int i = 0; i < length; i++)
            {
                questStatus.StartQuest(FailureQuests[i]);
            }
            length = FailureInputs.Count;
            for (int i = 0; i < length; i++)
            {
                questStatus.UpdateQuests(FailureInputs[i]);
            }
        }

        // The quests in this list will go from NotStarted to Stared if the current quest succeeds.
        [SerializeField]  protected List<Quest> SuccessQuests;
        [SerializeField]  protected List<InputUnit> SuccessInputs;

        [SerializeField]  protected List<Quest> FailureQuests;
        [SerializeField]  protected List<InputUnit> FailureInputs;
    }
}

