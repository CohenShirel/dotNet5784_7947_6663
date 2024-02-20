namespace DO;
/// <summary>
/// 
/// </summary>
/// <param  מזהה מספר ="IdLink"></param>
/// <param  תלויה משימה ="IdAssignment"></param>
/// <param קודמת משימה="IdPAssignment"></param>
public record Link
(
   int IdLink,
   int IdAssignment,
   int IdPAssignment
)
{
    //empty ctor
    public Link() : this(0, 0, 0)
    {

    }
}
