// -----------------------------------------------------------------------
// <copyright file="EventHandlers.cs">
// Copyright (c) DGvagabond. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RandomElevatorTimer
{
	using System;
	using Exiled.API.Features;
	using Exiled.Events.EventArgs.Player;
	using UnityEngine;
	
	internal class EventHandlers
	{

		private const float MinDistance = 3 * 3;

		public void OnElevatorUse(InteractingElevatorEventArgs ev)
		{
			if (!ev.Elevator.IsReady) return;

			var newSpeed = (float)ElevatorTimer.Instance.WeightedRandomizer.GetValue();

			ev.Elevator._animationTime = newSpeed;

			if (Math.Abs(ElevatorTimer.Instance.Config.BroadcastValue) < 1 || newSpeed < ElevatorTimer.Instance.Config.BroadcastValue) return;
			
			var allPlayers = Player.List;

			var message = ElevatorTimer.Instance.Config.BroadcastMessage.Replace("$seconds$", $"{Math.Floor(newSpeed)}");

			var time = Math.Min(ElevatorTimer.Instance.Config.BroadcastTime, (uint)Math.Floor(newSpeed));

			var playerPos = ev.Player.Position;

			foreach (var player in allPlayers)
			{
				if (player.Health == 0)
				{
					continue;
				}

				if (GetMagnitude(player.Position, playerPos) < MinDistance)
				{
					player.Broadcast((ushort)time, message);
				}
			}
		}

		private float GetMagnitude(Vector3 one, Vector3 two)
		{
			var x = one.x - two.x;
			var y = one.y - two.y;
			var z = one.z - two.z;

			return x * x + y * y + z * z;
		}
	}
}
