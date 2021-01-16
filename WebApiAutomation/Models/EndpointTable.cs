namespace WebApiAutomation.Models
{
    using System;
    using System.Collections.Generic;

    public  class EndpointTable
    {
        public EndpointTable()
        {
            this.Sort = new List<Sort>();
        }

        public Guid Id { get; set; }
        public string ServiceName { get; set; }
        public string EndPoint { get; set; }
        public string Request { get; set; }
        public string Header { get; set; }
        public string Response { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string IsDeleted { get; set; }
        public int RequestType { get; set; }
        public string Action { get; set; }
        public  List<Sort> Sort { get; set; }
    }
}
