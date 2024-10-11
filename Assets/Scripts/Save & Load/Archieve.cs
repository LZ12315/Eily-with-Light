using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "DataSO/ArchieveData")]
public class Archieve : ScriptableObject
{
    public ArchieveNumber archieveNumber;
    public string fileName;
    public float savings;
    public GameSceneSO lastScene;

    public SceneData sceneData;
}
