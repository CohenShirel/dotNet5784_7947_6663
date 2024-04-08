namespace Dal;
internal static class Config
{
    static string s_data_config_xml = "data-config";
    internal static int IdPAssignments { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "IdPAssignments"); }
    internal static int IDLink { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "IDLink"); }
    internal static DateTime? StartProject = DateTime.Now;
}





