

namespace DO;
/// <summary>
/// 
/// </summary>
/// <param זהות מספר="IdWorker"></param>
/// <param ניסיון="Experience"></param>
/// <param לשעה שכר="HourSalary"></param>
/// <param שם="Name"></param>
/// <param מייל="Email"></param>
public record Worker
(
    int IdWorker,
    int Experience,
    double HourSalary,
    string? Name=null,
    string? Email = null
)
{ 
    //empty ctor
    public Worker(): this(0, 0, 0)   
    {  

    }
    //ctor
    public Worker()
    {

    }
}



