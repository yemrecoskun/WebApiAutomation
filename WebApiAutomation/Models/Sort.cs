namespace WebApiAutomation.Models
{
    using System;

    public  class Sort
    {
        public Sort()
        {

        }
        public Guid ID { get; set; }
        public string SortName { get; set; }
        public Guid EndpointID { get; set; }
        public int RequestSorts { get; set; }

        public  EndpointTable EndpointTable { get; set; }
    }
}
