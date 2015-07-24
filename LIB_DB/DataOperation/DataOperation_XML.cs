using LIB_Common;
namespace LIB_DB
{
    public class DataOperation_XML : DataOperationOracle
    {
        public DataOperation_XML(string tablename, DBService db, bool IsLoadDt, FieldCollection fields) :
            base(tablename, db, IsLoadDt, fields)
        {

        }

        //private bool _foreUpdate = false;
        //private FieldCollection _fields;
        //private DataTable dt_Data = new DataTable();

        //#region IDataOpertion Members
       
        //public Common.FieldCollection Fields
        //{
        //    get { return _fields; }
        //}

        //public bool Insert()
        //{
        //    throw new NotImplementedException();
        //}

        //public bool Update()
        //{
        //    throw new NotImplementedException();
        //}

        //public bool Delete()
        //{
        //    throw new NotImplementedException();
        //}

        //public System.Collections.Hashtable ToHashTable()
        //{
        //    throw new NotImplementedException();
        //}

        //public System.Data.DataTable ToDataTable()
        //{
        //    throw new NotImplementedException();
        //}

        //public bool IsForceUpdate
        //{
        //    get
        //    {
        //        return _foreUpdate;
        //    }
        //    set
        //    {
        //        _foreUpdate = value;
        //    }
        //}

        //#endregion
    }
}
