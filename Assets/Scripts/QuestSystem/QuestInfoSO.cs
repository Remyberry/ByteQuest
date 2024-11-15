using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "Quest/QuestInfoSO", order = 1)]
public class QuestInfoSO : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }

    [Header("General")]
    public string displayName;
    [Header("Requirements")]
    public int levelRequirements;
    public QuestInfoSO[] questPrerequisites;
    [Header("Steps")]
    public GameObject[] questStepsPrefabs;
    [Header("Rewards")]
    public GameObject[] questItems;
    public int goldReward;

    private void OnValidate()
    {
        #if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}
