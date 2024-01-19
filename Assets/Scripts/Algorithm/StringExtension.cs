using System.Collections.Generic;

namespace Algorithm
{
    public static class StringExtension
    {
        /// <summary>
        /// 将字符串组合
        /// </summary>
        /// <param name="o">字符串集合</param>
        /// <param name="separator">分割符</param>
        /// <returns></returns>
        public static string JoinWithSeparator<T>(this T o, string separator) where T: IEnumerable<string>
        {
            return string.Join(separator, o);
        }
        
    }
}