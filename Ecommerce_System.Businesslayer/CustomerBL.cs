using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Console_Ecommerce_System.Contracts;
using Console_Ecommerce_System.Contracts.DALContracts;
using Console_Ecommerce_System.DataAccessLayer;
using Console_Ecommerce_System.Entities;
using Console_Ecommerce_System.Exceptions;

namespace Console_Ecommerce_System.BusinessLayer
{
    /// <summary>
    /// Contains data access layer methods for inserting, updating, deleting Customers from Customers collection.
    /// </summary>
    public class CustomerBL : BLBase<Customer>, ICustomerBL, IDisposable
    {
        //fields
        CustomerDALBase CustomerDAL;

        /// <summary>
        /// Constructor.
        /// </summary>
        public CustomerBL()
        {
            this.CustomerDAL = new CustomerDAL();
        }

        /// <summary>
        /// Validations on data before adding or updating.
        /// </summary>
        /// <param name="entityObject">Represents object to be validated.</param>
        /// <returns>Returns a boolean value, that indicates whether the data is valid or not.</returns>
        protected async override Task<bool> Validate(Customer entityObject)
        {
            //Create string builder
            StringBuilder sb = new StringBuilder();
            bool valid = await base.Validate(entityObject);

            //Email is Unique
            var existingObject = await GetCustomerByEmailBL(entityObject.Email);
            if (existingObject != null && existingObject?.CustomerID != entityObject.CustomerID)
            {
                valid = false;
                sb.Append(Environment.NewLine + $"Email {entityObject.Email} already exists");
            }

            if (valid == false)
                throw new System.Exception(sb.ToString());
            return valid;
        }

        /// <summary>
        /// Adds new Customer to Customers collection.
        /// </summary>
        /// <param name="newCustomer">Contains the Customer details to be added.</param>
        /// <returns>Determinates whether the new Customer is added.</returns>
        public async Task<(bool, Guid)> AddCustomerBL(Customer newCustomer)
        {
            bool CustomerAdded = false;
            Guid CustomerGuid = default(Guid);

            try
            {
                if (await Validate(newCustomer))
                {
                    await Task.Run(() =>
                    {
                        (CustomerAdded, CustomerGuid) = this.CustomerDAL.AddCustomerDAL(newCustomer);
                        CustomerAdded = true;
                        //Serialize();
                    });
                }
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
        public async Task<List<Customer>> GetAllCustomersBL()
        {
            List<Customer> CustomersList = null;
            try
            {
                await Task.Run(() =>
                {
                    CustomersList = CustomerDAL.GetAllCustomersDAL();
                });
            }
            catch (System.Exception)
            {
                throw;
            }
            return CustomersList;
        }

        /// <summary>
        /// Gets Customer based on CustomerID.
        /// </summary>
        /// <param name="searchCustomerID">Represents CustomerID to search.</param>
        /// <returns>Returns Customer object.</returns>
        public async Task<Customer> GetCustomerByCustomerIDBL(Guid searchCustomerID)
        {
            Customer matchingCustomer = null;
            try
            {
                await Task.Run(() =>
                {
                    matchingCustomer = CustomerDAL.GetCustomerByCustomerIDDAL(searchCustomerID);
                });
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
        public async Task<List<Customer>> GetCustomersByNameBL(string CustomerName)
        {
            List<Customer> matchingCustomers = new List<Customer>();
            try
            {
                await Task.Run(() =>
                {
                    matchingCustomers = CustomerDAL.GetCustomersByNameDAL(CustomerName);
                });
            }
            catch (System.Exception)
            {
                throw;
            }
            return matchingCustomers;
        }

        /// <summary>
        /// Gets Customer based on Email and Password.
        /// </summary>
        /// <param name="email">Represents Customer's Email Customer.</param>
        /// <returns>Returns Customer object.</returns>
        public async Task<Customer> GetCustomerByEmailBL(string email)
        {
            Customer matchingCustomer = null;
            try
            {
                await Task.Run(() =>
                {
                    matchingCustomer = CustomerDAL.GetCustomerByEmailDAL(email);
                });
            }
            catch (System.Exception)
            {
                throw;
            }
            return matchingCustomer;
        }

        /// <summary>
        /// Gets Customer based on Password.
        /// </summary>
        /// <param name="email">Represents Customer's Email Customer.</param>
        /// <param name="password">Represents Customer's Password.</param>
        /// <returns>Returns Customer object.</returns>
        public async Task<Customer> GetCustomerByEmailAndPasswordBL(string email, string password)
        {
            Customer matchingCustomer = null;
            try
            {
                await Task.Run(() =>
                {
                    matchingCustomer = CustomerDAL.GetCustomerByEmailAndPasswordDAL(email, password);
                });
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
        public async Task<bool> UpdateCustomerBL(Customer updateCustomer)
        {
            bool CustomerUpdated = false;
            try
            {
                if ((await Validate(updateCustomer)) && (await GetCustomerByCustomerIDBL(updateCustomer.CustomerID)) != null)
                {
                    this.CustomerDAL.UpdateCustomerDAL(updateCustomer);
                    CustomerUpdated = true;
                    //Serialize();
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
        public async Task<bool> DeleteCustomerBL(Guid deleteCustomerID)
        {
            bool CustomerDeleted = false;
            try
            {
                await Task.Run(() =>
                {
                    CustomerDeleted = CustomerDAL.DeleteCustomerDAL(deleteCustomerID);
                    Serialize();
                });
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
        public async Task<bool> UpdateCustomerPasswordBL(Customer updateCustomer)
        {
            bool passwordUpdated = false;
            try
            {
                if ((await Validate(updateCustomer)) && (await GetCustomerByCustomerIDBL(updateCustomer.CustomerID)) != null)
                {
                    this.CustomerDAL.UpdateCustomerPasswordDAL(updateCustomer);
                    passwordUpdated = true;
                   // Serialize();
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            return passwordUpdated;
        }

        public async Task<CustomerReport> GetCustomerReportByRetailIDBL(Guid CustomerID)
        {
           CustomerReport CustomerReport = new CustomerReport();
            CustomerReport.CustomerID = CustomerID;
            Customer Customer = new Customer();
            CustomerBL CustomerBL = new CustomerBL();
            Customer = await CustomerBL.GetCustomerByCustomerIDBL(CustomerReport.CustomerID);
            CustomerReport.CustomerName = Customer.CustomerName;
            List<Order> orderList = new List<Order>();
            OrderBL order = new OrderBL();
            orderList = await order.GetOrdersByCustomerIDBL(CustomerID);
            foreach (Order item in orderList)
            {
                CustomerReport.CustomerSalesCount++;
                CustomerReport.CustomerSalesAmount += item.OrderAmount;
            }

           return CustomerReport;

        }


        /// <summary>
        /// Disposes DAL object(s).
        /// </summary>
        public void Dispose()
        {
            ((CustomerDAL)CustomerDAL).Dispose();
        }

        /// <summary>
        /// Invokes Serialize method of DAL.
        /// </summary>
        public void Serialize()
        {
            try
            {
                CustomerDALBase.Serialize();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///Invokes Deserialize method of DAL.
        /// </summary>
        public void Deserialize()
        {
            try
            {
                CustomerDALBase.Deserialize();
            }
            catch(System.Exception)
            {
                throw;
            }
        }
    }
}



