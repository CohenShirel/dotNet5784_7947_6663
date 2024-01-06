﻿using DalApi;
using DO;
namespace DalTest;

public static class Initialization
{
    private static IWorker? s_dalWorker;
    private static IAssignments? s_dalAssignments; //stage 1
    private static ILink? s_dalLink; //stage 1


    //בנוסף, נצטרך שדה אחד, שכל הישויות יעשו בו שימוש, ליצירת מספרים רנדומליים בזמן מילוי ערכי האובייקטים. 

    private static readonly Random s_rand = new();


    private static void creatWorkers ()
    {
        string[] WorkersNames =
        {
           "Dani Levi", "Eli Amar", "Yair Cohen",
           "Ariela Levin", "Dina Klein", "Shira Israelof"
        };
        string[] WorkersEmail =
       {
           "Dani@gmail.com", "Eli@gmail.com", "Yair@gmail.com",
           "Ariela@gmail.com", "Dina@gmail.com", "Shira@gmail.com"
        };
     
        foreach (var _name in WorkersNames)
        {
            int index = 0;
            int _idW;
            do
                _idW = s_rand.Next(200000000, 400000001);
            while (s_dalWorker!.Read(_idW) != null);

            int _hourSalary = s_rand.Next(50, 600);
            int _experience= s_rand.Next(0,60);

            Worker newWork = new(_idW, _name, WorkersEmail[index], _hourSalary, _experience);

            s_dalWorker!.Create(newWork);
            index++;
        }
    }
    private static void creatAssignmentss()
    {
        int _idA;
        do
            _idA = s_rand.Next(200000000, 400000001);
        while (s_dalAssignments!.Read(_idA) != null);

        int _durationA = s_rand.Next(50, 600);
        int _levelA = s_rand.Next(50, 600);

        int _idW;
        do
            _idW = s_rand.Next(200000000, 400000001);
        while (s_dalWorker!.Read(_idW) == null);//if you find it!

        TimeSpan? _dateBegin = null;
        TimeSpan? _deadLine = null;
        TimeSpan? _dateStart = null;
        TimeSpan? _dateFinish = null;
        string? _name = null;
        string? _description = null;
        string? _remarks = null;
        string? _resultProduct = null;
        bool _milestone = false;

        Assignments newA = new(_idA, _durationA, _idW, _dateBegin, _deadLine, _dateStart,
            _dateFinish, _name, _description, _remarks, _resultProduct, _milestone);

        s_dalAssignments!.Create(newA);
    }
    private static void creatLnk()
    {
        int _idL;
        do
            _idL = s_rand.Next(200000000, 400000001);
        while (s_dalLink!.Read(_idL) != null);

        int _idA;
        do
            _idA = s_rand.Next(200000000, 400000001);
        while (s_dalAssignments!.Read(_idA) == null);

        int _idPA;
        do
            _idPA = s_rand.Next(200000000, 400000001);
        while (s_dalAssignments!.Read(_idPA) == null);

        Link newL = new(_idL, _idA, _idPA);
        s_dalLink!.Create(newL);
    }
}
