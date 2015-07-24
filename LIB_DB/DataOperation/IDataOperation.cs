using System.Collections;
using System.Data;
using LIB_Common;
namespace LIB_DB
{
    public interface IDataOpertion
    {
        FieldCollection Fields { get; }
        bool Insert();
        bool Update();
        bool Delete();
        Hashtable ToHashTable();
        DataTable ToDataTable();
        bool IsForceUpdate { get; set; }
    }
}
