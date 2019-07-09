namespace AngelORM.Tests.Models
{
    public class TableWithEnumColumn
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TableWithEnumColumnMode Mode { get; set; }
        public TableWithEnumColumnStatus Status { get; set; }
    }
}
