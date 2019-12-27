using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Interfaces;
using System.Linq;

namespace NetTrade.Helpers
{
    public static class OptimizerParameterSetsCalculator
    {
        public static List<Dictionary<string, object>> GetAllParameterSets(IEnumerable<IOptimizeParameter> parameters)
        {
            if (!parameters.Any())
            {
                return new List<Dictionary<string, object>>();
            }
            else
            {
                var result = new List<Dictionary<string, object>>();

                var currentParameter = parameters.First();

                foreach (var x in currentParameter.Values)
                {
                    var currentSet = new Dictionary<string, object>
                    { 
                        { currentParameter.Name, x},
                    };

                    var parametersWithoutCurrentParameter = parameters.Skip(1);

                    foreach (var y in GetAllParameterSets(parametersWithoutCurrentParameter))
                    {
                        var newSet = currentSet.Concat(y)
                            .ToDictionary(nameAndValue => nameAndValue.Key, nameAndValue => nameAndValue.Value);

                        result.Add(newSet);
                    }
                }

                return result;
            }
        }
    }
}
