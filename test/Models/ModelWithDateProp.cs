using System;

namespace AngelORM.Tests.Models
{
    public class ModelWithDateProp
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
