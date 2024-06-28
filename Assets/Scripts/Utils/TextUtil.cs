using System;

public static class TextUtil
{
    public static string NumberFormatter(int number)
    {
        if (number >= 1000 && number < 1000000)
        {
            double value = number / 1000.0;
            return (value % 1 == 0 ? value.ToString("0") : value.ToString("0.0")) + "K";
        }
        else if (number >= 1000000)
        {
            double value = number / 1000000.0;
            return (value % 1 == 0 ? value.ToString("0") : value.ToString("0.00")) + "M";
        }
        else
        {
            return number.ToString("N0");
        }
    }

    public static string FormatSecondsToTimeString(float seconds)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        return timeSpan.ToString(@"hh\:mm\:ss");
    }
}

