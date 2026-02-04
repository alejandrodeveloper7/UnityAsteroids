namespace ACG.Core.EventBus
{
    public interface IEvent 
    {    
    }
}

#region Event Examples

//ReadOnly struct is better for 1 or 2 primitives

//public readonly struct StopMusic : IEvent
//{
//    public readonly float ProgressivelyStopDuration;

//    public StopMusic(float progressivelyStopDuration)
//    {
//        ProgressivelyStopDuration = progressivelyStopDuration;
//    }
//}


//Sealed class is better for classes or more than 2 primitives

//public sealed class Generate2DSound : IEvent
//{
//    public List<SO_Sound> SoundsData { get; set; }

//    public Generate2DSound(List<SO_Sound> soundsData)
//    {
//        SoundsData = soundsData;
//    }
//}

#endregion