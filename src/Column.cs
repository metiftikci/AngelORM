namespace AngelORM
{
    public class Column
    {
        public bool IsPrimaryKey { get; set; }
        public bool IsForeignKey { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
    }
}
