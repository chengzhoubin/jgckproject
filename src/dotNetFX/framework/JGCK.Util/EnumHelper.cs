using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JGCK.Util
{
    public class EnumHelper
    {
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

        public static IList<SelectListItem> GetSelectItemByEnum<T>(string selectValue)
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList()
                .Select(m => new SelectListItem
                {
                    Value = m.ToString(),
                    Text = GetEnumDisplayName(m),
                    Selected = (string.Equals(m.ToString(), selectValue))
                }).ToList();
        }

        public static IList<SelectListItem> GetSelectItemByEnum<T>(bool toInsertNoticeTitle = false, string defaultText = "--请选择--", string defaultValue = "-1")
        {
            var items = new List<SelectListItem>();
            if (toInsertNoticeTitle)
            {
                items.Add(new SelectListItem
                {
                    Value = defaultValue,
                    Text = defaultText,
                    Selected = true
                });
            }
            Enum.GetValues(typeof(T)).Cast<T>().ToList()
            .ForEach(m => items.Add(new SelectListItem
            {
                Value = m.ToString(),
                Text = GetEnumDisplayName(m)
            }));
            return items;
        }

        public static IList<SelectListItem> GetSelectHtmlTag<TEnum>(TEnum selectedEnum)
        {
            var vmList = new List<SelectListItem>();
            Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList()
            .ForEach(m => vmList.Add(new SelectListItem
            {
                Value = m.ToString(),
                Text = GetEnumDisplayName(m)
            }));
            if (vmList.Count > 0)
            {
                string name = GetEnumDisplayName(selectedEnum);
                SelectListItem selectedItem = vmList.Where(m => m.Text == name).FirstOrDefault();
                if (selectedItem != null)
                    selectedItem.Selected = true;
                else
                    vmList[0].Selected = true;
            }
            return vmList;
        }

        public static IList<SelectListItem> GetSelectHtmlTag<TEnum>()
        {
            var items = new List<SelectListItem>();
            items.Add(new SelectListItem
            {
                Text = "请选择",
                Value = "0",
                Selected = true
            });
            Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList()
                .ForEach(m => items.Add(new SelectListItem
                {
                    Value = ((int) Enum.ToObject(typeof(TEnum), m)).ToString(),
                    Text = GetEnumDisplayName(m)
                }));
            return items;
        }

        public static IList<SelectListItem> GetSelectHtmlTag(Guid selectedId, List<IdAndName> nameList)
        {
            var vmList = new List<SelectListItem>();
            foreach (var item in nameList)
            {
                vmList.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            if (vmList.Count > 0)
            {
                SelectListItem selectedItem = vmList.Where(m => m.Value == selectedId.ToString()).FirstOrDefault();
                if (selectedItem != null)
                    selectedItem.Selected = true;
                else
                    vmList[0].Selected = true;
            }
            return vmList;
        }

        public static IList<SelectListItem> GetSelectHtmlTag(List<IdAndName> nameList)
        {
            var vmList = new List<SelectListItem>();
            vmList.Add(new SelectListItem { Text = "请选择", Value = Guid.Empty.ToString(), Selected = true });
            foreach (var item in nameList)
            {
                vmList.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            return vmList;
        }
    }

    public class IdAndName
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}