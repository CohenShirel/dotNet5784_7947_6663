namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class LinkImplementation : ILink
{
    public int Create(Link link)
    {
        Link newLink = link with { IdLink = DataSource.Config.idlink };
        DataSource.Links.Add(newLink);
        return newLink.IdLink;
    }
    public void Delete(int id)
    {
        if (Read(a => a.IdLink == id) is null)
            throw new DalDoesNotExistException($"Link with ID={id} not exists");
        Link link = Read(a => a.IdLink == id)!;
        DataSource.Links.Remove(link);
    }
    public void DeleteAll()
    {
        Link link = Read(a => true)!;
        DataSource.Links.Remove(link);
    }
    public Link? Read(Func<Link, bool> filter)
    {
        return DataSource.Links.FirstOrDefault(filter);
    }

    public IEnumerable<Link> ReadAll(Func<Link, bool>? filter = null) //stage 2
    {
        if (filter != null)
        {
            return from item in DataSource.Links
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Links
               select item;
    }
    public void Update(Link item)
    {
        Delete(item.IdLink);
        Create(item);
    }
}
