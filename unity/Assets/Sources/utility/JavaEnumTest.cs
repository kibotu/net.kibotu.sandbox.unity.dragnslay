using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Assets.Sources.utility
{
    // inspired by http://stackoverflow.com/a/4778347
    public static class JavaEnumTest
    {
        public enum Day
        {
            [DayAttr("Montag")]
            Monday,
            [DayAttr("Dienstag")]
            Thuesday,
            [DayAttr("Mittwoch")]
            Wednesday,
            [DayAttr("Donnerstag")]
            Thursday,
            [DayAttr("Freitag")]
            Friday,
            [DayAttr("Samstag")]
            Saturday,
            [DayAttr("Sonntag")]
            Sunday,
        }

        public static void TestEnum()
        {
            var d = ValueOf<Day>("Wednesday");
            Debug.Log(d + " is the " + (d.Ordinal() + 1) + "/" + Length<Day>() + " Day of the week.");
            Debug.Log(d.OpenCookie());

            Debug.Log(d.GetAttr().OpenCookie());

            // enum implements interface
            // IOpenCookie opener = d.GetAttr<Day, DayAttr>();
            IOpenCookie opener = d.GetAttr();
            Debug.Log(opener.OpenCookie());
        }

        public static int Ordinal<T>(this T _this) where T : struct
        {
            return (typeof (T).IsEnum) ? Convert.ToInt32(_this) : -1;
        }

        public static int Length<T>() where T : struct
        {
            return  (typeof (T).IsEnum) ? Enum.GetValues(typeof(T)).Cast<T>().Distinct().Count() : - 1;
        }

        public static string Name<T>(this T _this) where T : struct
        {
            return (typeof (T).IsEnum) ? _this.ToString() : null;
        }

        public static T ValueOf<T>(string typename) where T : struct
        {
            // return (typeof(T).IsEnum) ? (T)Enum.Parse(typeof(T), typename) : null; // hwy is it not working? q.q
            return (T)Enum.Parse(typeof(T), typename);
        }

        public static TR GetAttr<T, TR>(this T _this)
            where T : struct
            where TR : Attribute
        {
            return (TR)Attribute.GetCustomAttribute(ForValue(_this), typeof(Attribute));
        }

        public static DayAttr GetAttr(this Day _this)
        {
            return (DayAttr)Attribute.GetCustomAttribute(ForValue(_this), typeof(Attribute));
        }

        public static MemberInfo ForValue<T>(this T _this) where T : struct
        {
            return typeof(T).GetField(Enum.GetName(typeof(T), _this));
        }

        public interface IOpenCookie
        {
            string OpenCookie();
        }

        public class DayAttr : Attribute, IOpenCookie
        {
            internal DayAttr(string s) { this.s = s; }
            public string s { get; private set; }

            public string OpenCookie()
            {
                return "Opening cookie on " + s + ".";
            }
        }

        public static string OpenCookie(this Day _this)
        {
            return _this.GetAttr<Day, DayAttr>().OpenCookie();
        }
    }
}
