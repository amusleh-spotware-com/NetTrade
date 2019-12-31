using NetTrade.Abstractions.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace NetTrade.Helpers
{
    public static class OptimizerParameterGridCreator
    {
        public static List<Dictionary<string, object>> GetParameterGrid(IEnumerable<IOptimizeParameter> parameters)
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
                    var currentParameterGrid = new Dictionary<string, object>
                    {
                        { currentParameter.Name, currentValue},
                    };

                    var parametersWithoutCurrentParameter = parameters.Skip(1);

                    var otherParameterGrids = GetParameterGrid(parametersWithoutCurrentParameter);

                    if (otherParameterGrids.Any())
                    {
                        foreach (var otherParameterGrid in otherParameterGrids)
                        {
                            var newParameterGrid = currentParameterGrid.Concat(otherParameterGrid)
                                .ToDictionary(nameAndValue => nameAndValue.Key, nameAndValue => nameAndValue.Value);

                            result.Add(newParameterGrid);
                        }
                    }
                    else
                    {
                        result.Add(currentParameterGrid);
                    }
                }

                return result;
            }
        }
    }
}