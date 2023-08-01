namespace Tada.Server.Shared
{
    public static class ConvertUtil
    {
        public static int ToInt(this object obj)
        {
            if (obj == null)
            {
                return 0;
            }

            if (!int.TryParse(obj.ToString(), out var val))
            {
                return val;
            }
            else
            {
                if (!Convert.IsDBNull(obj))
                {
                    return Convert.ToInt32(obj);
                }
                return 0;
            }
        }

        public static double ToDouble(this object obj)
        {
            if (obj == null)
            {
                return 0;
            }

            if (!double.TryParse(obj.ToString(), out var val))
            {
                return val;
            }
            else
            {
                if (!Convert.IsDBNull(obj))
                {
                    return Convert.ToDouble(obj);
                }
                return 0;
            }
        }

        public static DateTime ToDateTime(this object obj)
        {
            if (obj == null)
            {
                return new DateTime();
            }

            if (!DateTime.TryParse(obj.ToString(), out var val))
            {
                return val;
            }
            else
            {
                if (!Convert.IsDBNull(obj))
                {
                    return Convert.ToDateTime(obj);
                }
                return new DateTime();
            }
        }

        public static DateTime? ToDateTimeNullable(this object obj)
        {
            if (obj == null || obj.ToString() == "")
            {
                return null;
            }

            if (!DateTime.TryParse(obj.ToString(), out var val))
            {
                return val;
            }
            else
            {
                if (!Convert.IsDBNull(obj))
                {
                    return Convert.ToDateTime(obj);
                }
                return null;
            }
        }

        public static bool ToBool(this object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!bool.TryParse(obj.ToString(), out var val))
            {
                return val;
            }
            else
            {
                if (!Convert.IsDBNull(obj))
                {
                    return Convert.ToBoolean(obj);
                }
                return false;
            }
        }

        public static string ToString(this object obj)
        {
            if (obj == null)
            {
                return "";
            }

            if (!Convert.IsDBNull(obj))
            {
                // nullの場合は空文字を返す
                var ret = Convert.ToString(obj);
                if (ret is null)
                {
                    return "";
                }
                return ret;
            }
            return "";
        }

    }
}
