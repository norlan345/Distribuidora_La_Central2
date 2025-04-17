using Distribuidora_La_Central.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace Distribuidora_La_Central.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public ProductoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllProductos")]
        public string GetProductos()
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("UsuarioAppCon").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Producto;", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Producto> productoList = new List<Producto>();
            Response response = new Response();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Producto producto = new Producto();
                    producto.codigoProducto = Convert.ToInt32(dt.Rows[i]["codigoProducto"]);
                    producto.descripcion = Convert.ToString(dt.Rows[i]["descripcion"]);
                    producto.cantidad = Convert.ToInt32(dt.Rows[i]["cantidad"]);
                    producto.categoria = Convert.ToString(dt.Rows[i]["categoria"]);
                    producto.descuento = Convert.ToDecimal(dt.Rows[i]["descuento"]);
                    producto.costo = Convert.ToDecimal(dt.Rows[i]["costo"]);
                    producto.bodega = Convert.ToString(dt.Rows[i]["bodega"]);
                    producto.idProveedor = Convert.ToInt32(dt.Rows[i]["idProveedor"]);
                    productoList.Add(producto);
                }
            }

            if (productoList.Count > 0)
                return JsonConvert.SerializeObject(productoList);
            else
            {
                response.StatusCode = 100;
                response.ErrorMessage = "No data found";
                return JsonConvert.SerializeObject(response);
            }
        }
    }
}