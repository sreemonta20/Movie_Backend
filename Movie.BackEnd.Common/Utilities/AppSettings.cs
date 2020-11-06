using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Movie.BackEnd.Common.Utilities
{
    public class AppSettings
    {

        private IConfiguration _appsettings;
        public string MainConnString { get; set; }
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }

        public static string CompanyId { get; set; }
        public AppSettings(IConfiguration appsettings)
        {
            this._appsettings = appsettings;
            AttributeInitializer();
        }

        private void AttributeInitializer()
        {
          
            this.MainConnString = ConnectionParameter.MainConnection;
            this.SecretKey = this._appsettings.GetSection("AppSettings").GetSection("SecretKey").Value;
            this.Issuer = this._appsettings.GetSection("AppSettings").GetSection("Issuer").Value;
            this.Audience = this._appsettings.GetSection("AppSettings").GetSection("Audience").Value;
            this.Subject = this._appsettings.GetSection("AppSettings").GetSection("Subject").Value;
        }

        public static void ModelSetAuditFieldsForAsync<T>(T model, DateTime dateTime, bool IsAdded, bool includeCompany) where T : class
        {
            Dictionary<String, Object> fields = null;
            try
            {
                var properties = TypeDescriptor.GetProperties(model);

                if (IsAdded)
                {
                    if (includeCompany)
                    {
                        fields = new Dictionary<String, Object>{
                        {"CreatedBy", AppUser.AppUserCode},
                        {"CreatedDate", dateTime},
                        {"UpdatedBy", AppUser.AppUserCode},
                        {"UpdatedDate", dateTime}
                        };
                    }
                    else
                    {
                        fields = new Dictionary<String, Object>{

                        {"CreatedBy", AppUser.AppUserCode},
                        {"CreatedDate", dateTime},
                        {"UpdatedBy", AppUser.AppUserCode},
                        {"UpdatedDate", dateTime}
                        };
                    }

                    ModelSetAuditFields(model, properties, fields);
                }
                else
                {
                    if (includeCompany)
                    {
                        fields = new Dictionary<String, Object>{
                        {"UpdatedBy", AppUser.AppUserCode},
                        {"UpdatedDate", dateTime}
                        };
                    }
                    else
                    {
                        fields = new Dictionary<String, Object>{

                        {"UpdatedBy", AppUser.AppUserCode},
                        {"UpdatedDate", dateTime}
                        };
                    }
                    ModelSetAuditFields(model, properties, fields);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void ModelSetAuditFields<T>(ref T model, DateTime dateTime, bool IsAdded, bool includeCompany) where T : class
        {
            Dictionary<String, Object> fields = null;
            try
            {
                var properties = TypeDescriptor.GetProperties(model);

                if (IsAdded)
                {
                    if (includeCompany)
                    {
                        fields = new Dictionary<String, Object>{
                        {"CreatedBy", AppUser.AppUserCode},
                        {"CreatedDate", dateTime},
                        {"UpdatedBy", AppUser.AppUserCode},
                        {"UpdatedDate", dateTime}
                        };
                    }
                    else
                    {
                        fields = new Dictionary<String, Object>{

                        {"CreatedBy", AppUser.AppUserCode},
                        {"CreatedDate", dateTime},
                        {"UpdatedBy", AppUser.AppUserCode},
                        {"UpdatedDate", dateTime}
                        };
                    }
                    
                    ModelSetAuditFields(model, properties, fields);
                }
                else
                {
                    if (includeCompany)
                    {
                        fields = new Dictionary<String, Object>{
                        {"CreatedBy", AppUser.AppUserCode},
                        {"CreatedDate", dateTime},
                        {"UpdatedBy", AppUser.AppUserCode},
                        {"UpdatedDate", dateTime}
                        };
                    }
                    else
                    {
                        fields = new Dictionary<String, Object>{

                       
                        {"CreatedBy", AppUser.AppUserCode},
                        {"CreatedDate", dateTime},
                        {"UpdatedBy", AppUser.AppUserCode},
                        {"UpdatedDate", dateTime}
                        };
                    }
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

        
    }
}