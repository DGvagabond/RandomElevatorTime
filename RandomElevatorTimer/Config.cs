// -----------------------------------------------------------------------
// <copyright file="Config.cs">
// Copyright (c) DGvagabond. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RandomElevatorTimer
{
    using System.ComponentModel;
    using Exiled.API.Interfaces;
    
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; }
        public bool Debug { get; set; }
        [Description("The values that the elevator time can be set at")]
        public string TimerValues { get; set; } = "(4-6:60)(6-10:5)(10-15:1)";
        [Description("Minimum amount of seconds before broadcasting the time to all players near the elevator")]
        public float BroadcastValue { get; set; } = 7;
        [Description("The message that will be broadcasted to all users. Replacement string for seconds is \"$seconds$\"")]
        public string BroadcastMessage { get; set; } = "This elevator will take $seconds$ seconds to arrive.";
        [Description("The time the message will be broadcasted for.")]
        public float BroadcastTime { get; set; } = 2;
    }
}