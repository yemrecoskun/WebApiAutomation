using System.Collections.Generic;
namespace WebApiAutomation.Models
{
    public partial class FastpayToolsEntities : FileEntitiesBase
    {
        public List<EndpointTable> EndpointTable { get; set; }
        public List<Sort> Sort { get; set; }
    }
}