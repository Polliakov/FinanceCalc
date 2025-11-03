namespace FinanceCalc.Domain.Exceptions.EntityExceptions
{
    public class EntityAlreadyExistsException(string entityName, object id) : EntityException(entityName, id, "already exists.")
    {
    }
}
