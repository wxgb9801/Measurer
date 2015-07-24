namespace LIB_Common
{
    public enum EIATaskType
    {
        AssignTask= 10,
        ExecuteBook =20,
        RollBack= 30,
        OrderInventory =40,
        CycleCountInventory=50,
        CycleCount=60
    }

    public enum EIARollBackType
    {
        PreAssignRollback = 10,
        AssignRollback = 20
    }

    public enum EPickArea
    {
        NONE = 0,
        ASRS = 10,
        FULLCARTON = 20,
        SPLIT = 30
    }
}
