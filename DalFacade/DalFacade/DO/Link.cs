

namespace DO;
//תלויות
public record Link
(
   int IdLink,//מספר מזהה 
   int IdAssignments,//משימה תלויה
   int IdPAssignments//משימה קודמת
)
{

}
