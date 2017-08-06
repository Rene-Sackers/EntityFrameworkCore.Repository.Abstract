namespace EntityFramework.Repository.Abstract.Models.Interfaces
{
    public interface IId<TKey>
    {
        TKey Id { get; set; }
    }
}