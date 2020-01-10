using Dapper;
using SportsStore.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SportsStore.Managers
{
    public class BaseManager
    {
        protected string _connectionString { get; set; }
        protected string _webRootPath { get; set; }
        public BaseManager(string connectionString, string webRootPath)
        {
            _connectionString = connectionString;
            _webRootPath = webRootPath;
        }

        protected object TransformArrayParametersToTableValuedParameters(object parameters)
        {
            DynamicParameters dynamicParameters = parameters as DynamicParameters;

            if (dynamicParameters == null)
            {
                return parameters;
            }

            bool containsArray = false;

            DynamicParameters dynamicParametersCopy = new DynamicParameters();

            foreach (string parameterName in dynamicParameters.ParameterNames)
            {
                dynamic parameter = ((SqlMapper.IParameterLookup)dynamicParameters)[parameterName];

                if (parameter != null && parameter.GetType().IsArray)
                {
                    containsArray = true;
                    parameter = ToTableValuedParameter(parameter);
                }

                dynamicParametersCopy.Add(parameterName, parameter);
            }

            return containsArray ? dynamicParametersCopy : dynamicParameters;
        }
        protected bool IsPrimitiveType(Type type)
        {
            return type == typeof(string) || !type.IsClass;
        }

        protected bool IsNullable(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        protected DataTable ToTableValuedParameter<T>(T[] elements)
        {
            DataTable table = new DataTable();

            if (IsPrimitiveType(typeof(T)))
            {
                table.Columns.Add("Value", typeof(T));

                foreach (T element in elements)
                {
                    table.Rows.Add(element);
                }

                table.SetTypeName(typeof(T).Name + "Array");
            }
            else
            {
                PropertyInfo[] properties = typeof(T).GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    Type propertyType = property.PropertyType;

                    if (IsNullable(propertyType))
                    {
                        propertyType = Nullable.GetUnderlyingType(propertyType);
                    }

                    table.Columns.Add(property.Name, propertyType);
                }

                foreach (T element in elements)
                {
                    object[] values = properties
                        .Select(property => property.GetValue(element) ?? DBNull.Value)
                        .ToArray();

                    table.Rows.Add(values);
                }

                table.SetTypeName(typeof(T).Name);
            }

            return table;
        }
    }
}
