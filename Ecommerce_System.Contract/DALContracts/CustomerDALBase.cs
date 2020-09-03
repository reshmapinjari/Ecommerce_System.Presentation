using System;
using System.Collections.Generic;
using System.IO;
using Console_Ecommerce_System.Entities;
using Newtonsoft.Json;

namespace Console_Ecommerce_System.Contracts.DALContracts
{
    /// <summary>
    /// This abstract class acts as a base for CustomerDAL class
    /// </summary>
    public abstract class CustomerDALBase
    {
        //Collection of Customers
        protected static List<Customer> CustomerList = new List<Customer>() { new Customer() { CustomerID = Guid.NewGuid(), Email = "prafull@capgemini.com", CustomerName = "prafull", Password = "prafull", CreationDateTime = DateTime.Now, LastModifiedDateTime = DateTime.Now } };
        private static string fileName = "Customers.json";

        //Methods for CRUD operations
        public abstract (bool,Guid) AddCustomerDAL(Customer newCustomer);
        public abstract List<Customer> GetAllCustomersDAL();
        public abstract Customer GetCustomerByCustomerIDDAL(Guid searchCustomerID);
        public abstract List<Customer> GetCustomersByNameDAL(string CustomerName);
        public abstract Customer GetCustomerByEmailDAL(string email);
        public abstract Customer GetCustomerByEmailAndPasswordDAL(string email, string password);
        public abstract bool UpdateCustomerDAL(Customer updateCustomer);
        public abstract bool UpdateCustomerPasswordDAL(Customer updateCustomer);
        public abstract bool DeleteCustomerDAL(Guid deleteCustomerID);

        /// <summary>
        /// Writes collection to the file in JSON format.
        /// </summary>
        public static void Serialize()
        {
            string serializedJson = JsonConvert.SerializeObject(CustomerList);
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
                var CustomerListFromFile = JsonConvert.DeserializeObject<List<Customer>>(fileContent);
                if (CustomerListFromFile != null)
                {
                    CustomerList = CustomerListFromFile;
                }
            }
        }

        /// <summary>
        /// Static Constructor.
        /// </summary>
        static CustomerDALBase()
        {
            Deserialize();
        }
    }
}


