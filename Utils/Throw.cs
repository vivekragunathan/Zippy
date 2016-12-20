using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Zippy.Utils
{
    public class Throw
    {

        #region Collection Methods

        public static void IfNullOrEmpty<T>(IList<T> list, string fmtSpec, params object[] args)
        {
            bool cond = list == null || list.Count == 0;
            Throw.IfTrue(cond, fmtSpec, args);
        }

        #endregion

        #region IfTrue Methods

        public static void IfBlank(string text, string message)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException(
                    message ?? $"{nameof(text)} cannot be null/empty/blank",
                    nameof(text)
                );
            }
        }

        public static void IfBlank<T>(string text, T exception) where T : Exception
        {
            Throw.IfTrue(string.IsNullOrWhiteSpace(text), exception);
        }

        public static void IfTrue(bool condition, string errorMessage)
        {
            Throw.IfTrue(condition, new Exception(errorMessage));
        }

        public static void IfTrue(bool condition, string fmtSpec, params object[] args)
        {
            Debug.Assert(fmtSpec != null);
            var errorText = args == null ? fmtSpec : string.Format(fmtSpec, args);
            Throw.IfTrue(condition, new Exception(errorText));
        }

        public static void IfTrue(bool condition, Exception ex)
        {
            Debug.Assert(ex != null);

            if (condition)
            {
                throw ex;
            }
        }

        #endregion

        #region IfFalse Methods

        public static void IfFalse(bool condition, string errorMessage)
        {
            Throw.IfTrue(!condition, errorMessage);
        }

        public static void IfFalse(bool condition, Exception ex)
        {
            Throw.IfTrue(!condition, ex);
        }

        public static void IfFalse(bool condition, string fmtSpec, params object[] args)
        {
            Throw.IfTrue(!condition, fmtSpec, args);
        }

        #endregion

        #region IfNull Methods

        public static void IfNull<T>(T objRef, string errorMessage) where T : class
        {
            Throw.IfTrue(objRef == null, errorMessage);
        }

        private static void IfNull<T>(T objRef, string fmtSpec, object[] args) where T : class
        {
            Throw.IfTrue(objRef == null, fmtSpec, args);
        }

        public static void IfNull<T>(T objRef, Exception ex) where T : class
        {
            Throw.IfTrue(objRef == null, ex);
        }

        #endregion

        #region IfNotNull Methods

        public static void IfNotNull<T>(T objRef, string errorMessage) where T : class
        {
            Throw.IfTrue(objRef != null, errorMessage);
        }

        public static void IfNotNull<T>(T objRef, string fmtSpec, params object[] args) where T : class
        {
            Throw.IfTrue(objRef != null, fmtSpec, args);
        }

        public static void IfNotNull<T>(T objRef, Exception ex) where T : class
        {
            Throw.IfTrue(objRef != null, ex);
        }

        #endregion
    }
}