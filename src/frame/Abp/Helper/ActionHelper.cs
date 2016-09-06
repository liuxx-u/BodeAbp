using System;

namespace Abp.Helper
{
    public class ActionHelper
    {
        public static void EatException(Action action)
        {
            try
            {
                action();
            }
            catch (Exception)
            {
            }
        }
        public static T EatException<T>(Func<T> action, T defaultValue = default(T))
        {
            try
            {
                return action();
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }
}
