using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprocket.UpdateMonitor
{
	public class FormResult
	{
		public enum ResultType
		{
			None,
			Confirm,
			Cancel
		}

		private ResultType resultValue = ResultType.None;

		public ResultType Value
		{
			get
			{
				return resultValue;
			}

			set
			{
				resultValue = value;
				ready = true;
			}
		}

		private bool ready = false;

		public bool IsReady
		{
			get
			{
				return ready;
			}
		}
	}
}
