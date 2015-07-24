//====================================================================
// Name Space:  DematicChina.CommClassLib
// Class Name:  MessageSerializer
// Author:	    feng yiping
// Version:	    1.0
// Date:	    2011-01-08
//
// Change History:
// Version	    Date		    By	Details
// 1.0		    2011-01-08      Create	
//
//====================================================================
// Function:
// MessageSerializer class : This class is  factory , 
// which is convert MessagePacket object to string and convert string to MessagePacket object
//====================================================================
using System;
using System.IO;				 // For reading/writing data to an XML file.
using System.IO.IsolatedStorage; // For accessing user isolated data.
using System.Runtime.Serialization.Formatters.Binary; // For serialization of an object to an XML Binary file.
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Xml.Serialization;	 // For serialization of an object to an XML Document file.

namespace LIB_Common
{
    /// <summary>
    /// Serialization format types.
    /// </summary>
    public enum SerializedFormat
    {
        /// <summary>
        /// Binary serialization format.
        /// </summary>
        Binary=0,
        /// <summary>
        /// Soap serialization format.
        /// </summary>
        Soap =1,
        /// <summary>
        /// XMLDocument serialization format.
        /// </summary>
        XML = 2,
        /// <summary>
        /// Use Each Class self Serialization function
        /// </summary>
        Customer = 3
    }

    public static class MessageSerializer<T> where T : class // Specify that T must be a class.
    {
        #region Public Interface

        public static void Save(T serializableObject, string path, SerializedFormat serializedFormat, ref string errMsg)
        {
            switch (serializedFormat)
            {
                case SerializedFormat.Binary:
                    SaveToBinaryFormat(serializableObject, path, null, ref  errMsg);
                    break;
                case SerializedFormat.Soap:
                    SaveToSoapFormat(serializableObject, path, null, ref  errMsg);
                    break;

                case SerializedFormat.XML:
                default:
                    SaveToXMLFormat(serializableObject, null, path, null, ref  errMsg);
                    break;
            }
        }

        public static T Load(string path, SerializedFormat serializedFormat,ref string errMsg)
        {
            T serializableObject;

            switch (serializedFormat)
            {
                case SerializedFormat.Binary:
                    serializableObject = LoadFromBinaryFormat(path, null, ref errMsg);
                    break;
                case SerializedFormat.Soap:
                    serializableObject = LoadFromSoapFormat(path, null, ref errMsg);
                    break;

                case SerializedFormat.XML:
                default:
                    serializableObject = LoadFromXMLFormat(null, path, null, ref errMsg);
                    break;
            }

            return serializableObject;
        }
        
        public static string SaveToString(T serializableObject, SerializedFormat serializedFormat, ref string errMsg)
        {
            switch (serializedFormat)
            {
                case SerializedFormat.Binary:
                    return SaveToBinaryFormat(serializableObject, null, ref errMsg);
                case SerializedFormat.Soap:
                    return SaveToSoapFormat(serializableObject, null, ref errMsg);
                case  SerializedFormat.Customer:
                    return SaveToCustomerFormat(serializableObject, null, eCustomerSerializableType.NoStruct, ref errMsg);
                case SerializedFormat.XML:
                default:
                    return SaveToXMLFormat(serializableObject, null, null, ref errMsg);
            }
        }

        public static T GetObjectByString(string strdata, SerializedFormat format, ref string errMsg)
        {
            switch (format)
            {
                case SerializedFormat.Binary:
                    return GetObjectFromBinaryString(strdata, ref errMsg);
                case SerializedFormat.Soap:
                    return GetObjectFromSoapString(strdata, ref errMsg);
                case SerializedFormat.Customer:
                    return GetObjectFromCustomerString(strdata, eCustomerSerializableType.NoStruct, ref errMsg);
                case SerializedFormat.XML:

                default:
                    return GetObjectFromXML(strdata, ref errMsg);
            }
        }

        //add by FYP @2013/11/28
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializableObject"></param>
        /// <param name="serializedFormat"></param>
        /// <param name="ecsformat"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static string SaveToString(T serializableObject, SerializedFormat serializedFormat, eCustomerSerializableType ecsformat, ref string errMsg)
        {
            switch (serializedFormat)
            {
                case SerializedFormat.Customer:
                    return SaveToCustomerFormat(serializableObject, null, ecsformat, ref errMsg);
                default:
                    return SaveToXMLFormat(serializableObject, null, null, ref errMsg);
            }
        }

        //add by FYP @2013/11/28
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strdata"></param>
        /// <param name="format"></param>
        /// <param name="ecsformat"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static T GetObjectByString(string strdata, SerializedFormat format, eCustomerSerializableType ecsformat, ref string errMsg)
        {
            switch (format)
            {
                case SerializedFormat.Customer:
                    return GetObjectFromCustomerString(strdata,ecsformat, ref errMsg);
                default:
                    return GetObjectFromXML(strdata, ref errMsg);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads an object from an XML file in XMLDocument format.
        /// </summary>
        /// <param name="path">Path of the file to load the object from.</param>
        /// <param name="errMsg"></param>
        /// <returns>Object loaded from an XML file in Document format.</returns>        
        private static T Load(string path,ref string errMsg)
        {
            var serializableObject = LoadFromXMLFormat(null, path, null, ref errMsg);
            return serializableObject;
        }

        /// <summary>
        /// Loads an object from an XML file in Document format, supplying extra data types to enable deserialization of custom types within the object.
        /// </summary>
        /// <param name="path">Path of the file to load the object from.</param>
        /// <param name="extraTypes">Extra data types to enable deserialization of custom types within the object.</param>
        /// <param name="errMsg"></param>
        /// <returns>Object loaded from an XML file in Document format.</returns>
        private static T Load(string path, Type[] extraTypes, ref string errMsg)
        {
            var serializableObject = LoadFromXMLFormat(extraTypes, path, null, ref errMsg);
            return serializableObject;
        }

        /// <summary>
        /// Loads an object from an XML file in Document format, located in a specified isolated storage area.
        /// </summary>
        /// <param name="fileName">Name of the file in the isolated storage area to load the object from.</param>
        /// <param name="isolatedStorageDirectory">Isolated storage area directory containing the XML file to load the object from.</param>
        /// <param name="errMsg"></param>
        /// <returns>Object loaded from an XML file in Document format located in a specified isolated storage area.</returns>
        private static T Load(string fileName, IsolatedStorageFile isolatedStorageDirectory, ref string errMsg)
        {
            var serializableObject = LoadFromXMLFormat(null, fileName, isolatedStorageDirectory, ref errMsg);
            return serializableObject;
        }

        /// <summary>
        /// Loads an object from an XML file located in a specified isolated storage area, using a specified serialized format.
        /// </summary>		
        /// <param name="fileName">Name of the file in the isolated storage area to load the object from.</param>
        /// <param name="isolatedStorageDirectory">Isolated storage area directory containing the XML file to load the object from.</param>
        /// <param name="serializedFormat">XML serialized format used to load the object.</param>
        /// <param name="errMsg"></param>
        /// <returns>Object loaded from an XML file located in a specified isolated storage area, using a specified serialized format.</returns>
        private static T Load(string fileName, IsolatedStorageFile isolatedStorageDirectory, SerializedFormat serializedFormat, ref string errMsg)
        {
            T serializableObject;

            switch (serializedFormat)
            {
                case SerializedFormat.Binary:
                    serializableObject = LoadFromBinaryFormat(fileName, isolatedStorageDirectory, ref errMsg);
                    break;
                case SerializedFormat.Soap:
                    serializableObject = LoadFromSoapFormat(fileName, isolatedStorageDirectory, ref errMsg);
                    break;
                case SerializedFormat.XML:
                default:
                    serializableObject = LoadFromXMLFormat(null, fileName, isolatedStorageDirectory, ref errMsg);
                    break;
            }

            return serializableObject;
        }

        /// <summary>
        /// Loads an object from an XML file in Document format, located in a specified isolated storage area, and supplying extra data types to enable deserialization of custom types within the object.
        /// </summary>		
        /// <param name="fileName">Name of the file in the isolated storage area to load the object from.</param>
        /// <param name="isolatedStorageDirectory">Isolated storage area directory containing the XML file to load the object from.</param>
        /// <param name="extraTypes">Extra data types to enable deserialization of custom types within the object.</param>
        /// <param name="errMsg"></param>
        /// <returns>Object loaded from an XML file located in a specified isolated storage area, using a specified serialized format.</returns>
        private static T Load(string fileName, IsolatedStorageFile isolatedStorageDirectory, Type[] extraTypes, ref string errMsg)
        {
            var serializableObject = LoadFromXMLFormat(null, fileName, isolatedStorageDirectory, ref errMsg);
            return serializableObject;
        }

        #endregion

        #region Save methods

        /// <summary>
        /// Saves an object to an XML file in Document format.
        /// </summary>
        /// <param name="serializableObject">Serializable object to be saved to file.</param>
        /// <param name="path">Path of the file to save the object to.</param>
        private static void Save(T serializableObject, string path,ref string errMsg)
        {
            SaveToXMLFormat(serializableObject, null, path, null,ref  errMsg);
        }



        /// <summary>
        /// Saves an object to an XML file in Document format, supplying extra data types to enable serialization of custom types within the object.
        /// </summary>
        /// <param name="serializableObject">Serializable object to be saved to file.</param>
        /// <param name="path">Path of the file to save the object to.</param>
        /// <param name="extraTypes">Extra data types to enable serialization of custom types within the object.</param>
        private static void Save(T serializableObject, string path, Type[] extraTypes,ref string errMsg)
        {
            SaveToXMLFormat(serializableObject, extraTypes, path, null,ref  errMsg);
        }

        /// <summary>
        /// Saves an object to an XML file in Document format, located in a specified isolated storage area.
        /// </summary>
        /// <param name="serializableObject">Serializable object to be saved to file.</param>
        /// <param name="fileName">Name of the file in the isolated storage area to save the object to.</param>
        /// <param name="isolatedStorageDirectory">Isolated storage area directory containing the XML file to save the object to.</param>
        private static void Save(T serializableObject, string fileName, IsolatedStorageFile isolatedStorageDirectory,ref string errMsg)
        {
            SaveToXMLFormat(serializableObject, null, fileName, isolatedStorageDirectory,ref  errMsg);
        }

        /// <summary>
        /// Saves an object to an XML file located in a specified isolated storage area, using a specified serialized format.
        /// </summary>
        /// <param name="serializableObject">Serializable object to be saved to file.</param>
        /// <param name="fileName">Name of the file in the isolated storage area to save the object to.</param>
        /// <param name="isolatedStorageDirectory">Isolated storage area directory containing the XML file to save the object to.</param>
        /// <param name="serializedFormat">XML serialized format used to save the object.</param>        
        private static void Save(T serializableObject, string fileName, IsolatedStorageFile isolatedStorageDirectory, SerializedFormat serializedFormat,ref string errMsg)
        {
            switch (serializedFormat)
            {
                case SerializedFormat.Binary:
                    SaveToBinaryFormat(serializableObject, fileName, isolatedStorageDirectory,ref errMsg);
                    break;
                case SerializedFormat.Soap:
                    SaveToSoapFormat(serializableObject, fileName, isolatedStorageDirectory,ref errMsg);
                    break;
                case SerializedFormat.XML:
                default:
                    SaveToXMLFormat(serializableObject, null, fileName, isolatedStorageDirectory,ref errMsg);
                    break;
            }
        }

        /// <summary>
        /// Saves an object to an XML file in Document format, located in a specified isolated storage area, and supplying extra data types to enable serialization of custom types within the object.
        /// </summary>		
        /// <param name="serializableObject">Serializable object to be saved to file.</param>
        /// <param name="fileName">Name of the file in the isolated storage area to save the object to.</param>
        /// <param name="isolatedStorageDirectory">Isolated storage area directory containing the XML file to save the object to.</param>
        /// <param name="extraTypes">Extra data types to enable serialization of custom types within the object.</param>
        private static void Save(T serializableObject, string fileName, IsolatedStorageFile isolatedStorageDirectory, Type[] extraTypes, ref string errMsg)
        {
            SaveToXMLFormat(serializableObject, null, fileName, isolatedStorageDirectory,ref  errMsg);
        }

        #endregion

        #region Private Methods

        private static FileStream CreateFileStream(IsolatedStorageFile isolatedStorageFolder, string path)
        {
            FileStream fileStream;

            if (isolatedStorageFolder == null)
                fileStream = new FileStream(path, FileMode.OpenOrCreate);
            else
                fileStream = new IsolatedStorageFileStream(path, FileMode.OpenOrCreate, isolatedStorageFolder);

            return fileStream;
        }

        private static T LoadFromBinaryFormat(string path, IsolatedStorageFile isolatedStorageFolder ,ref string errMsg)
        {
            errMsg = "";
            try
            {
                T serializableObject = null;

                using (var fileStream = CreateFileStream(isolatedStorageFolder, path))
                {
                    var binaryFormatter = new BinaryFormatter();
                    serializableObject = binaryFormatter.Deserialize(fileStream) as T;
                }

                return serializableObject;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return null;
            }
        }

        private static T LoadFromSoapFormat(string path, IsolatedStorageFile isolatedStorageFolder, ref string errMsg)
        {
            errMsg = "";
            try
            {
                T serializableObject = null;

                using (var fileStream = CreateFileStream(isolatedStorageFolder, path))
                {
                    var binaryFormatter = new SoapFormatter();
                    serializableObject = binaryFormatter.Deserialize(fileStream) as T;
                }

                return serializableObject;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return null;
            }
        }

        private static T LoadFromXMLFormat(Type[] extraTypes, string path, IsolatedStorageFile isolatedStorageFolder,ref string errMsg)
        {
            errMsg = "";
            try
            {
                T serializableObject = null;

                using (var textReader = CreateTextReader(isolatedStorageFolder, path))
                {
                    var xmlSerializer = CreateXmlSerializer(extraTypes);
                    serializableObject = xmlSerializer.Deserialize(textReader) as T;

                }

                return serializableObject;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return null;
            }
        }

        private static TextReader CreateTextReader(IsolatedStorageFile isolatedStorageFolder, string path)
        {
            TextReader textReader = null;

            if (isolatedStorageFolder == null)
                textReader = new StreamReader(path);
            else
                textReader = new StreamReader(new IsolatedStorageFileStream(path, FileMode.Open, isolatedStorageFolder));

            return textReader;
        }

        private static TextWriter CreateTextWriter(IsolatedStorageFile isolatedStorageFolder, string path)
        {
            TextWriter textWriter = null;

            if (isolatedStorageFolder == null)
                textWriter = new StreamWriter(path);
            else
                textWriter = new StreamWriter(new IsolatedStorageFileStream(path, FileMode.OpenOrCreate, isolatedStorageFolder));

            return textWriter;
        }

        private static XmlSerializer CreateXmlSerializer(Type[] extraTypes)
        {
            var ObjectType = typeof(T);

            XmlSerializer xmlSerializer = null;

            if (extraTypes != null)
                xmlSerializer = new XmlSerializer(ObjectType, extraTypes);
            else
                xmlSerializer = new XmlSerializer(ObjectType);

            return xmlSerializer;
        }

        private static void SaveToXMLFormat(T serializableObject, Type[] extraTypes, string path, IsolatedStorageFile isolatedStorageFolder,ref string errMsg)
        {
            errMsg = "";
            try
            {
                using (var textWriter = CreateTextWriter(isolatedStorageFolder, path))
                {
                    var xmlSerializer = CreateXmlSerializer(extraTypes);

                    xmlSerializer.Serialize(textWriter, serializableObject);

                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return;
            }

        }

        private static void SaveToBinaryFormat(T serializableObject, string path, IsolatedStorageFile isolatedStorageFolder, ref string errMsg)
        {
            errMsg = "";
            try
            {
                using (var fileStream = CreateFileStream(isolatedStorageFolder, path))
                {
                    var binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(fileStream, serializableObject);
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return;
            }
        }

        private static void SaveToSoapFormat(T serializableObject, string path, IsolatedStorageFile isolatedStorageFolder, ref string errMsg)
        {
            errMsg = "";
            try
            {
                using (var fileStream = CreateFileStream(isolatedStorageFolder, path))
                {
                    var soapFormatter = new SoapFormatter();
                    soapFormatter.Serialize(fileStream, serializableObject);
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return;
            }
        }

        #endregion

        #region Convert Object To String /Convert String To Object

        /// <summary>
        /// Saves an object to an XML file in Document format.
        /// </summary>
        /// <param name="serializableObject">Serializable object to be saved to file.</param>
        /// <param name="errMsg"></param>
        private static string SaveToString(T serializableObject,ref string errMsg)
        {
            return SaveToXMLFormat(serializableObject, null, null, ref errMsg);
        }

        /// <summary>
        /// Saves an object to an XML file in Document format, supplying extra data types to enable serialization of custom types within the object.
        /// </summary>
        /// <param name="serializableObject">Serializable object to be saved to file.</param>
        /// <param name="path">Path of the file to save the object to.</param>
        /// <param name="extraTypes">Extra data types to enable serialization of custom types within the object.</param>
        private static string SaveToString(T serializableObject, Type[] extraTypes,ref string errMsg)
        {
            return SaveToXMLFormat(serializableObject, extraTypes, null, ref errMsg);
        }

        /// <summary>
        /// Saves an object to an XML file in Document format, located in a specified isolated storage area.
        /// </summary>
        /// <param name="serializableObject">Serializable object to be saved to file.</param>
        /// <param name="fileName">Name of the file in the isolated storage area to save the object to.</param>
        /// <param name="isolatedStorageDirectory">Isolated storage area directory containing the XML file to save the object to.</param>
        private static string SaveToString(T serializableObject, IsolatedStorageFile isolatedStorageDirectory,ref string errMsg)
        {
            return SaveToXMLFormat(serializableObject, null, isolatedStorageDirectory, ref errMsg);
        }

        /// <summary>
        /// Saves an object to an XML file located in a specified isolated storage area, using a specified serialized format.
        /// </summary>
        /// <param name="serializableObject">Serializable object to be saved to file.</param>
        /// <param name="fileName">Name of the file in the isolated storage area to save the object to.</param>
        /// <param name="isolatedStorageDirectory">Isolated storage area directory containing the XML file to save the object to.</param>
        /// <param name="serializedFormat">XML serialized format used to save the object.</param>        
        private static string SaveToString(T serializableObject, IsolatedStorageFile isolatedStorageDirectory, SerializedFormat serializedFormat,ref string errMsg)
        {
            switch (serializedFormat)
            {
                case SerializedFormat.Binary:
                    return SaveToBinaryFormat(serializableObject, isolatedStorageDirectory,ref errMsg);
                case SerializedFormat.Soap:
                    return SaveToSoapFormat(serializableObject, isolatedStorageDirectory, ref errMsg);
                case SerializedFormat.XML:
                default:
                    return SaveToXMLFormat(serializableObject, null, isolatedStorageDirectory,ref  errMsg);
            }
        }

        /// <summary>
        /// Saves an object to an XML file in Document format, located in a specified isolated storage area, and supplying extra data types to enable serialization of custom types within the object.
        /// </summary>		
        /// <param name="serializableObject">Serializable object to be saved to file.</param>
        /// <param name="fileName">Name of the file in the isolated storage area to save the object to.</param>
        /// <param name="isolatedStorageDirectory">Isolated storage area directory containing the XML file to save the object to.</param>
        /// <param name="extraTypes">Extra data types to enable serialization of custom types within the object.</param>
        private static string SaveToXMLString(T serializableObject, IsolatedStorageFile isolatedStorageDirectory, Type[] extraTypes, ref string errMsg)
        {
            return SaveToXMLFormat(serializableObject, null, isolatedStorageDirectory,ref errMsg);
        }


        /// <summary>
        /// Loads an object from an XML file using a specified serialized format.
        /// </summary>	
        /// <param name="path">Path of the file to load the object from.</param>
        /// <returns>Object loaded from an XML file using the specified serialized format.</returns>
        public static T GetObjectFromXML(string strdata, ref string errMsg)
        {
            errMsg = "";
            T serializableObject = null;

            serializableObject = LoadFromXMLFormat(null, strdata,ref errMsg);

            return serializableObject;
        }

        private static T LoadFromXMLFormat(Type[] extraTypes, string strdata, ref string errMsg)
        {
            errMsg = "";
            try
            {
                T serializableObject = null;
                // StringWriter strw = new StringWriter(new StringBuilder(strdata));
                var strr = new StringReader(strdata);
                var xmlSerializer = CreateXmlSerializer(extraTypes);
                serializableObject = xmlSerializer.Deserialize(strr) as T;
                return serializableObject;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return null;
            }
        }


        public static T GetObjectFromSoapString(string strdata, ref string errMsg)
        {
            T serializableObject = null;

            serializableObject = LoadFromSoapFormat(null, strdata,ref errMsg);

            return serializableObject;
        }

        private static T LoadFromSoapFormat(Type[] extraTypes, string strdata, ref string errMsg)
        {
            errMsg = "";
            try
            {
                
                T serializableObject = null;
                var mms = new MemoryStream(Encoding.Unicode.GetBytes(strdata));
                mms.Seek(0, SeekOrigin.Begin);
                var soapserializer = new SoapFormatter();             
                serializableObject = soapserializer.Deserialize(mms) as T;
                return serializableObject;
                
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return null;
            }
        }

        //add by FYP @2013/11/28
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strdata"></param>
        /// <param name="ecsformat"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static T GetObjectFromCustomerString(string strdata,eCustomerSerializableType ecsformat,  ref string errMsg)
        {
            var serializableObject = default(T);
            var t = typeof(T);
            serializableObject = (T)Activator.CreateInstance(t);

            if (serializableObject is ICustomerSerializable)
            {
                ICustomerSerializable csdata = serializableObject as ICustomerSerializable;
                csdata.LoadObjectFromSerializableString(ecsformat, strdata);
            }

            return serializableObject;
        }


        public static T GetObjectFromBinaryString(string strdata, ref string errMsg)
        {
            T serializableObject = null;

            serializableObject = LoadFromBinaryFormat(null, strdata,ref errMsg);

            return serializableObject;
        }

        private static T LoadFromBinaryFormat(Type[] extraTypes, string strdata, ref string errMsg)
        {
            errMsg = "";
            try
            {
                T serializableObject = null;
                //byte[] ss = Encoding.UTF8.GetBytes(strdata);
                //byte[] ss1 = Encoding.Convert(Encoding.UTF8, Encoding.Default, ss);
                //MemoryStream mms = new MemoryStream(ss1);
                //MemoryStream mms = new MemoryStream(Encoding.Unicode.GetBytes(strdata));
                var mms = new MemoryStream(Encoding.Default.GetBytes(strdata));
                mms.Seek(0, SeekOrigin.Begin);
                var binserializer = new BinaryFormatter();
                serializableObject = binserializer.Deserialize(mms) as T;
                return serializableObject;
            }
            catch(Exception ex)
            {
                errMsg = ex.Message;
                return null;
            }
        }
        

        private static string SaveToXMLFormat(T serializableObject, Type[] extraTypes, IsolatedStorageFile isolatedStorageFolder, ref string errMsg)
        {
            errMsg = "";
            try
            {
                var sb = new StringBuilder();
                var strw = new StringWriter(sb);
                var xmlSerializer = CreateXmlSerializer(extraTypes);
                xmlSerializer.Serialize(strw, serializableObject);
                strw.Close();
                return sb.ToString();

                //MemoryStream mstream = new MemoryStream();
                //XmlSerializer xmlSerializer = CreateXmlSerializer(extraTypes);
                //xmlSerializer.Serialize(mstream, serializableObject);
                //return Encoding.UTF8.GetString(mstream.ToArray());
            }
            catch(Exception ex )
            {
                errMsg = ex.Message;
                return "";
            }
        }

        private static string SaveToBinaryFormat(T serializableObject, IsolatedStorageFile isolatedStorageFolder, ref string errMsg)
        {
            errMsg = "";
            try
            {
                var mstream = new MemoryStream();
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(mstream, serializableObject);
                //return Encoding.Unicode.GetString(mstream.GetBuffer());
                mstream.Seek(0, SeekOrigin.Begin);
                //byte[] ss = mstream.GetBuffer();
                //byte[] ss1 = Encoding.Convert(Encoding.Default,Encoding.UTF8,ss );
                //return Encoding.UTF8.GetString(mstream.GetBuffer());
                return Encoding.Default.GetString(mstream.GetBuffer());
                
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return "";
            }
        }

        private static string SaveToSoapFormat(T serializableObject, IsolatedStorageFile isolatedStorageFolder, ref string errMsg)
        {
            errMsg = "";

            try
            {
                var mstream = new MemoryStream();
                var formatter = new SoapFormatter();
                formatter.Serialize(mstream, serializableObject);
                return Encoding.Unicode.GetString(mstream.GetBuffer());
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return "";
            }
        }

        //add by FYP @2013/11/28
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializableObject"></param>
        /// <param name="isolatedStorageFolder"></param>
        /// <param name="csformat"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private static string SaveToCustomerFormat(T serializableObject, IsolatedStorageFile isolatedStorageFolder, eCustomerSerializableType csformat , ref string errMsg)
        {
            errMsg = "";

            try
            {
                if (serializableObject is ICustomerSerializable)
                {
                    ICustomerSerializable csdata = serializableObject as ICustomerSerializable;
                    return csdata.ToSerializableString(csformat);
                }
                else
                {
                    return "";
                }

            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return "";
            }
        }
        #endregion

    }
}
