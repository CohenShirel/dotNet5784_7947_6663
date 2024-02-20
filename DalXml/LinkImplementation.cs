using DalApi;
using DO;

namespace Dal;

internal class LinkImplementation : ILink
{
    readonly string s_Links_xml = "links";
    public int Create(Link lnk)
    {
        List<DO.Link> links = XMLTools.LoadListFromXMLSerializer<DO.Link>(s_Links_xml);
        Link newLink = lnk with { IdLink = Config.IDLink };
        links.Add(newLink);
        XMLTools.SaveListToXMLSerializer<DO.Link>(links, s_Links_xml);
        return newLink.IdLink;
    }

    public void Delete(int id)
    {
        List<DO.Link> links = XMLTools.LoadListFromXMLSerializer<DO.Link>(s_Links_xml);
        if (links.RemoveAll(item => item.IdLink == id) == 0)
        {
            throw new DalDoesNotExistException($"Link with id {id} does not exist");
        }
        XMLTools.SaveListToXMLSerializer<DO.Link>(links, s_Links_xml);
    }
    public void DeleteAll()
    {
        List<DO.Link> links = XMLTools.LoadListFromXMLSerializer<DO.Link>(s_Links_xml);
        links.RemoveAll(link => true);
        XMLTools.SaveListToXMLSerializer<DO.Link>(links, s_Links_xml);
    }
    public Link? Read(Func<Link, bool> filter)
    {
        List<DO.Link> links = XMLTools.LoadListFromXMLSerializer<DO.Link>(s_Links_xml);
        //return assignmentss.FirstOrDefault(item => filter(item));
        return links.FirstOrDefault(filter);
    }

    public IEnumerable<Link> ReadAll(Func<Link, bool>? filter)
    {
        List<DO.Link> links = XMLTools.LoadListFromXMLSerializer<DO.Link>(s_Links_xml);
        if (filter != null)
        {
            return from item in links
                   where filter(item)
                   select item;
        }
        return from item in links
               select item;
    }
    public void Update(Link item)
    {
        Delete(item.IdLink);
        Create(item);
    }
}