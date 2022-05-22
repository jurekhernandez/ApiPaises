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
        private readonly string _baseUrl;
        private readonly string _fields;

        public PaisesController()
        {
            _baseUrl = $"https://restcountries.com/v3.1/";
            // _fields = "?fields=name,capital,population,flags";
            _fields = "";
        }

        // api/paises
        [HttpGet]
        public Task<ActionResult> Get(){
            string search = "all"+ _fields;
            return obtenerPaises(search);
        }

        // api/paises/name/chile
        [HttpGet("name/{country}")]
        public Task<ActionResult> Get([FromRoute]string country = ""){
            if (country == "") {
                throw new HttpException(400, "Bad Request");
            }
            string search = "name/"+ country + _fields;
            return obtenerPaises(search);
        }

        // api/paises/capital/santiago
        [HttpGet("capital/{capital}")]
        public Task<ActionResult> GetCapital([FromRoute] string capital = ""){
            if (capital == ""){
                throw new HttpException(400, "Bad Request");
            }
            string search = "capital/" + capital + _fields;
            return obtenerPaises(search);
        }

        // api/paises/codigo/cl
        [HttpGet("code/{code}")]
        public Task<ActionResult> GetCountry([FromRoute] string code = "")
        {
            if (code == "")
            {
                throw new HttpException(400, "Bad Request");
            }
            string search = "alpha/" + code + _fields;
            return obtenerPaises(search);
        }

        private async Task<ActionResult> obtenerPaises(string search) {
            var request = (HttpWebRequest)WebRequest.Create(_baseUrl + search);
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
