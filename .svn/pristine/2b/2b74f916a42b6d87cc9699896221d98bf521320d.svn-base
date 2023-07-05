using HCP.SMS.DL.Repository;
using HCP.SMS.DO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HCP.SMS.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ConfigMasterDataController : ControllerBase
    {  
        
        /// <summary>
       /// Get Business key.
       /// </summary>
       /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetBusinessKey()
        {
            var ds = await new CommonMethod().GetBusinessKey();
            return Ok(ds);
        }

        /// <summary>
        /// Get Form Detail.
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetFormDetails()
        {
            var ds = await new CommonMethod().GetFormDetails();
            return Ok(ds);
        }
    }
}