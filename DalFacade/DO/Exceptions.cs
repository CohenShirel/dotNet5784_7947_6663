﻿

namespace DO;

[Serializable]//האובייקט לא קיים
public class DalDoesNotExistException : Exception
{
    public DalDoesNotExistException(string? message) : base(message)
    {
       
    }
}

[Serializable]//האובייקט כבר קיים
public class DalAlreadyExistsException : Exception
{
    public DalAlreadyExistsException(string? message) : base(message)
    {
        
    }
}

[Serializable]//המחיקה בלתי אפשרית
public class DalDeletionImpossible : Exception
{
    public DalDeletionImpossible(string? message) : base(message)
    {
       
    }
}
//[Serializable]//המחיקה בלתי אפשרית
//public class FormatException : Exception
//{
//    public FormatException(string? message) : base(message)
//    {

//    }
//}

