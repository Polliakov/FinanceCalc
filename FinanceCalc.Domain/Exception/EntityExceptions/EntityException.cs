namespace FinanceCalc.Domain.Exceptions.EntityExceptions
{
    public class EntityException : Exception
    {
        public string EntityName { get; set; }
        public object Id { get; set; }

        protected EntityException(string entityName, object id, string message)
            : base($"Entity '{entityName}' ({id}) {message}")
        {
            EntityName = entityName;
            Id = id;
        }
    }
}
