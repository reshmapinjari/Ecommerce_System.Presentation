using System;
using System.Collections.Generic;
using System.IO;
using Console_Ecommerce_System.Entities;
using Newtonsoft.Json;

namespace Console_Ecommerce_System.Contracts.DALContracts
{
    /// <summary>
    /// This abstract class acts as a base for AddressDAL class
    /// </summary>
    public abstract class AddressDALBase
    {
        //Collection of Addresss
        protected static List<Address> addressList = new List<Address>();
        private static string fileName = "addresss.json";

        //Methods for CRUD operations
        public abstract bool AddAddressDAL(Address newAddress);
        public abstract List<Address> GetAllAddresssDAL();
        public abstract Address GetAddressByAddressIDDAL(Guid searchAddressID);
        public abstract List<Address> GetAddressByCustomerIDDAL(Guid CustomerID);
        public abstract bool UpdateAddressDAL(Address updateAddress);
        public abstract bool DeleteAddressDAL(Guid deleteAddressID);

        /// <summary>
        /// Writes collection to the file in JSON format.
        /// </summary>
        public static void Serialize()
        {
            string serializedJson = JsonConvert.SerializeObject(addressList);
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
            string fileContent = string.Empty;
            if (!File.Exists(fileName))
                File.Create(fileName).Close();

            using (StreamReader streamReader = new StreamReader(fileName))
            {
                fileContent = streamReader.ReadToEnd();
                streamReader.Close();
                var addressListFromFile = JsonConvert.DeserializeObject<List<Address>>(fileContent);
                if (addressListFromFile != null)
                {
                    addressList = addressListFromFile;
                }
            }
        }

        /// <summary>
        /// Static Constructor.
        /// </summary>
        static AddressDALBase()
        {
            Deserialize();
        }
    }
}


