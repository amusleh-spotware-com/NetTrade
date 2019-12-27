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

                foreach (var currentValue in currentParameter.Values)
                {
                    var currentSet = new Dictionary<string, object>
                    { 
                        { currentParameter.Name, currentValue},
                    };

                    var parametersWithoutCurrentParameter = parameters.Skip(1);

                    var otherParametersSets = GetAllParameterSets(parametersWithoutCurrentParameter);

                    if (otherParametersSets.Any())
                    {
                        foreach (var otherParametersSet in otherParametersSets)
                        {
                            var newSet = currentSet.Concat(otherParametersSet)
                                .ToDictionary(nameAndValue => nameAndValue.Key, nameAndValue => nameAndValue.Value);

                            result.Add(newSet);
                        }
                    }
                    else
                    {
                        result.Add(currentSet);
                    }
                }

                return result;
            }
        }
    }
}
