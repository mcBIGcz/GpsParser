using System;
using System.Linq;
using System.Xml;

namespace Bakalarka
{
    static class GpsParser
    {
        public static bool TryParseCoord(String coord, out double value)
        {
            string[] separator = new string[] { " " };
            string replaced = coord.Replace(',', '.').Replace('°', ' ').Replace('\'', ' ')
                .Replace('"', ' ').Replace('N', ' ').Replace('E', ' ');
            String[] parts = replaced.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            try
            {
                switch (parts.Length)
                {
                    case 1:
                        value = parseDFormat(parts);
                        return true;

                    case 2:
                        value = parseDMFormat(parts);
                        return true;

                    case 3:
                        value = parseDMSFormat(parts);
                        return true;

                    default:
                        value = double.NaN;
                        return false;
                }
            }
            catch (Exception ex)
            {
                value = double.NaN;
                return false;
            }
        }

        private static double parseDFormat(String[] parts)
        {
            String value = parts[0];
            return XmlConvert.ToDouble(value);
        }

        private static double parseDMFormat(String[] parts)
        {
            String valueD = parts[0];
            String valueM = parts[1];


            double retVal = 0;
            bool negative = valueD.Contains('-');
            retVal += XmlConvert.ToDouble(valueD);

            double minutes = (XmlConvert.ToDouble(valueM) / 60);
            if (minutes >= 60 || minutes < 0) throw new FormatException();

            retVal += (negative) ? -minutes : minutes;

            return retVal;
        }

        private static double parseDMSFormat(String[] parts)
        {
            String valueD = parts[0];
            String valueM = parts[1];
            String valueS = parts[2];


            double retVal = 0;
            bool negative = valueD.Contains('-');

            retVal += XmlConvert.ToDouble(valueD);

            double minutes = (XmlConvert.ToDouble(valueM) / 60);
            if (minutes >= 60 || minutes < 0 || valueM.Contains('.')) throw new FormatException();
            retVal += (negative) ? -minutes : minutes;

            double seconds = (XmlConvert.ToDouble(valueS) / 3600);
            if (seconds >= 60 || seconds < 0) throw new FormatException();
            retVal += (negative) ? -seconds : seconds;

            return retVal;
        }

    }
}
