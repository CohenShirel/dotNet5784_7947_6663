/// <summary>
/// 
/// </summary>
/// <param זהות מספר="IdWorker"></param>
/// <param ניסיון="Experience"></param>
/// <param לשעה שכר="HourSalary"></param>
/// <param שם="Name"></param>
/// <param מייל="Email"></param>
namespace DO;

public record Worker
(
    int IdWorker,
    Level Experience,
    int HourSalary,
    string? Name = null,
    string? Email = null
)
{
    //empty ctor
    public Worker() : this(0, 0, 0)
    {

    }
}

