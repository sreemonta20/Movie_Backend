using Movie.BackEnd.Common.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Movie.BackEnd.Common.Utilities
{
    public static class ExtensionMethods
    {

        #region IsNotNull Methods
        public static Boolean IsNotNull(this BaseModel obj)
        {
            return obj != null;
        }

        public static Boolean IsNotNull(this Object obj)
        {
            return obj != null;
        }
        #endregion

        #region IsNull Methods
        public static Boolean IsNull(this Object obj)
        {
            return obj == null;
        }
        #endregion

        #region NullOrEmpty Methods
        public static Boolean IsNullOrEmpty(this String str)
        {
            return String.IsNullOrEmpty(str);
        }
        public static Boolean IsNotNullOrEmpty(this String str)
        {
            return !String.IsNullOrEmpty(str);
        }
        #endregion

        #region IsFalse
        public static Boolean IsFalse(this Boolean val)
        {
            return !val;
        }
        #endregion

        #region Base Entity Check
        public static Boolean IsBaseEntity(this Type type)
        {
            if (type.BaseType == null) return false;
            if (type.BaseType == typeof(BaseModel)) return true;
            return type.BaseType.IsBaseEntity();
        }
        #endregion

        #region Zero Length Check
        public static Boolean IsZero(this int val)
        {
            return val.Equals(0);
        }

        public static Boolean IsNotZero(this int val)
        {
            return !val.Equals(0);
        }

        public static Boolean MoreThanZero(this int val)
        {
            return val > 0;
        }
        public static Boolean MoreThanOne(this int val)
        {
            return val > 1;
        }
        public static Boolean MoreThanZero(this long val)
        {
            return val > 0;
        }

        #endregion

        #region Negative Value Check
        public static Boolean IsNegative(this int val)
        {
            return val < 0;
        }
        public static Boolean IsNotNegative(this int val)
        {
            return val >= 0;
        }
        #endregion

        #region Entity State Change after Operation
        public static void AcceptChanges<T>(this T list) where T : class
        {
            if (!typeof(T).IsBaseEntity())
            {
                return;
                //throw new Exception("Item in this list must be inherited from BaseModel class.");
            }
            var item = list as BaseModel;
            switch (item.State)
            {
                case ModelState.Deleted:
                //case EntityState.Detached:
                //    list.Remove((T)Convert.ChangeType(item, typeof(T)));
                //    break;
                case ModelState.Modified:
                case ModelState.Added:
                    item.SetUnchanged();
                    break;
            }

        }
        #endregion

        #region Mapfield
        public static Object MapField(this Object value, Type type)
        {
            if (value.GetType() == type) return value;
            if (value == DBNull.Value) return default(Object);
            var uType = Nullable.GetUnderlyingType(type);
            if (uType.IsNull()) uType = type;
            return Convert.ChangeType(value, uType);
        }
        #endregion

        #region String value Equality Check
        public static Boolean IsBaseTypeComponent(this string val)
        {
            return val.Equals("BaseEntity");
        }
        public static bool ContainsWord(this String Source, String ToCheck, StringComparison oComp)
        {
            return Source?.IndexOf(ToCheck, oComp) >= 0;
        }


        #endregion

        #region Reader Close Method
        public static void CloseReader(this IDataReader reader)
        {
            if (reader.IsNotNull() && !reader.IsClosed)
                reader.Close();
        }
        #endregion

        #region Field Remover From Object

        #endregion

        #region Get All Months
        public static List<KeyValuePair<Int32, String>> GetAllMonths()
        {
            var monthList = new List<KeyValuePair<int, String>>();
            for (var i = 1; i <= 12; i++)
            {
                if (DateTimeFormatInfo.CurrentInfo != null)
                    monthList.Add(new KeyValuePair<Int32, String>(i, DateTimeFormatInfo.CurrentInfo.GetMonthName(i)));
            }
            return monthList;
        }
        #endregion

        #region Audit Fields Method
        public static void SetAuditFields(BaseModel model)
        {
            SetAuditFields(model, DateTime.Now);
        }
        public static void SetAuditFields(BaseModel model, DateTime dateTime)
        {

            try
            {
                var properties = TypeDescriptor.GetProperties(model);

                if (model.IsAdded)
                {
                    var fields = new Dictionary<String, Object>
                    {

                        {"AssignedTo", "AssignedTo"},
                        {"AssginedTeam", "AssginedTeam"},
                        {"IsVisible", "IsVisible"},
                        {"CompanyID", "CompanyID"},
                        {"IsActive", "IsActive"},
                        {"CreatedBy", "CreatedBy"},
                        {"CreatedDate", "CreatedDate"},
                        {"UpdatedBy", "UpdatedBy"},
                        {"UpdatedDate", "UpdatedDate"}
                    };
                    SetAuditFields(model, properties, fields);
                }
                else if (model.IsModified)
                {
                    var fields = new Dictionary<String, Object>
                    {
                        //{"UpdatedBy", UserSession.UserName},
                        //{"DateUpdated", dateTime}
                        {"AssignedTo", "AssignedTo"},
                        {"AssginedTeam", "AssginedTeam"},
                        {"IsVisible", "IsVisible"},
                        {"CompanyID", "CompanyID"},
                        {"IsActive", "IsActive"},
                        {"UpdatedBy", "UpdatedBy"},
                        {"UpdatedDate", "UpdatedDate"}
                    };
                    SetAuditFields(model, properties, fields);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static void SetAuditFields(BaseModel model, PropertyDescriptorCollection properties, Dictionary<String, Object> fields)
        {
            try
            {
                foreach (var field in fields)
                {
                    var propInfo = properties.Find(field.Key, true);
                    if (propInfo.IsNull()) continue;
                    propInfo.SetValue(model, field.Value);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region To Get IpAddress
        public static string GetIP()
        {
            return Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
        }
        #endregion

        #region Password Masking
        public static string EncryptWord(string s)
        {
            int num = 13;
            string str = "";
            for (int i = 0; i < s.Length; i++)
            {
                char ch = Convert.ToChar(s.Substring(i, 1));
                str = str + Convert.ToChar((int)(ch + num));
            }
            return str;
        }
        public static string DecryptWord(string s)
        {
            int num = 13;
            string str = "";
            for (int i = 0; i < s.Length; i++)
            {
                char ch = Convert.ToChar(s.Substring(i, 1));
                str = str + Convert.ToChar((int)(ch - num));
            }
            return str;
        }

        public static string getHash(string text)
        {
            
            // SHA512 is disposable by inheritance.  
            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                // Get the hashed string.  
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
        public  static string getSalt()
        {
            byte[] bytes = new byte[128 / 8];
            using (var keyGenerator = RandomNumberGenerator.Create())
            {
                keyGenerator.GetBytes(bytes);
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
        #endregion

        #region Get Model Parameters

        public static PropertyInfo[] GetModelProperties<T>(T model)
        {
            PropertyInfo[] properties = model.GetType().GetProperties().ToArray();
            return properties;
        }

        public static SqlParameter[] MakeSqlParameters<T>(PropertyInfo[] properties, T model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            foreach (PropertyInfo property in properties)
            {
                parameters.Add(new SqlParameter(string.Format("@{0}", property.Name), property.GetValue(model)));
            }

            return ((parameters.Count > 0) ? parameters.ToArray() : null);
        }


        #endregion

        #region Entity Records Log

        public static void ModelSetAuditFields<T>(T model, DateTime dateTime, bool IsAdded) where T: class
        {

            try
            {
                var properties = TypeDescriptor.GetProperties(model);

                if (IsAdded)
                {
                    var fields = new Dictionary<String, Object>
                    {

                        {"CreatedBy", AppUser.AppUserCode},
                        {"CreatedDate", DateTime.Now},
                        {"UpdatedBy", AppUser.AppUserCode},
                        {"UpdatedDate", DateTime.Now}
                    };
                    ModelSetAuditFields(model, properties, fields);
                }
                else
                {
                    var fields = new Dictionary<String, Object>
                    {
                        {"CreatedBy", AppUser.AppUserCode},
                        {"UpdatedBy", AppUser.AppUserCode},
                        {"UpdatedDate", DateTime.Now}
                    };
                    ModelSetAuditFields(model, properties, fields);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void ModelSetAuditFields<T>(T model, PropertyDescriptorCollection properties, Dictionary<String, Object> fields) where T : class
        {
            try
            {
                foreach (var field in fields)
                {
                    var propInfo = properties.Find(field.Key, true);
                    if (propInfo.IsNull()) continue;
                    propInfo.SetValue(model, field.Value);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Event Operation

        public enum EventOperation: int
        {
            New,
            GetAll,
            GetSpecific,
            Save,
            Update,
            Delete,
            Authentication
            
            
        }
        #endregion

        #region Get Mac Address

        public static string GetMacAddress()
        {
            string rawMacAddress = string.Empty;
            string macAddresses = string.Empty;
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces, thereby ignoring any
                // loopback devices etc.
                if (nic.NetworkInterfaceType != NetworkInterfaceType.Ethernet) continue;
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    rawMacAddress += nic.GetPhysicalAddress().ToString();
                    macAddresses = GetFinalMac(rawMacAddress);
                    break;
                }
            }
            return macAddresses;
        }

        static string GetFinalMac(string str)
        {
            int count = 0; StringBuilder builder = new StringBuilder();
            int placeNumber = (str.Length / 2) - 1;
            for (int i = 0; i < str.Length; i++)
            {
                count += 1;
                builder.Append(str[i]);
                if (count % 2 == 0 & str[i] != '\0')
                {
                    if (placeNumber != 0)
                    {
                        builder.Append('-');
                        placeNumber--;
                    }



                }

            }
            return builder.ToString();
        }

        #endregion

        #region Get IP Address

        public static string GetIPAddress()
        {
            string HostName = Dns.GetHostName(); int count = 0; StringBuilder oBuilder = new StringBuilder();
            IPAddress[] ipAdrArray = Dns.GetHostAddresses(HostName);
            
            foreach (IPAddress ip in ipAdrArray)
            {
                count++;
                oBuilder.Append("IP" + count + ": " + ip.ToString());
                oBuilder.Append(" ");
            }
            return Convert.ToString(oBuilder);
        }

        #endregion

        #region Conversion Method For Pagination (In case of Entity Framework Core)
        public static int Skip(ModelCommParam cmncls)
        {
            int skipnumber = 0;
            if (cmncls.pageNumber > 0)
            {
                skipnumber = ((int)cmncls.pageNumber - 1) * (int)cmncls.pageSize;
            }
            return skipnumber;
        }
        #endregion

        #region Get Model State
        public static string GetModelState(Enum mstate)
        {
            string changestate = String.Empty;
            try
            {
                switch (mstate)
                {
                    case ModelState.Unchanged:
                        changestate = "Unchange";
                        break;

                    case ModelState.Added:
                        changestate = "Save";
                        break;

                    case ModelState.Modified:
                        changestate = "Update";
                        break;

                    case ModelState.Deleted:
                        changestate = "Delete";
                        break;
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return changestate;
        }
        #endregion

    }
}
