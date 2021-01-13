using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SMP.Helpers
{
    public class EnumsToSelectList<T>
    {
        public static SelectList GetSelectList(int [] include)
        {
            //var values = from T e in Enum.GetValues(typeof(T))
            //             select new { ID = Convert.ToInt16(e), Name = e.ToString() };

            var list = new List<KeyValuePair<Enum, string>>();
            foreach (Enum value in Enum.GetValues(typeof(T)))
            {
                string description = value.ToString();
                FieldInfo fieldInfo = value.GetType().GetField(description);
                var attribute = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).First();
                if (attribute != null)
                {
                    description = (attribute as DescriptionAttribute).Description;
                }
                list.Add(new KeyValuePair<Enum, string>(value, description));
            }
            var values = from e in list
                         select new { ID = Convert.ToInt16(e.Key), Name = e.Value };

            values = values.Where(q => include.Contains(q.ID));

            return new SelectList(values, "ID", "Name");
        }

        public static SelectList GetSelectList1(int? selected)
        {
            //var values = from T e in Enum.GetValues(typeof(T))
            //             select new { ID = Convert.ToInt16(e), Name = e.ToString() };

            var list = new List<KeyValuePair<Enum, string>>();
            foreach (Enum value in Enum.GetValues(typeof(T)))
            {
                string description = value.ToString();
                FieldInfo fieldInfo = value.GetType().GetField(description);
                var attribute = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).First();
                if (attribute != null)
                {
                    description = (attribute as DescriptionAttribute).Description;
                }
                list.Add(new KeyValuePair<Enum, string>(value, description));
            }

            var values = from e in list
                         select new { ID = Convert.ToInt16(e.Key), Name = e.Value };

            if (selected != null)
            {
                return new SelectList(values, "ID", "Name", selectedValue: selected);
            }
            else
            {
                return new SelectList(values, "ID", "Name");
            }

        }


        public static SelectList GetSelectList(int selectedObj)
        {
            //var values = from T e in Enum.GetValues(typeof(T))
            //             select new { ID = Convert.ToInt16(e), Name = e.ToString() };

            var list = new List<KeyValuePair<Enum, string>>();
            foreach (Enum value in Enum.GetValues(typeof(T)))
            {
                string description = value.ToString();
                FieldInfo fieldInfo = value.GetType().GetField(description);
                var attribute = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).First();
                if (attribute != null)
                {
                    description = (attribute as DescriptionAttribute).Description;
                }
                list.Add(new KeyValuePair<Enum, string>(value, description));
            }

            var values = from e in list
                         select new { ID = Convert.ToInt16(e.Key), Name = e.Value };

            return new SelectList(values, "ID", "Name", selectedObj);
        }
    }
}
