using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Ecommerce_System.Entities;

namespace Console_Ecommerce_System.Contracts.DALContracts
{
    public abstract class CartProductDALBase
    {
        //Collection of Carts
        protected static List<CartProduct> cartList = new List<CartProduct>();
        private static string fileName = "cart.json";

        //Methods for CRUD operations
        public abstract bool AddCartProductDAL(CartProduct newCartProduct);
        public abstract List<CartProduct> GetAllCartProductsDAL();
        public abstract CartProduct GetCartProductByCartIDDAL(Guid searchCartID);
        public abstract bool UpdateCartProductDAL(CartProduct updateCartProduct);
        public abstract bool DeleteCartProductDAL(Guid deleteCartProductID);

        /// <summary>
        /// Writes collection to the file in JSON format.
        /// </summary>
        public static void Serialize()
        {
            string serializedJson = JsonConvert.SerializeObject(cartList);
            using StreamWriter streamWriter = new StreamWriter(fileName);
            streamWriter.Write(serializedJson);
            streamWriter.Close();
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
                var systemUserListFromFile = JsonConvert.DeserializeObject<List<CartProduct>>(fileContent);
                if (systemUserListFromFile != null)
                {
                    cartList = systemUserListFromFile;
                }
            }
        }

        /// <summary>
        /// Static Constructor.
        /// </summary>
        static CartProductDALBase()
        {
            Deserialize();
        }
    }
}
