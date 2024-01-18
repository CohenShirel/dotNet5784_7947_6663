

using DalApi;
using DO;

namespace Dal;

internal class AssignmentsImplementation : IAssignments
{
    readonly string s_assignmentss_xml = "assignmentss";
    public int Create(Assignments ass)
    {
        List<DO.Assignments> assignmentss = XMLTools.LoadListFromXMLSerializer<DO.Assignments>(s_assignmentss_xml);
        Assignments newAssignments = ass with { IdAssignments = Config.IdPAssignments };
        assignmentss.Add(newAssignments);
        XMLTools.SaveListToXMLSerializer<DO.Assignments>(assignmentss, s_assignmentss_xml);
        return newAssignments.IdAssignments;
    }

    public void Delete(int id)
    {
        List<DO.Assignments> assignmentss = XMLTools.LoadListFromXMLSerializer<DO.Assignments>(s_assignmentss_xml);
        if (assignmentss.RemoveAll(item => item.IdAssignments == id) == 0)
        {
            throw new DalDoesNotExistException($"Assignments with id {id} does not exist");
        }
        XMLTools.SaveListToXMLSerializer<DO.Assignments>(assignmentss, s_assignmentss_xml);
    }

    public Assignments? Read(Func<Assignments, bool> filter)
    {
        List<DO.Assignments> assignmentss = XMLTools.LoadListFromXMLSerializer<DO.Assignments>(s_assignmentss_xml);
        //return assignmentss.FirstOrDefault(item => filter(item));
        return assignmentss.FirstOrDefault(filter);
    }

    public IEnumerable<Assignments?> ReadAll(Func<Assignments, bool>? filter = null)
    {
        List<DO.Assignments> assignmentss = XMLTools.LoadListFromXMLSerializer<DO.Assignments>(s_assignmentss_xml);

        if (filter != null)
        {
            return from item in assignmentss
                   where filter(item)
                   select item;
        }
        return from item in assignmentss
               select item;
    }

    public void Update(ref Assignments ass)
    {
        //List<DO.Assignments> assignmentss = XMLTools.LoadListFromXMLSerializer<DO.Assignments>(s_assignmentss_xml);
        Delete(ass.IdAssignments);
        Create(ass);
        //XMLTools.SaveListToXMLSerializer<DO.Assignments>(assignmentss, s_assignmentss_xml);

    }
}
