using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Paises.Controllers
{
    [ApiController]
    [Route("api/paises")]
    // [Produces("application/json")]
    public class PaisesController : ControllerBase
    {
        private readonly string _paisesUrl;
        private readonly string _campos;

        public PaisesController()
        {
            _paisesUrl = $"https://restcountries.com/v3.1/";
            _campos = "?fields=name,capital,population,flags";
        }

        // api/paises
        [HttpGet]
        public Task<ActionResult> Get(){
            string buscar = "all"+ _campos;
            return obtenerPaises(buscar);
        }

        // api/paises/pais/chile
        [HttpGet("pais/{pais}")]
        public   Task<ActionResult> Get([FromRoute]string pais = ""){
            if (pais == "") {
                throw new HttpException(400, "Bad Request");
            }
            string buscar = "name/"+ pais+ _campos;
            return obtenerPaises(buscar);
        }

        // api/paises/capital/santiago
        [HttpGet("capital/{capital}")]
        public Task<ActionResult> GetCapital([FromRoute] string capital = ""){
                if (capital == ""){
                    throw new HttpException(400, "Bad Request");
                }
            string buscar = "capital/" + capital + _campos;
            return obtenerPaises(buscar);
        }

        public async Task<ActionResult> obtenerPaises(string buscar) {
            var request = (HttpWebRequest)WebRequest.Create(_paisesUrl + buscar);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            try{
                using WebResponse response = request.GetResponse();
                using Stream strReader = response.GetResponseStream();
                if (strReader == null){
                    return NotFound();
                }
                using StreamReader objReader = new StreamReader(strReader);
                string responseBody = await objReader.ReadToEndAsync();
                return Ok(responseBody);
            }
            catch (WebException ex){
                return Ok(ex);
            }
        }
    }
}
