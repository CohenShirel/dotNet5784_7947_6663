namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Xml;
using System.Xml.Linq;


internal class WorkerImplementation : IWorker
{
    readonly string s_workers_xml = "workers";
    
    //fet element and return worker
    static Worker getWorker(XElement wrk)
    {
        return new Worker()
        {
            IdWorker = int.TryParse((string?)wrk.Element("IdWorker"), out var idWorker) ? idWorker : throw new FormatException("can't convert id"),
            Name = (string?)wrk.Element("Name") ?? "",
            Email = (string?)wrk.Element("Email") ?? "",
            HourSalary = int.TryParse((string?)wrk.Element("HourSalary"), out var hourSalary) ? hourSalary : 0,
            Experience = Enum.TryParse<DO.Level>((string?)wrk.Element("Experience"), out var experience) ? experience :0

        };
    }
    public int Create(Worker item)
    {
        XElement? wrkRoot = XMLTools.LoadListFromXMLElement(s_workers_xml);
        XElement? wrkElem = wrkRoot.Elements().FirstOrDefault(wrk => (int?)wrk.Element("IdWorker") == item.IdWorker);
        if(wrkElem != null)
            throw new DalAlreadyExistsException($"Worker with ID={item.IdWorker} already exists");
        //אולי יהיה צריך לעשות את זה דרך גטוורקר 
        wrkRoot.Add(item);
        XMLTools.SaveListToXMLElement(wrkRoot, s_workers_xml);//save the changes in the file
        return item.IdWorker;
    }
    public void Delete(int id)
    {
        XElement? wrkRoot = XMLTools.LoadListFromXMLElement(s_workers_xml);
        XElement? elemToDelete = wrkRoot.Elements().FirstOrDefault(wrk => (int?)wrk.Element("idWorker") == id);
        if (elemToDelete == null)
            throw new DalDoesNotExistException($"Worker with ID={id} not exists");
        elemToDelete!.Remove();//delete
        XMLTools.SaveListToXMLElement(wrkRoot, s_workers_xml);//save the changes in the file
    }

    public Worker? Read(Func<Worker, bool> filter)
    {
        XElement? wrkRoot = XMLTools.LoadListFromXMLElement(s_workers_xml);
        Worker? wrk = wrkRoot.Elements().Select(elemwrk => getWorker(elemwrk)).FirstOrDefault(filter);

        if (wrk == null)
            throw new DalDoesNotExistException($"Worker does not exist");
        return wrk;

        //XElement? wrkRoot = XMLTools.LoadListFromXMLElement(s_workers_xml);
        //XElement? wrkElem = wrkRoot.Elements().FirstOrDefault(wrk => (int?)wrk.Element("IdWorker") == id);
        //if (wrkElem == null)
        //    throw new DalDoesNotExistException($"Worker with ID={Worker.id} not exists");
        //return getWorker(wrkElem);
    }

    public IEnumerable<Worker?> ReadAll(Func<Worker, bool>? filter = null)
    {
        XElement? wrkRoot = XMLTools.LoadListFromXMLElement(s_workers_xml);
        List<DO.Worker> lstwrk=wrkRoot.Elements().Select(elemwrk=>getWorker(elemwrk)).ToList();
        if (filter != null)
        {
            return from item in lstwrk
                   where filter(item)
                   select item;
        }
        return from item in lstwrk
               select item;
    }

    public void Update(ref Worker item)
    {
        Delete(item.IdWorker);
        Create(item);
    }
}
