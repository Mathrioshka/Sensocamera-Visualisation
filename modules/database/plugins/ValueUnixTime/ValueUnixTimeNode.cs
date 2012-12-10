#region usings
using System;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

using VVVV.Core.Logging;
#endregion usings

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "UnixTime", Category = "Value", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class ValueUnixTimeNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input", DefaultValue = 1.0)]
		IDiffSpread<string> FInput;
		
		[Input("Refresh", IsSingle = true, IsBang = true)]
		IDiffSpread<bool> FRefreshIn;

		[Output("Output")]
		ISpread<double> FOutput;

		[Import()]
		ILogger FLogger;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			FOutput.SliceCount = SpreadMax;
			
			if(FInput.IsChanged || FRefreshIn[0])
			{
				for(int i = 0; i < SpreadMax; i++)
				{
					try
					{
						var date = DateTime.Parse(FInput[i]);
						FOutput[i] = ConvertToTimestamp(date);
					}
					catch
					{
						FOutput[i] = -1;
					}
				}
			}			
		}
		
		private double ConvertToTimestamp(DateTime value)
		{
    		TimeSpan span = value - new DateTime(1970, 1, 1, 0, 0, 0, 0);
			
    		return (double)span.TotalSeconds;
		}
	}
}
