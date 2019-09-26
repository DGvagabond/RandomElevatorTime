using System;
using System.Collections.Generic;
using Smod2.API;
using Smod2.EventHandlers;
using Smod2.Events;

namespace RandomElevatorTimer
{
	class ElevatorUseEvent : IEventHandlerElevatorUse
	{
		private readonly ElevatorTimer plugin;
		private readonly Random random;

		private const float MIN_DISTANCE = 3 * 3; // 3 (unity) units? idk how the secret lab distance thing works.

		public ElevatorUseEvent(ElevatorTimer plugin)
		{
			this.plugin = plugin;
			random = new Random();
		}

		public void OnElevatorUse(PlayerElevatorUseEvent ev)
		{
			if ((ev.Elevator.GetComponent() as Lift).operative)
			{
				return;
			}

			float newSpeed = (float)plugin.WeightedRandomizer.GetValue();

			ev.Elevator.MovingSpeed = newSpeed;

			// if announcing is on and it's above the ejaoasidasiojd blah blah
			if ((Math.Abs(plugin.BroadcastValue) < 1) || newSpeed < plugin.BroadcastValue)
			{
				return;
			}

			// get all players close to him...
			List<Player> allPlayers = plugin.Server.GetPlayers();

			string message = plugin.BroadcastMessage.Replace("$seconds$", $"{Math.Floor(newSpeed)}");

			uint time = plugin.BroadCastTime;

			Vector playerPos = ev.Player.GetPosition();

			foreach (Player player in allPlayers)
			{
				if (player.GetHealth() == 0)
				{
					continue;
				}

				if (GetMagnitude(player.GetPosition(), playerPos) < MIN_DISTANCE)
				{
					player.PersonalBroadcast(time, message, false);
				}
			}
		}

		private float GetMagnitude(Vector one, Vector two)
		{
			float x = one.x - two.x;
			float y = one.y - two.y;
			float z = one.z - two.z;

			// that should be a squared magnitude right? yeah. speedy.
			return x * x + y * y + z * z;
		}
	}
}
