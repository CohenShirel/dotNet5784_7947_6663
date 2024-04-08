using DalApi;
using DO;
using System.Reflection.Metadata;

namespace Dal;

internal class AssignmentsImplementation : IAssignments
{
    readonly string s_assignmentss_xml = "assignments";
    public int Create(Assignment ass)
    {
        List<DO.Assignment> assignments = XMLTools.LoadListFromXMLSerializer<DO.Assignment>(s_assignmentss_xml);
        Assignment newAssignments = ass with { IdAssignment = Config.IdPAssignments };
        assignments.Add(newAssignments);
        XMLTools.SaveListToXMLSerializer<DO.Assignment>(assignments, s_assignmentss_xml);
        return newAssignments.IdAssignment;
    }

    public void Delete(int id)
    {
        List<DO.Assignment> assignmentss = XMLTools.LoadListFromXMLSerializer<DO.Assignment>(s_assignmentss_xml);
        if (assignmentss.RemoveAll(item => item.IdAssignment == id) == 0)
        {
            throw new DalDoesNotExistException($"Assignment with id {id} does not exist");
        }
        XMLTools.SaveListToXMLSerializer<DO.Assignment>(assignmentss, s_assignmentss_xml);
    }

    public void DeleteAll()
    {
        List<DO.Assignment> assignmentss = XMLTools.LoadListFromXMLSerializer<DO.Assignment>(s_assignmentss_xml);
        assignmentss.RemoveAll(item => true);
        XMLTools.SaveListToXMLSerializer<DO.Assignment>(assignmentss, s_assignmentss_xml);
    }
    public Assignment? Read(Func<Assignment, bool> filter)
    {
        List<DO.Assignment> assignmentss = XMLTools.LoadListFromXMLSerializer<DO.Assignment>(s_assignmentss_xml);
        return assignmentss.FirstOrDefault(filter);
    }
    public IEnumerable<Assignment> ReadAll(Func<Assignment, bool>? filter = null)
    {
        List<DO.Assignment> assignmentss = XMLTools.LoadListFromXMLSerializer<DO.Assignment>(s_assignmentss_xml);
        if (filter != null)
        {
            return from item in assignmentss
                   where filter(item)
                   select item;
        }
        return from item in assignmentss
               select item;
    }

    public void Update(Assignment ass)
    {
        List<DO.Assignment> assignmentss1 = XMLTools.LoadListFromXMLSerializer<DO.Assignment>(s_assignmentss_xml);
        if (assignmentss1.RemoveAll(item => item.IdAssignment == ass.IdAssignment) == 0)
        {
            throw new DalDoesNotExistException($"Assignment with id {ass.IdAssignment} does not exist");
        }
        XMLTools.SaveListToXMLSerializer<DO.Assignment>(assignmentss1, s_assignmentss_xml);
        List<DO.Assignment> assignmentss = XMLTools.LoadListFromXMLSerializer<DO.Assignment>(s_assignmentss_xml);
        assignmentss.Add(ass);
        XMLTools.SaveListToXMLSerializer<DO.Assignment>(assignmentss, s_assignmentss_xml);
    }
}
