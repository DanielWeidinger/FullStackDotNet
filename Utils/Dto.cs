using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public static class Dto
    {
        public static R SetProps<T, R>(this T dto) where R : new()
        {
            R result = new R();

            Type resultEntityType = result.GetType();
            Type dtoType = dto.GetType();

            resultEntityType
                .GetProperties()
                .Where(resProp => dtoType
                    .GetProperties()
                    .Any(dtoProp => dtoProp.Name == resProp.Name)
                )
                .ToList()
                .ForEach(resProp => resProp
                    .SetValue(result, dtoType
                        .GetProperty(resProp.Name)
                        .GetValue(dto)
                    )
                );

            return result;
        }
    }
}
