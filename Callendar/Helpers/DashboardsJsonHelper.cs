using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Callendar.Helpers
{
	public class DashboardsJsonHelper
	{
		public static IEnumerable<string> GetDashboard(User user)
		{
			if (user != null && positions.TryGetValue(user.Position.Name, out EPosition position))
			{
				switch (position)
				{
					case EPosition.Telemarketer: return TelemarketerDashboard(user);
					case EPosition.Accountant: return AccountantDashboard(user);
					case EPosition.Lider: return LiderDashboard(user);
				}
			}

			return new string[] { };
		}

		#region Dashboards

		private static IEnumerable<string> TelemarketerDashboard(User user)
		{
			//TODO
			return new string[] { "marketer" };
		}

		private static IEnumerable<string> AccountantDashboard(User user)
		{
			//TODO
			return new string[] { "accountant" };
		}

		private static IEnumerable<string> LiderDashboard(User user)
		{
			//TODO
			return new string[] { "lider" };
		}

		#endregion

		#region Positions

		private enum EPosition
		{
			Telemarketer,
			Accountant,
			Lider
		}

		private static Dictionary<string, EPosition> positions = new Dictionary<string, EPosition>
		{
			{"Marketer", EPosition.Telemarketer},
			{"Accountant", EPosition.Accountant},
			{"Lider", EPosition.Lider}
		};

		#endregion
	}
}
