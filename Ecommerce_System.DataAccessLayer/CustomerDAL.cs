using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console_Ecommerce_System.Contracts.DALContracts;
using Console_Ecommerce_System.Entities;
using Console_Ecommerce_System.Exceptions;
using Console_Ecommerce_System.Helpers;

namespace Console_Ecommerce_System.DataAccessLayer
{
    /// <summary>
    /// Contains data access layer methods for inserting, updating, deleting Customers from Customers collection.
    /// </summary>
    public class CustomerDAL : CustomerDALBase, IDisposable
    {
        /// <summary>
        /// Adds new Customer to Customers collection.
        /// </summary>
        /// <param name="newCustomer">Contains the Customer details to be added.</param>
        /// <returns>Determinates whether the new Customer is added.</returns>
        public override (bool, Guid) AddCustomerDAL(Customer newCustomer)
        {
            bool CustomerAdded = false;
            Guid CustomerGuid;
            try
            {
                CustomerGuid = Guid.NewGuid();
                newCustomer.CustomerID = CustomerGuid;
                newCustomer.CreationDateTime = DateTime.Now;
                newCustomer.LastModifiedDateTime = DateTime.Now;
                CustomerList.Add(newCustomer);
                CustomerAdded = true;
            }
            catch (System.Exception)
            {
                throw;
            }
            return (CustomerAdded, CustomerGuid);
        }

        /// <summary>
        /// Gets all Customers from the collection.
        /// </summary>
        /// <returns>Returns list of all Customers.</returns>
        public override List<Customer> GetAllCustomersDAL()
        {
            return CustomerList;
        }

        /// <summary>
        /// Gets Customer based on CustomerID.
        /// </summary>
        /// <param name="searchCustomerID">Represents CustomerID to search.</param>
        /// <returns>Returns Customer object.</returns>
        public override Customer GetCustomerByCustomerIDDAL(Guid searchCustomerID)
        {
            Customer matchingCustomer = null;
            try
            {
                //Find Customer based on searchCustomerID
                matchingCustomer = CustomerList.Find(
                    (item) => { return item.CustomerID == searchCustomerID; }
                );
            }
            catch (System.Exception)
            {
                throw;
            }
            return matchingCustomer;
        }

        /// <summary>
        /// Gets Customer based on CustomerName.
        /// </summary>
        /// <param name="CustomerName">Represents CustomerName to search.</param>
        /// <returns>Returns Customer object.</returns>
        public override List<Customer> GetCustomersByNameDAL(string CustomerName)
        {
            List<Customer> matchingCustomers = new List<Customer>();
            try
            {
                //Find All Customers based on CustomerName
                matchingCustomers = CustomerList.FindAll(
                    (item) => { return item.CustomerName.Equals(CustomerName, StringComparison.OrdinalIgnoreCase); }
                );
            }
            catch (System.Exception)
            {
                throw;
            }
            return matchingCustomers;
        }

        /// <summary>
        /// Gets Customer based on email.
        /// </summary>
        /// <param name="email">Represents Customer's Email Address.</param>
        /// <returns>Returns Customer object.</returns>
        public override Customer GetCustomerByEmailDAL(string email)
        {
            Customer matchingCustomer = null;
            try
            {
                //Find Customer based on Email and Password
                matchingCustomer = CustomerList.Find(
                    (item) => { return item.Email.Equals(email); }
                );
            }
            catch (System.Exception)
            {
                throw;
            }
            return matchingCustomer;
        }

        /// <summary>
        /// Gets Customer based on Email and Password.
        /// </summary>
        /// <param name="email">Represents Customer's Email Address.</param>
        /// <param name="password">Represents Customer's Password.</param>
        /// <returns>Returns Customer object.</returns>
        public override Customer GetCustomerByEmailAndPasswordDAL(string email, string password)
        {
            Customer matchingCustomer = null;
            try
            {
                //Find Customer based on Email and Password
                matchingCustomer = CustomerList.Find(
                    (item) => { return item.Email.Equals(email) && item.Password.Equals(password); }
                );
            }
            catch (System.Exception)
            {
                throw;
            }
            return matchingCustomer;
        }

        /// <summary>
        /// Updates Customer based on CustomerID.
        /// </summary>
        /// <param name="updateCustomer">Represents Customer details including CustomerID, CustomerName etc.</param>
        /// <returns>Determinates whether the existing Customer is updated.</returns>
        public override bool UpdateCustomerDAL(Customer updateCustomer)
        {
            bool CustomerUpdated = false;
            try
            {
                //Find Customer based on CustomerID
                Customer matchingCustomer = GetCustomerByCustomerIDDAL(updateCustomer.CustomerID);

                if (matchingCustomer != null)
                {
                    //Update Customer details
                    ReflectionHelpers.CopyProperties(updateCustomer, matchingCustomer, new List<string>() { "CustomerName", "Email" });
                    matchingCustomer.LastModifiedDateTime = DateTime.Now;

                    CustomerUpdated = true;
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            return CustomerUpdated;
        }

        /// <summary>
        /// Deletes Customer based on CustomerID.
        /// </summary>
        /// <param name="deleteCustomerID">Represents CustomerID to delete.</param>
        /// <returns>Determinates whether the existing Customer is updated.</returns>
        public override bool DeleteCustomerDAL(Guid deleteCustomerID)
        {
            bool CustomerDeleted = false;
            try
            {
                //Find Customer based on searchCustomerID
                Customer matchingCustomer = CustomerList.Find(
                    (item) => { return item.CustomerID == deleteCustomerID; }
                );

                if (matchingCustomer != null)
                {
                    //Delete Customer from the collection
                    CustomerList.Remove(matchingCustomer);
                    CustomerDeleted = true;
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            return CustomerDeleted;
        }

        /// <summary>
        /// Updates Customer's password based on CustomerID.
        /// </summary>
        /// <param name="updateCustomer">Represents Customer details including CustomerID, Password.</param>
        /// <returns>Determinates whether the existing Customer's password is updated.</returns>
        public override bool UpdateCustomerPasswordDAL(Customer updateCustomer)
        {
            bool passwordUpdated = false;
            try
            {
                //Find Customer based on CustomerID
                Customer matchingCustomer = GetCustomerByCustomerIDDAL(updateCustomer.CustomerID);

                if (matchingCustomer != null)
                {
                    //Update Customer details
                    ReflectionHelpers.CopyProperties(updateCustomer, matchingCustomer, new List<string>() { "Password" });
                    matchingCustomer.LastModifiedDateTime = DateTime.Now;

                    passwordUpdated = true;
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            return passwordUpdated;
        }

        /// <summary>
        /// Clears unmanaged resources such as db connections or file streams.
        /// </summary>
        public void Dispose()
        {
            //No unmanaged resources currently
        }
    }
}



