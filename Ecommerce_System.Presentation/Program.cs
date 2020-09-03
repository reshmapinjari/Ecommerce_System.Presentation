using System;
using System.Threading.Tasks;
using static System.Console;
using Console_Ecommerce_System.BusinessLayer;
using Console_Ecommerce_System.Exceptions;
using Console_Ecommerce_System.Helpers;
using Console_Ecommerce_System.Entities;
using Console_Ecommerce_System.Contracts;

namespace Console_Ecommerce_System.Presentation
{
    class Program
    {
        /// <summary>
        /// GreatOutdoors page
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns></returns>
        static async Task Main(string[] args)
        {
            try
            {

                int option = 1;
                do
                {
                    int internalChoice = -2;
                    WriteLine("===============GREAT OUTDOORS MANAGEMENT SYSTEM=========================");
                    WriteLine("1.Existing User\n2.Customer Registration");
                    WriteLine("Enter your choice");
                    int choice;
                    bool isValidChoice = int.TryParse(ReadLine(), out choice);
                    if (isValidChoice)
                    {
                        switch (choice)
                        {
                            case 1:
                                do
                                {
                                    //Invoke Login Screen
                                    (UserType userType, IUser currentUser) = await ShowLoginScreen();

                                    //Set current user details into CommonData (global data)
                                    CommonData.CurrentUser = currentUser;
                                    CommonData.CurrentUserType = userType;

                                    //Invoke User's Menu
                                    if (userType == UserType.Admin)
                                    {
                                        internalChoice = await AdminPresentation.AdminUserMenu();
                                    }
                                   
                                    else if (userType == UserType.Customer)
                                    {
                                        internalChoice = await CustomerPresentation.CustomerUserMenu();
                                    }
                                    else if (userType == UserType.Anonymous)
                                    {
                                    }
                                } while (internalChoice != -1);
                                break;

                            case 2:
                                {
                                    await AddCustomer();

                                }
                                break;
                            
                            default:
                                {
                                    WriteLine("Invalid choice");
                                }
                                break;
                        }

                    }
                } while (option == 1);
            }
            catch (System.Exception ex)
            {
                ExceptionLogger.LogException(ex);
                WriteLine(ex.Message);
            }

            WriteLine("Thank you!");
            ReadKey();
        }

        /// <summary>
        /// Login (based on Email and Password)
        /// </summary>
        /// <returns></returns>
        static async Task<(UserType, IUser)> ShowLoginScreen()
        {
            //Read inputs
            string email, password;
            WriteLine("=====LOGIN=========");
            Write("Email: ");
            email = ReadLine();
            Write("Password: ");
            password = null;
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if ((key.Key != ConsoleKey.Backspace) && (key.Key != ConsoleKey.Enter))
                {
                    password += key.KeyChar;
                    Write("*");
                }
                else
                {
                    Write("\b");
                }
            }
            while (key.Key != ConsoleKey.Enter);
            WriteLine("");

            using (IAdminBL adminBL = new AdminBL())
            {
                //Invoke GetAdminByEmailAndPasswordBL for checking email and password of Admin
                Admin admin = await adminBL.GetAdminByEmailAndPasswordBL(email, password);
                if (admin != null)
                {
                    return (UserType.Admin, admin);
                }
            }

           
            using (ICustomerBL CustomerBL = new CustomerBL())
            {
                //Invoke GetAdminByEmailAndPasswordBL for checking email and password of Admin
                Customer Customer = await CustomerBL.GetCustomerByEmailAndPasswordBL(email, password);
                if (Customer != null)
                {
                    return (UserType.Customer, Customer);
                }
            }

            WriteLine("Invalid Email or Password. Please try again...");
            return (UserType.Anonymous, null);
        }
        private static async Task AddCustomer()
        {
            try
            {

                Customer newCustomer = new Customer();
                Console.WriteLine("Enter Customer Name :");
                newCustomer.CustomerName = Console.ReadLine();
                Console.WriteLine("Enter Phone Number :");
                newCustomer.CustomerMobile = Console.ReadLine();
                Console.WriteLine("Enter Customer's Email");
                newCustomer.Email = Console.ReadLine();
                Console.WriteLine("Enter Customer's Password");
                newCustomer.Password = Console.ReadLine();
                newCustomer.CustomerID = default(Guid);
                CustomerBL rb = new CustomerBL();

                bool CustomerAdded;
                Guid newCustomerGuid;
                (CustomerAdded, newCustomerGuid) = await rb.AddCustomerBL(newCustomer);
                if (CustomerAdded)
                {
                    Console.WriteLine("Customer Added");
                    Console.WriteLine("Your User id is " + newCustomer.Email);

                }
                else
                    Console.WriteLine("Customer Not Added");

            }



            catch (Exceptions.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        //private static async Task AddAdmin()
        //{
        //    try
        //    {

        //        Admin newAdmin = new Admin();
        //        Console.WriteLine("Enter Admin Name :");
        //        newAdmin.AdminName = Console.ReadLine();
        //        //Console.WriteLine("Enter Admin ID :");
        //        //newAdmin.AdminID = Console.ReadLine();
        //        Console.WriteLine("Enter Admin's Email");
        //        newAdmin.Email = Console.ReadLine();
        //        Console.WriteLine("Enter Admin's Password");
        //        newAdmin.Password = Console.ReadLine();
        //        newAdmin.AdminID = default(Guid);
        //        AdminBL rb = new AdminBL();

        //        bool AdminAdded;
        //        Guid newCustomerGuid;
        //        (AdminAdded, newCustomerGuid) = await rb.AdminBL(newAdmin);
        //        if (AdminAdded)
        //        {
        //            Console.WriteLine("Customer Added");
        //            Console.WriteLine("Your User id is " + newAdmin.Email);

        //        }
        //        else
        //            Console.WriteLine("Customer Not Added");

        //    }
        //    catch (GreatOutdoorException ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }



        //}
    }
}