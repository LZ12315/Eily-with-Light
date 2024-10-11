
/// <summary>
/// 一个不搭载到任何物体上的脚本 负责写一些Enum变量供开发者使用
/// 依次是 场景类型、DataUID类型、存档序号
/// </summary>

public enum SceneType
{
    GameScene,MenuScene
};

public enum PersistentType
{
    ReadWrite,OnltRead
};

public enum ArchieveNumber
{
    Ar1,Ar2, Ar3
};
