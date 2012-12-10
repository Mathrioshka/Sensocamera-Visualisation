#region usings
using System;
using System.ComponentModel.Composition;
using System.Collections.Generic;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

using VVVV.Core.Logging;
#endregion usings

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "GetSession", Category = "Value", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class ValueGetSessionNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input", DefaultValue = 1.0)]
		IDiffSpread<double> FInput;
		
		[Input("Max Delay (Seconds)", DefaultValue = 300, IsSingle = true)]
		IDiffSpread<double> FMaxDelayIn;

		[Output("Output")]
		ISpread<ISpread<double>> FOutput;
		
		List<List<double>> FData = new List<List<double>>();

		[Import()]
		ILogger FLogger;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			if(FInput.IsChanged || FMaxDelayIn.IsChanged)
			{
				FData.Clear();
				
				double pValue = -1;
				List<double> pList = new List<double>();
				FData.Add(pList);
				
				for (int i = 0; i < SpreadMax; i++)
				{
					var value = FInput[i];
					
					if(value - pValue > FMaxDelayIn[0] && pValue > 0)
					{
						pList = new List<double>();
						FData.Add(pList);
					}
					
					pList.Add(value);
					pValue = value;
				}
				
				var count = FData.Count;
				FOutput.SliceCount = count;
				
				for(int i = 0; i < count; i++)
				{
					FOutput[i].AssignFrom(FData[i]);
				}
			}
		}
	}
}
