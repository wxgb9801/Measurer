using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
namespace LIB_Common
{
    public interface IEnableCollectionProperty
    {
        FieldCollection Fields { get; set; }
    }

    ////ConfigFile Example:
    ////<?xml version="1.0" encoding="utf-8" ?>
    ////<SystemBusDataCfg>
    ////  <BusObjConfigGroup GroupName="WAP">
    ////    <BusObjConfigItem ClassName="Material" PropertySchema="FC_Material"/>
    ////    <BusObjConfigItem ClassName="Location" PropertySchema="FC_Location"/>
    ////  </BusObjConfigGroup>
    ////</SystemBusDataCfg>
    /// <summary>
    /// 
    /// </summary>
    public class SysBusinesObjectConfig
    {
        public Dictionary<string, Dictionary<string, string>> SystemBusObjCfgDic = new Dictionary<string, Dictionary<string, string>>();
        public  void Load(string cfgFilePath)
        {
            var xd = FunBase.ReadXmdDoc(cfgFilePath);
            ReadBussinessObjectConfig(xd, SystemBusObjCfgDic,"");
        }
        private void ReadBussinessObjectConfig(XmlNode pNode, Dictionary<string, Dictionary<string, string>> systemBusObjCfgDic,string GroupName)
        {
            try
            {
                foreach (XmlNode theNode in pNode)
                {
                    switch (theNode.Name)
                    {
                        case "SystemBusDataCfg":

                            ReadBussinessObjectConfig(theNode, systemBusObjCfgDic, GroupName);
                            break;
                        case "BusObjConfigGroup":
                            var groupName = theNode.Attributes["GroupName"].Value;
                            if (!systemBusObjCfgDic.ContainsKey(groupName))
                            {
                                var dir_group = new Dictionary<string, string>();
                                systemBusObjCfgDic.Add(groupName, dir_group);
                 
                            }               
                            GroupName = groupName;
                            ReadBussinessObjectConfig(theNode, systemBusObjCfgDic, GroupName);
                            break;
                        case "BusObjConfigItem":
                            var childDict = systemBusObjCfgDic[GroupName];
                            var className = theNode.Attributes["ClassName"].Value;
                            var fieldFieldColletionName = theNode.Attributes["PropertySchema"].Value;
                            childDict.Add(className, fieldFieldColletionName);
                            break;
                    }
                }
            }
            catch
            {
                //暂不处理，将来些日志
                return;
            }
        }

    }

    ////ConfigFile Example:
    ////<?xml version="1.0" encoding="utf-8" ?>
    ////<FieldCollection Name="FC_Material" >
    ////  <Field Name="DetailID" Source="WAPDB.Tab_Detail.ID" isKey="true" DataType="System.Int32"/>
    ////  <Field Name="DetailCode" Source="WAPDB.Tab_Detail.Code"/>
    ////  <Field Name="DetailName" Source="WAPDB.Tab_Detail.Name"/>
    ////  <Field Name="DetailDesc" Source="WAPDB.Tab_Detail.Desc"/>
    ////  <Field Name="MasterID" Source="WAPDB.Tab_Detail.MasterID=WAPDB.Tab_MaterialMaster.ID"  DataType="System.Int32"/>
    ////  <Field Name="ItemType" Source="WAPDB.Tab_MaterialMaster.ItemType" />
    ////</FieldCollection>
    /// <summary>
    /// 系统业务类属性数据结构管理器 用于类属性结构的注册和获取
    /// </summary>
    public class SysBusPropertySchemaManager
    {
        //private static  object Lockobject;
        public static Dictionary<string, FieldCollection> PropertySchemaDic = new Dictionary<string, FieldCollection>();

        public static bool RegisterBusPropertySchema(FieldCollection fc)
        {
            //lock (Lockobject)
            //{
                if (PropertySchemaDic.ContainsKey(fc.Name))
                {
                    return false;
                }
                else
                {
                    PropertySchemaDic.Add(fc.Name, fc);
                    return true;
                }
            //}
        }
        public static bool RegisterBusPropertySchema(string xmlFilePath)
        {
            var xd = FunBase.ReadXmdDoc(xmlFilePath);
            return RegisterBusPropertySchema(xd);
        }
        public static bool RegisterBusPropertySchema(XmlNode pNode)
        {
            var fcs = new List<FieldCollection>();
            ReadOneBusPropertySchema(pNode, ref fcs);
            return fcs.Select(RegisterBusPropertySchema).All(result => result);
        }
        public static FieldCollection GetBusPropertySchema(string fcName)
        {
            if (PropertySchemaDic.ContainsKey(fcName))
            {
                return PropertySchemaDic[fcName].Clone();
            }
            else
            {
                return null;
            }
        }

        private static void ReadOneBusPropertySchema(XmlNode pNode, ref List<FieldCollection> fcS)
        {
            foreach (XmlNode theNode in pNode.ChildNodes)
            {
                switch (theNode.Name)
                {
                    case "FieldCollection":
                        var fc = new FieldCollection {Name = theNode.Attributes["Name"].Value};
                        fcS.Add(fc);
                        ReadOneBusPropertySchema(theNode, ref fcS);
                        break;
                    case "Field":
                        var theFieldCollect = fcS[fcS.Count - 1];
                        var tmpdic = new Dictionary<string, string>();
                        FunBase.ReadXmlAttributes(theNode, ref tmpdic);
                        var source = "";
                        var name = "";
                        var desc = "";
                        object defaultValue = null;
                        var DataType = typeof(String);
                        var onlyRead = false;
                        var isSafe = false;
                        var iskey = false;
                        var allowModify = true;
                        #region
                        if (tmpdic.ContainsKey("SOURCE"))
                        {
                            source = tmpdic["SOURCE"];
                        }
                        if (tmpdic.ContainsKey("DATATYPE"))
                        {
                            DataType = Type.GetType(tmpdic["DATATYPE"]);
                        }
                        if (tmpdic.ContainsKey("NAME"))
                        {
                            name = tmpdic["NAME"];
                        }
                        if (tmpdic.ContainsKey("DESC"))
                        {
                            desc = tmpdic["DESC"];
                        }
                        if (tmpdic.ContainsKey("ONLYREAD"))
                        {
                            bool.TryParse(tmpdic["ONLYREAD"], out onlyRead);
                        }
                        if (tmpdic.ContainsKey("ISKEY"))
                        {
                            bool.TryParse(tmpdic["ISKEY"], out iskey);
                        }
                        if (tmpdic.ContainsKey("ISSAFE"))
                        {
                            bool.TryParse(tmpdic["ISSAFE"], out isSafe);
                        }
                        if (tmpdic.ContainsKey("DEFAULTVALUE"))
                        {
                            defaultValue = tmpdic["DEFAULTVALUE"];
                            if (defaultValue.ToString() == "NOW()")
                            {
                                if (DataType == typeof(DateTime))
                                {
                                    defaultValue = DateTime.Now;
                                }
                                else
                                {
                                    defaultValue = DateTime.Now.ToString(FunBase.STR_DATETIME_TO_DB_ORC);
                                }
                            }
                        }
                        if (tmpdic.ContainsKey("ALLOWMODIFY"))
                        {
                            bool.TryParse(tmpdic["ALLOWMODIFY"], out allowModify);
                        }
                        #endregion
                        var f = new Field(source, name, DataType, defaultValue, iskey, desc, onlyRead, isSafe)
                        {
                            AllowModify = allowModify
                        };
                        theFieldCollect.Add(f);
                        break;
                    default:
                        ReadOneBusPropertySchema(theNode, ref fcS);
                        break;
                }
            }
        }
    }
    public class SysBusinesObjectManager
    {
        public const string DefaultSysBGroup = "WAP";
        public static SysBusinesObjectConfig BOConfig = new SysBusinesObjectConfig();
        public static string CurrentBGroup = "";
        public static FieldCollection GetBusPropertySchemaByClassID(string classID)
        {
            if (CurrentBGroup == "")
                return GetBusPropertySchemaByClassID(DefaultSysBGroup, classID);
            else
                return GetBusPropertySchemaByClassID(CurrentBGroup, classID);
        }
        public static FieldCollection GetBusPropertySchemaByClassID(string CfgGroupName, string classID)
        {
            if (BOConfig.SystemBusObjCfgDic.ContainsKey(CfgGroupName))
            {
                if (BOConfig.SystemBusObjCfgDic[CfgGroupName].ContainsKey(classID))
                {
                    var fcName = BOConfig.SystemBusObjCfgDic[CfgGroupName][classID];
                    return SysBusPropertySchemaManager.GetBusPropertySchema(fcName);
                }
                return null;
            }
            else
            {
                return null;
            }

           
        }
    }
}
