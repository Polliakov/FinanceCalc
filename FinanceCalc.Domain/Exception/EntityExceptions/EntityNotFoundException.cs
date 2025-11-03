namespace FinanceCalc.Domain.Exceptions.EntityExceptions
{
    public class EntityNotFoundException(string entityName, object id)
        : EntityException(entityName, id, "was not found.")
    {
    }
}
