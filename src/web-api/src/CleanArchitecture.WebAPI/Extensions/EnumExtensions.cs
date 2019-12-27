using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.WebAPI.Extensions
{
    public static class EnumExtensions
    {
        public static TDestEnum ToEnum<TDestEnum>(this IConvertible sourceEnum)
                    where TDestEnum : struct, IConvertible
        {
            if (!typeof(TDestEnum).IsEnum)
                throw new ArgumentException("TDestEnum must be an enum type");

            if (sourceEnum == null)
                throw new ArgumentNullException(nameof(sourceEnum));

            if (!sourceEnum.GetType().IsEnum && !(sourceEnum is int))
                throw new ArgumentException("SourceEnum must be an enum type");

            return (TDestEnum)Enum.Parse(typeof(TDestEnum), sourceEnum.ToString(CultureInfo.InvariantCulture));
        }
    }
}
