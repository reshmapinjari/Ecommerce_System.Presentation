using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Console;
using Console_Ecommerce_System.BusinessLayer;
using Console_Ecommerce_System.Entities;
using Console_Ecommerce_System.Exceptions;
using Console_Ecommerce_System.Contracts;

namespace Console_Ecommerce_System.Presentation
{
    public static class AdminPresentation
    {
        /// <summary>
        /// Menu for Admin User
        /// </summary>
        /// <returns></returns>
        public static async Task<int> AdminUserMenu()
        {
            int choice = -2;
            using (IAdminBL AdminBL = new AdminBL())
            {
                do
                {//Menu

                    WriteLine("Welcome Admin " + ((Admin)CommonData.CurrentUser).AdminName + "!");
                    WriteLine("");

                    WriteLine("------------Customer Related Options------------");
                    WriteLine("");
                    WriteLine("1. View Customer Profile");
                    WriteLine("2. View Customer Reports");

                    WriteLine("");

                   
                    WriteLine("-------------------Edit Personal Profile---------------");
                    WriteLine("");
                    WriteLine("3.Update Admin Profile ");
                    WriteLine("4. Change Password");
                    WriteLine("5. Logout");
                    WriteLine("");
                    WriteLine("..................Product Related Options..........");
                    WriteLine("");
                    WriteLine("6. Add New Product");
                    WriteLine("-1. Exit");
                    WriteLine("");
                    Write("Choice: ");

                    //Accept and check choice
                    bool isValidChoice = int.TryParse(ReadLine(), out choice);
                    if (isValidChoice)
                    {
                        switch (choice)
                        {
                            case 1: await ViewCustomersProfile(); break;
                            case 2: await ViewCustomerReports(); break;
                            case 3: await UpdateAdmin(); break;
                            case 4: await ChangeAdminPassword(); break;
                            case 5: break;
                            case 6: await AddProduct(); break;
                            case -1: break;
                            default: WriteLine("Invalid Choice"); break;
                        }
                    }
                    else
                    {
                        choice = -2;
                    }
                } while (choice != 0 && choice != -1);
            }
            return choice;
        }

        
    

       
        /// <summary>
        /// Updates Admin's Password.
        /// </summary>
        /// <returns></returns>
        public static async Task ChangeAdminPassword()
        {
            try
            {
                using (IAdminBL adminBL = new AdminBL())
                {
                    //Read Current Password
                    Write("Current Password: ");
                    string currentPassword = ReadLine();

                    Admin existingAdmin = await adminBL.GetAdminByEmailAndPasswordBL(CommonData.CurrentUser.Email, currentPassword);

                    if (existingAdmin != null)
                    {
                        //Read inputs
                        Write("New Password: ");
                        string newPassword = ReadLine();
                        Write("Confirm Password: ");
                        string confirmPassword = ReadLine();

                        if (newPassword.Equals(confirmPassword))
                        {
                            existingAdmin.Password = newPassword;

                            //Invoke UpdateSystemUserBL method to update
                            bool isUpdated = await adminBL.UpdateAdminPasswordBL(existingAdmin);
                            if (isUpdated)
                            {
                                WriteLine("Admin Password Updated");
                            }
                        }
                        else
                        {
                            WriteLine($"New Password and Confirm Password doesn't match");
                        }
                    }
                    else
                    {
                        WriteLine($"Current Password doesn't match.");
                    }
                }
            }
            catch (System.Exception ex)
            {
                ExceptionLogger.LogException(ex);
                WriteLine(ex.Message);
            }
        }



        /// <summary>
        /// updates admin profile (i.e, name, email,mobile number).
        /// </summary>
        /// <returns></returns>
        public static async Task UpdateAdmin()
        {
            try
            {
                using (IAdminBL adminBL = new AdminBL())
                {
                    //Read Sl.No
                    Admin admin = await adminBL.GetAdminByAdminEmailBL(CommonData.CurrentUser.Email);
                    Write("Name: ");
                    admin.AdminName = ReadLine();
                    Write("Email: ");
                    admin.Email = ReadLine();

                    //Invoke UpdateCustomerBL method to update
                    bool isUpdated = await adminBL.UpdateAdminBL(admin);
                    if (isUpdated)
                    {
                        WriteLine(" Account Updated");
                    }

                }

            }
            catch (Exceptions.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// views all sales persons report
        /// </summary>
        /// <returns></returns>
     

       
        /// <summary>
        /// views Customer reports.
        /// </summary>
        /// <returns></returns>
        public static async Task ViewCustomerReports()
        {
            CustomerBL CustomerBL = new CustomerBL();
            List<Customer> Customers = await CustomerBL.GetAllCustomersBL();
            List<CustomerReport> Customerreports = new List<CustomerReport>();
            CustomerReport item;
            foreach (var Customer in Customers)
            {
                item = await CustomerBL.GetCustomerReportByRetailIDBL(Customer.CustomerID);
                Customerreports.Add(item);

            }
            WriteLine("#\tCustomer Name\t no.of orders placed\t Total amount spent on orders");
            int serial = 0;
            foreach (var report in Customerreports)
            {
                serial++;
                WriteLine($"{serial}\t{report.CustomerName}\t{report.CustomerSalesCount}\t{report.CustomerSalesAmount}");
            }



        }
        /// <summary>
        /// Adds Product.
        /// </summary>
        /// <returns></returns>
        public static async Task AddProduct()
        {
            try
            {
                //Read inputs
                Product product = new Product();
                Write("Product Name: ");
                product.ProductName = ReadLine();
                Write("Product Category: ");

                Write("Product Price: ");
                product.ProductPrice = double.Parse(ReadLine());
                Write("Product Color: ");
                product.ProductColor = ReadLine();
                Write("Product Size: ");
                product.ProductSize = ReadLine();
                Write("Product Material: ");
                product.ProductMaterial = ReadLine();

                //Invoke AddSystemUserBL method to add
                using (IProductBL productBL = new ProductBL())
                {
                    bool isAdded = await productBL.AddProductBL(product);
                    if (isAdded)
                    {
                        WriteLine("Product Added!");
                    }
                }
            }
            catch (System.Exception ex)
            {
                ExceptionLogger.LogException(ex);
                WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// views Customers profile
        /// </summary>
        /// <returns></returns>
        public static async Task ViewCustomersProfile()
        {
            CustomerBL CustomerBL = new CustomerBL();
            List<Customer> Customers = await CustomerBL.GetAllCustomersBL();
            WriteLine("#\tCustomer Name\t Retialer Email\tCustomer Phone no.");
            int serial = 0;
            foreach (var Customer in Customers)
            {
                serial++;
                WriteLine($"{serial}\t{Customer.CustomerName}\t{Customer.Email}\t{Customer.CustomerMobile}");
            }

        }

    }
}
