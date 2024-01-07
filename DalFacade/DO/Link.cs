

namespace DO;
using DO;
using DalApi;
/// <summary>
/// 
/// </summary>
/// <param  מזהה מספר ="IdLink"></param>
/// <param  תלויה משימה ="IdAssignments"></param>
/// <param קודמת משימה="IdPAssignments"></param>
public record Link
(
   int IdLink,
   int IdAssignments,
   int IdPAssignments
);
//empty ctor
public Link() : this(0, 0, 0)
{

}