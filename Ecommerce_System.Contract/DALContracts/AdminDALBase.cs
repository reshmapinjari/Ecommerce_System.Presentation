using System;
using System.Collections.Generic;
using System.IO;
using Console_Ecommerce_System.Entities;
using Newtonsoft.Json;
/// <summary>
/// developed by sravani
/// </summary>
namespace Console_Ecommerce_System.Contracts.DALContracts
{
    /// <summary>
    /// This abstract class acts as a base for AdminDAL class
    /// </summary>
    public abstract class AdminDALBase
    {
        //Collection of Admins
        public static List<Admin> adminList = new List<Admin>()
        {
            new Admin() { AdminID = Guid.NewGuid(), Email = "admin@capgemini.com", AdminName = "Capgemini", Password = "manager", CreationDateTime = DateTime.Now, LastModifiedDateTime = DateTime.Now },
        };
        private static readonly string fileName = "admins.json";

        //Methods
        public abstract Admin GetAdminByAdminEmailDAL(string Email);
        public abstract Admin GetAdminByEmailAndPasswordDAL(string email, string password);
        public abstract bool UpdateAdminDAL(Admin updateAdmin);
        public abstract bool UpdateAdminPasswordDAL(Admin updateAdmin);

        /// <summary>
        /// Writes collection to the file in JSON format.
        /// </summary>
        public static void Serialize()
        {
            string serializedJson = JsonConvert.SerializeObject(adminList);
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
                var adminListFromFile = JsonConvert.DeserializeObject<List<Admin>>(fileContent);
                if (adminListFromFile != null)
                {
                    adminList = adminListFromFile;
                }
            }
        }
        
    }
}


