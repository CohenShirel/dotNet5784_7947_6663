﻿/// <summary>
/// 
/// </summary>
/// <param זהות מספר="Id"></param>
/// <param ניסיון="Experience"></param>
/// <param לשעה שכר="HourSalary"></param>
/// <param שם="Name"></param>
/// <param מייל="Email"></param>
namespace DO;
public record Worker
(
    int Id,
    Level Experience,
    int HourSalary,
    string? Name = null,
    string? Email = null
)
{
    public Worker() : this(0, 0, 0)
    {

    }
}


