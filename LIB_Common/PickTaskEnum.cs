namespace LIB_Common
{
    public enum PicktaskExecuteStatus
    {
        Transport = 10,
        Pick = 20,
        Picking = 30,
        Picked = 40,
        UnFullPicked = 50,
        Cancelled = 60,
        Terminated = 70,
        Faulted = 80,
        Timeout = 90,
        Resume = 100,
        CycleCount= 110,

        //Ready = 10,
        //Release = 20,
        //Active = 30,
        //Suspended = 40,
        //Completed = 50,
        //Cancelled = 60,
        //Terminated = 70,
        //Faulted = 80,
        //Timeout = 90,
        //Resume = 100
    }

    public enum ESubmitPicktaskStatus
    {
        Pick = 10,
        Pause = 20,
        ForceComplete = 30,
        Cancelled = 40
    }

}
