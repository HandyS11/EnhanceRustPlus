namespace EfCore.Interfaces
{
    public interface IEntity
    {
        public ulong Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
