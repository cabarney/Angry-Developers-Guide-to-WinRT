using SQLite;

namespace SImpleDB
{
    public class Thing
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}