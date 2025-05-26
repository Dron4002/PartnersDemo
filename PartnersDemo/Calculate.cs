using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartnersDemo
{
    public class Calculate
    {
        private string connectionString = "Server=DESKTOP-90NUGUN; Database =Partners; Trusted_Connection = True; TrustServerCertificate=true;";
        public int CalculateMaterialAmount(int productId, int materialId, int quantity, double param1, double param2)
        {
            double coefficient = GetProductCoefficient(productId);
            double defectPercentage = GetMaterialDefectPercentage(productId);
            if(defectPercentage < 0 || coefficient < 0)
            {
                return -1;
            }
            double unitMaterialAmount = param1 * param2 * coefficient;
            double totalMaterialAmount = unitMaterialAmount * quantity * (1 + defectPercentage);
            return (int)Math.Ceiling(totalMaterialAmount);
        }
        private double GetProductCoefficient(int productId)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Coefficient FROM ProductTypes WHERE Id = @ProductId";
                using(SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductId", productId);
                    object result = command.ExecuteScalar();
                    return result != null ? Convert.ToDouble(result) : -1;
                }
            }
        }
        private double GetMaterialDefectPercentage(int materialId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Percentage FROM TypesMaterial WHERE Id = @MaterialId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaterialId", materialId);
                    object result = command.ExecuteScalar();
                    return result != null ? Convert.ToDouble(result)/100 : -1;
                }
            }
        }
    }
}
