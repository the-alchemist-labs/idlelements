using System;

public class Notification
{
    public DateTime Date { get; set; }
    public string Message { get; set; }

    public Notification(string message)
    {
        Message = message;
        Date = DateTime.Now;
    }
}