using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGCK.Util
{
    public static class EnumComm
    {
        /// <summary>
        /// 获取枚举的描述文本
        /// </summary>
        /// <param name="e">枚举成员</param>    
        /// <returns></returns>
        public static string GetEnumDescription(object e)
        {
            //获取字段信息
            System.Reflection.FieldInfo[] ms = e.GetType().GetFields();

            Type t = e.GetType();
            foreach (System.Reflection.FieldInfo f in ms)
            {
                //判断名称是否相等
                if (f.Name != e.ToString()) continue;

                //反射出自定义属性
                foreach (Attribute attr in f.GetCustomAttributes(true))
                {
                    //类型转换找到一个Description，用Description作为成员名称
                    System.ComponentModel.DescriptionAttribute dscript = attr as System.ComponentModel.DescriptionAttribute;
                    if (dscript != null)
                        return dscript.Description;
                }

            }
            //如果没有检测到合适的注释，则用默认名称
            return e.ToString();
        }

        public static string GetEnumDisplayName(object o)
        {
            return o.GetType()
                .GetMember(o.ToString())
                .First()
                .GetCustomAttributes(false)
                .OfType<DisplayAttribute>()
                .LastOrDefault()
                .Name;
        }
    }
}
