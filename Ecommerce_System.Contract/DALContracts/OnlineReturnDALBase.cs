using System;
using System.Collections.Generic;
using System.IO;
using Console_Ecommerce_System.Entities;
using Newtonsoft.Json;
using Console_Ecommerce_System.Helpers;

namespace Console_Ecommerce_System.Contracts.DALContracts
{
    /// <summary>
    /// This abstract class acts as a base for OnlineReturnDAL class
    /// </summary>
    public abstract class OnlineReturnDALBase
    {
        //Collection of OnlineReturnList
        protected static List<OnlineReturn> onlineReturnList = new List<OnlineReturn>();
        private static string fileName = "OnlineReturns.json";

        //Methods for CRUD operations
        public abstract bool AddOnlineReturnDAL(OnlineReturn newOnlineReturn);
        public abstract List<OnlineReturn> GetAllOnlineReturnsDAL();
        public abstract OnlineReturn GetOnlineReturnByOnlineReturnIDDAL(Guid searchOnlineReturnID);
        public abstract List<OnlineReturn> GetOnlineReturnByCustomerIDDAL(Guid CustomerID);
        public abstract List<OnlineReturn> GetOnlineReturnsByPurposeDAL(PurposeOfReturn purpose);
        public abstract bool UpdateOnlineReturnDAL(OnlineReturn updateOnlineReturn);
        public abstract bool DeleteOnlineReturnDAL(Guid deleteOnlineReturnID);

        /// <summary>
        /// Writes collection to the file in JSON format.
        /// </summary>
        public static void Serialize()
        {
            string serializedJson = JsonConvert.SerializeObject(onlineReturnList);
            using (StreamWriter streamWriter = new StreamWriter(fileName))
            {
                streamWriter.Write(serializedJson);
                streamWriter.Close();
            }
        }

        /// <summary>
        /// Reads collection from the file in JSON format.
        /// </summary>
        public static void Deserialize()
        {
            if (!File.Exists(fileName))
                File.Create(fileName).Close();

            using (StreamReader streamReader = new StreamReader(fileName))
            {
                string fileContent = streamReader.ReadToEnd();
                streamReader.Close();
                var systemUserListFromFile = JsonConvert.DeserializeObject<List<OnlineReturn>>(fileContent);
                if (systemUserListFromFile != null)
                {
                    onlineReturnList = systemUserListFromFile;
                }
            }
        }

        /// <summary>
        /// Static Constructor.
        /// </summary>
        static OnlineReturnDALBase()
        {
            Deserialize();
        }
    }
}


