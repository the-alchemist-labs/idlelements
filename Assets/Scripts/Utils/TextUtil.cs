using System;

public static class TextUtil
{
    public static string NumberFormatter(int number)
    {
        if (number >= 1000 && number < 1000000)
        {
            // Format numbers in thousands with a 'K' suffix
            return (number / 1000.0).ToString("0.0K");
        }
        else if (number >= 1000000)
        {
            // Format numbers in millions with an 'M' suffix
            return (number / 1000000.0).ToString("0.00M");
        }
        else
        {
            // Format numbers less than 1000 with commas
            return number.ToString("N0");
        }
    }

    public static string FormatSecondsToTimeString(float seconds)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        return timeSpan.ToString(@"hh\:mm\:ss");
    }
}

